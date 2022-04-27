namespace DigitalHealthCheckCommon
{

    /// <summary>
    /// The health status of a person's body mass index (BMI)
    /// </summary>
    public enum BodyMassIndexStatus
    {
        /// <summary>
        /// < 16
        /// </summary>
        SeverelyUnderweight,
        /// <summary>
        /// Between 16 and 18.5
        /// </summary>
        Underweight,
        /// <summary>
        /// Between 18.5 and either 23 for some ethnic groups or 25 for others.
        /// </summary>
        Healthy,
        /// <summary>
        /// Between either 23 and 27.5 for some ethnic groups or 25 and 30 for others.
        /// </summary>
        Overweight,
        /// <summary>
        /// Greater than either 27.5 for some ethnic groups or 30 for others.
        /// </summary>
        Obese
    }
}