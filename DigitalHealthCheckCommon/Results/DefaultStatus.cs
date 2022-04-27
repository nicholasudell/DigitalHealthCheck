namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// Common health status enum for traffic light health reporting.
    /// </summary>
    public enum DefaultStatus
    {
        /// <summary>
        /// Healthy - low risk.
        /// </summary>
        Healthy,
        /// <summary>
        /// Warning - moderate risk.
        /// </summary>
        Warning,
        /// <summary>
        /// Danger - high risk.
        /// </summary>
        Danger
    }
}