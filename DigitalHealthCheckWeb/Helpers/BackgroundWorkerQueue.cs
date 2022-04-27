using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalHealthCheckWeb.Helpers
{
    public class BackgroundWorkerQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, IServiceProvider, Tuple<Task, Guid>>> workItems = new();
        private readonly ConcurrentDictionary<Guid, bool> erroredJobs = new();
        private readonly SemaphoreSlim signal = new(0);

        public async Task<Func<CancellationToken, IServiceProvider, Tuple<Task, Guid>>> DequeueAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (!workItems.IsEmpty)
                {
                    workItems.TryDequeue(out var workItem);
                    return workItem;
                }

                await signal.WaitAsync(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }

        public Guid QueueBackgroundWorkItem(Func<CancellationToken, IServiceProvider, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            var id = Guid.NewGuid();

            workItems.Enqueue((x, y) => new Tuple<Task, Guid>(workItem(x, y), id));
            signal.Release();

            return id;
        }

        public void RegisterError(Guid jobId)
        {
            erroredJobs.TryAdd(jobId, true);
        }
        public bool CheckError(Guid jobId)
        {
            if(erroredJobs.TryGetValue(jobId, out _))
            {
                erroredJobs.TryRemove(jobId, out _);
                
                return true;
            }

            return false;
        }
    }
}
