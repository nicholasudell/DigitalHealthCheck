namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// The health status of a person's blood sugar
    /// </summary>
    public enum BloodSugarStatus
    {
        /// <summary> < 42 </summary>
        Healthy,

        /// <summary>
        /// 42-47
        /// </summary>
        Warning,

        /// <summary>
        /// 48-100
        /// </summary>
        Danger,

        /// <summary>
        /// 100+
        /// </summary>
        Severe
    }
}