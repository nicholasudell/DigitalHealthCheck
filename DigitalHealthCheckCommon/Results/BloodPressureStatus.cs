namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// The health status of a person's blood pressure.
    /// </summary>
    public enum BloodPressureStatus
    {
        /// <summary> Systolic < 90 or Diastolic < 60 </summary>
        Low,

        /// <summary>
        /// Systolic between 90 and 120 and Diastolic between 60 and 90
        /// </summary>
        Healthy,

        /// <summary>
        /// Systolic &gt; 120 or Diastolic &gt; 80
        /// </summary>
        SlightlyHigh,

        /// <summary>
        /// Systolic &gt; 140 or Diastolic &gt; 90
        /// </summary>
        High,

        /// <summary>
        /// Systolic &gt; 180 or Diastolic &gt; 110
        /// </summary>
        Severe
    }
}