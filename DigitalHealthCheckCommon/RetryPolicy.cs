using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// Helper class for re-attempting operations that rely on non-guaranteed resources, such as
    /// network connections or database connections with timeouts.
    /// </summary>
    public static class RetryPolicy
    {
        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <typeparam name="T">The type of return value.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="retryInterval">How long to wait before retrying.</param>
        /// <param name="maxAttemptCount">
        /// The maximum number of attempts to make before throwing an exception.
        /// </param>
        /// <returns>The result of executing the action.</returns>
        /// <exception cref="AggregateException">
        /// An aggregate of all exceptions raised and retried while executing the action, thrown
        /// only if the max attempt count is exceeded.
        /// </exception>
        public static T Retry<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = 3) =>
            Retry(action, new[] { typeof(Exception) }, retryInterval, maxAttemptCount);

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exceptionTypes">
        /// A white list of exceptions to retry the operation on. Any exception raised not in this
        /// list will cancel the retry procedure and throw the exception.
        /// </param>
        /// <param name="retryInterval">How long to wait before retrying.</param>
        /// <param name="maxAttemptCount">
        /// The maximum number of attempts to make before throwing an exception.
        /// </param>
        /// <param name="retryPeriodMultiplier">
        /// After every retry, multiply the retry interval by this amount. This means we wait longer
        /// and longer, the more times it has failed.
        /// </param>
        /// <exception cref="AggregateException">
        /// An aggregate of all exceptions raised and retried while executing the action, thrown
        /// only if the max attempt count is exceeded.
        /// </exception>
        public static void Retry(Action action, IEnumerable<Type> exceptionTypes, TimeSpan retryInterval, int maxAttemptCount = 3, float retryPeriodMultiplier = 1.0f) =>
            Retry(() =>
            {
                action();
                return (object)null;
            }, exceptionTypes, retryInterval, maxAttemptCount, retryPeriodMultiplier);

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <typeparam name="T">The type of return value.</typeparam>
        /// <typeparam name="TException">The type of exception to retry on.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="retryInterval">How long to wait before retrying.</param>
        /// <param name="maxAttemptCount">
        /// The maximum number of attempts to make before throwing an exception.
        /// </param>
        /// <param name="retryPeriodMultiplier">
        /// After every retry, multiply the retry interval by this amount. This means we wait longer
        /// and longer, the more times it has failed.
        /// </param>
        /// <returns>The result of executing the action.</returns>
        /// <exception cref="AggregateException">
        /// An aggregate of all exceptions raised and retried while executing the action, thrown
        /// only if the max attempt count is exceeded.
        /// </exception>
        public static T Retry<T, TException>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = 3, float retryPeriodMultiplier = 1.0f)
            where TException : Exception =>
            Retry(action, new[] { typeof(TException) }, retryInterval, maxAttemptCount, retryPeriodMultiplier);

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exceptionTypes">
        /// A white list of exceptions to retry the operation on. Any exception raised not in this
        /// list will cancel the retry procedure and throw the exception.
        /// </param>
        /// <param name="retryInterval">How long to wait before retrying.</param>
        /// <param name="maxAttemptCount">
        /// The maximum number of attempts to make before throwing an exception.
        /// </param>
        /// <param name="retryPeriodMultiplier">
        /// After every retry, multiply the retry interval by this amount. This means we wait longer
        /// and longer, the more times it has failed.
        /// </param>
        /// <returns>The result of executing the action.</returns>
        /// <exception cref="AggregateException">
        /// An aggregate of all exceptions raised and retried while executing the action, thrown
        /// only if the max attempt count is exceeded.
        /// </exception>
        public static T Retry<T>(Func<T> action, IEnumerable<Type> exceptionTypes, TimeSpan retryInterval, int maxAttemptCount = 3, float retryPeriodMultiplier = 1.0f)
        {
            var currentInterval = retryInterval;

            var exceptions = new List<Exception>();

            for (var attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(currentInterval);
                        currentInterval = TimeSpan.FromSeconds(currentInterval.TotalSeconds * retryPeriodMultiplier);
                    }
                    return action();
                }
                catch (Exception e)
                when (
                    ExceptionIsRetryable(exceptionTypes, e) ||
                    (e.InnerException != null && ExceptionIsRetryable(exceptionTypes, e))
                )
                {
                    exceptions.Add(e);
                }
            }

            throw new AggregateException(exceptions);
        }

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="retryInterval">How long to wait before retrying.</param>
        /// <param name="maxAttemptCount">
        /// The maximum number of attempts to make before throwing an exception.
        /// </param>
        /// <returns>The result of executing the action.</returns>
        /// <exception cref="AggregateException">
        /// An aggregate of all exceptions raised and retried while executing the action, thrown
        /// only if the max attempt count is exceeded.
        /// </exception>
        public static async Task<T> RetryAsync<T>(Func<Task<T>> action, TimeSpan retryInterval, int maxAttemptCount = 3) =>
            await Retry(action, new[] { typeof(Exception) }, retryInterval, maxAttemptCount);

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exceptionTypes">
        /// A white list of exceptions to retry the operation on. Any exception raised not in this
        /// list will cancel the retry procedure and throw the exception.
        /// </param>
        /// <param name="retryInterval">How long to wait before retrying.</param>
        /// <param name="maxAttemptCount">
        /// The maximum number of attempts to make before throwing an exception.
        /// </param>
        /// <param name="retryPeriodMultiplier">
        /// After every retry, multiply the retry interval by this amount. This means we wait longer
        /// and longer, the more times it has failed.
        /// </param>
        /// <returns>The result of executing the action.</returns>
        /// <exception cref="AggregateException">
        /// An aggregate of all exceptions raised and retried while executing the action, thrown
        /// only if the max attempt count is exceeded.
        /// </exception>
        public static async Task RetryAsync(Func<Task> action, IEnumerable<Type> exceptionTypes, TimeSpan retryInterval, int maxAttemptCount = 3, float retryPeriodMultiplier = 1.0f) =>
            await Retry(async () =>
            {
                await action();
                return (object)null;
            }, exceptionTypes, retryInterval, maxAttemptCount, retryPeriodMultiplier);

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="retryInterval">How long to wait before retrying.</param>
        /// <param name="maxAttemptCount">
        /// The maximum number of attempts to make before throwing an exception.
        /// </param>
        /// <param name="retryPeriodMultiplier">
        /// After every retry, multiply the retry interval by this amount. This means we wait longer
        /// and longer, the more times it has failed.
        /// </param>
        /// <returns>The result of executing the action.</returns>
        /// <exception cref="AggregateException">
        /// An aggregate of all exceptions raised and retried while executing the action, thrown
        /// only if the max attempt count is exceeded.
        /// </exception>
        public static async Task<T> RetryAsync<T, TException>(Func<Task<T>> action, TimeSpan retryInterval, int maxAttemptCount = 3, float retryPeriodMultiplier = 1.0f)
            where TException : Exception =>
            await Retry(action, new[] { typeof(TException) }, retryInterval, maxAttemptCount, retryPeriodMultiplier);

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exceptionTypes">
        /// A white list of exceptions to retry the operation on. Any exception raised not in this
        /// list will cancel the retry procedure and throw the exception.
        /// </param>
        /// <param name="retryInterval">How long to wait before retrying.</param>
        /// <param name="maxAttemptCount">
        /// The maximum number of attempts to make before throwing an exception.
        /// </param>
        /// <param name="retryPeriodMultiplier">
        /// After every retry, multiply the retry interval by this amount. This means we wait longer
        /// and longer, the more times it has failed.
        /// </param>
        /// <returns>The result of executing the action.</returns>
        /// <exception cref="AggregateException">
        /// An aggregate of all exceptions raised and retried while executing the action, thrown
        /// only if the max attempt count is exceeded.
        /// </exception>
        public static async Task<T> RetryAsync<T>(Func<Task<T>> action, IEnumerable<Type> exceptionTypes, TimeSpan retryInterval, int maxAttemptCount = 3, float retryPeriodMultiplier = 1.0f)
        {
            var currentInterval = retryInterval;

            var exceptions = new List<Exception>();

            for (var attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(currentInterval);
                        currentInterval = TimeSpan.FromSeconds(currentInterval.TotalSeconds * retryPeriodMultiplier);
                    }
                    return await action();
                }
                catch (Exception e)
                when (
                    ExceptionIsRetryable(exceptionTypes, e) ||
                    (e.InnerException != null && ExceptionIsRetryable(exceptionTypes, e))
                )
                {
                    exceptions.Add(e);
                }
            }

            throw new AggregateException(exceptions);
        }

        private static bool ExceptionIsRetryable(IEnumerable<Type> retryableExceptions, Exception e) =>
            retryableExceptions.Any(x => x.IsAssignableFrom(e.GetType()) || x.IsAssignableFrom(e.InnerException.GetType()));
    }
}