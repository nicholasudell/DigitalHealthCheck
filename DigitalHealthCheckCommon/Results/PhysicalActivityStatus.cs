namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// The health status of a person's physical activity
    /// </summary>
    public enum PhysicalActivityStatus
    {
        /// <summary>
        /// GPPAQ 0
        /// </summary>
        Sedentary,
        /// <summary>
        /// GPPAQ 1
        /// </summary>
        MostlyInactive,
        /// <summary>
        /// GPPAQ 2
        /// </summary>
        MostlyActive,
        /// <summary>
        /// GPPAQ 3+
        /// </summary>
        Active
    }
}