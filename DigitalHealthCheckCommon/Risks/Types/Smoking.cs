namespace QMSUK.QRisk
{
    /// <summary>
    /// Smoking categories
    /// </summary>
    public enum Smoking
    {
        /// <summary>
        /// Non smokers.
        /// </summary>
        NonSmoker = 0,
        /// <summary>
        /// Former smokers.
        /// </summary>
        ExSmoker = 1,
        /// <summary>
        /// Light smokers (0-10 per day).
        /// </summary>
        LightSmoker = 2,
        /// <summary>
        /// Moderate smokers (10-20 per day).
        /// </summary>
        ModerateSmoker = 3,
        /// <summary>
        /// Heavy smokers (20+ per day).
        /// </summary>
        HeavySmoker = 4
    }
}