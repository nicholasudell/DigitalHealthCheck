using System;
using System.Threading;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using DigitalHealthCheckWeb.Model.Risks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalHealthCheckWeb.Pages
{
    public class CalculatingModel : HealthCheckPageModel
    {
        private const int Delay = 15;
        private const string BackgroundJobIdKey = "BackgroundJobId";
        private const string CalculatingStartTimeKey = "CalculatingStartTime";
        private const string CalculatingEndTimeKey = "CalculatingMinEndTime";
        private readonly BackgroundWorkerQueue backgroundWorkerQueue;
        private readonly ILogger<CalculatingModel> logger;

        private static readonly object _lock = new object();

        [FromQuery(Name = "checkReady")]
        public bool ManualCheck { get; set; }

        public CalculatingModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow, BackgroundWorkerQueue backgroundWorkerQueue, ILogger<CalculatingModel> logger) :
            base(database, credentialsDecrypter, pageFlow)
        {
            this.backgroundWorkerQueue = backgroundWorkerQueue;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {

            logger.LogInformation("Checking on background task");

            logger.LogInformation($"{BackgroundJobIdKey}: {HttpContext.Session.GetString(BackgroundJobIdKey)}");
            logger.LogInformation($"{CalculatingStartTimeKey}: {HttpContext.Session.GetString(CalculatingStartTimeKey)}");
            logger.LogInformation($"{CalculatingEndTimeKey}: {HttpContext.Session.GetString(CalculatingEndTimeKey)}");

            bool jobStarted;
            Guid jobId;
            DateTime startingTime = DateTime.MinValue;
            DateTime endingTime = DateTime.MinValue;

            lock (_lock)
            {
                jobStarted = Guid.TryParse(HttpContext.Session.GetString(BackgroundJobIdKey), out jobId) &&
                    DateTime.TryParse(HttpContext.Session.GetString(CalculatingStartTimeKey), out startingTime) &&
                    DateTime.TryParse(HttpContext.Session.GetString(CalculatingEndTimeKey), out endingTime);

                if (!jobStarted)
                {
                    var now = DateTime.UtcNow;

                    HttpContext.Session.SetString(CalculatingStartTimeKey, now.ToString());
                    HttpContext.Session.SetString(CalculatingEndTimeKey, now.AddSeconds(Delay).ToString());

                    logger.LogInformation("Queuing background task");

                    var newId = backgroundWorkerQueue.QueueBackgroundWorkItem(async (token, services) =>
                    {
                        await Task.Yield();

                        logger.LogInformation($"Starting background task");

                        var riskScoreService = (IRiskScoreService)services.GetService(typeof(IRiskScoreService));

                        logger.LogInformation($"Got risk service");
                        await riskScoreService.UpdateRisksForCheckAsync(Id, Variant);

                        logger.LogInformation("Completed background task");
                    });

                    HttpContext.Session.SetString(BackgroundJobIdKey, newId.ToString());

                    logger.LogInformation($"Background task {newId} queued.");

                    return Page();
                }
            }

            //Job has started, check progress.

            logger.LogInformation($"Checking on task id: {jobId}");
            logger.LogInformation($"Starting time: {startingTime}");
            logger.LogInformation($"Ending time: {endingTime}");

            if (ManualCheck && (int)((endingTime - startingTime).TotalSeconds) >= Delay)
            {
                // First time the user has clicked "Check now" we reduce the enforced wait to 10 seconds.

                endingTime = startingTime.AddSeconds(10);

                HttpContext.Session.SetString(CalculatingEndTimeKey, endingTime.ToString());
            }

            if (backgroundWorkerQueue.CheckError(jobId))
            {
                logger.LogInformation($"Background worker reports an error, cancelling the task.");

                lock (_lock)
                {
                    HttpContext.Session.SetString(CalculatingStartTimeKey, string.Empty);
                    HttpContext.Session.SetString(CalculatingEndTimeKey, string.Empty);
                    HttpContext.Session.SetString(BackgroundJobIdKey, string.Empty);
                }

                return RedirectWithId("./Error");
            }

            var check = await GetHealthCheckAsync();

            //Because we're calculating this asynchronously, we need to force a reload here,
            //or sometimes we don't get an updated score.

            await Database.Entry(check).ReloadAsync();

            logger.LogInformation($"Starting time: {startingTime}, check calculated date: {check.CalculatedDate}");

            var calculationCompleted = check.CalculatedDate > startingTime.AddSeconds(-5); //Add a little leeway.

            if (calculationCompleted)
            {
                logger.LogInformation($"Calculation completed.");

                if ((endingTime - DateTime.UtcNow).TotalSeconds <= 0)
                {
                    logger.LogInformation($"Wait time managed. Navigating to results.");

                    lock (_lock)
                    {

                        HttpContext.Session.SetString(CalculatingStartTimeKey, string.Empty);
                        HttpContext.Session.SetString(CalculatingEndTimeKey, string.Empty);
                        HttpContext.Session.SetString(BackgroundJobIdKey, string.Empty);
                    }

                    return RedirectWithId("./Results");
                }
                else
                {
                    logger.LogInformation($"Waiting for enough time to pass.");
                }
            }

            return Page();
        }
    }
}