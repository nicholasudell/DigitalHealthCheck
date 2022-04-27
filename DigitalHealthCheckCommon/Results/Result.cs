namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// A user's health check represented in risk categories.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// The user's risk of alcohol dependency.
        /// </summary>
        /// <value>
        /// The alcohol status.
        /// </value>
        public DefaultStatus Alcohol { get; set; }

        /// <summary>
        /// The user's risk from their blood pressure.
        /// </summary>
        /// <value>
        /// The blood pressure.
        /// </value>
        public BloodPressureStatus? BloodPressure { get; set; }

        /// <summary>
        /// The user's risk from their blood sugar.
        /// </summary>
        /// <value>
        /// The blood sugar.
        /// </value>
        public BloodSugarStatus? BloodSugar { get; set; }

        /// <summary>
        /// The user's risk from their BMI.
        /// </summary>
        /// <value>
        /// The BMI.
        /// </value>
        public BodyMassIndexStatus BodyMassIndex { get; set; }

        /// <summary>
        /// The user's risk from their cholesterol.
        /// </summary>
        /// <value>
        /// The cholesterol.
        /// </value>
        public DefaultStatus? Cholesterol { get; set; }

        /// <summary>
        /// The user's risk from their cholesterol ratio (HDL / Total)
        /// </summary>
        /// <value>
        /// The cholesterol ratio.
        /// </value>
        public DefaultStatus? CholesterolRatio { get; set; }

        /// <summary>
        /// The user's risk of developing Type 2 diabetes.
        /// </summary>
        /// <value>
        /// The diabetes.
        /// </value>
        public DefaultStatus Diabetes { get; set; }

        /// <summary>
        /// The user's risk from their HDL cholesterol.
        /// </summary>
        /// <value>
        /// The HDL cholesterol.
        /// </value>
        public DefaultStatus? HdlCholesterol { get; set; }

        /// <summary>
        /// The user's risk from their heart age.
        /// </summary>
        /// <value>
        /// The heart age.
        /// </value>
        public DefaultStatus HeartAge { get; set; }

        /// <summary>
        /// The user's risk of developing heart disease.
        /// </summary>
        /// <value>
        /// The heart disease.
        /// </value>
        public DefaultStatus HeartDisease { get; set; }

        /// <summary>
        /// The user's risk from their mental wellbeing.
        /// </summary>
        /// <remarks>
        /// If the user has skipped the mental health questionnaire
        /// this field will be null.
        /// </remarks>
        /// <value>
        /// The mental wellbeing.
        /// </value>
        public DefaultStatus? MentalWellbeing { get; set; }

        /// <summary>
        /// The user's risk from their physical activity.
        /// </summary>
        /// <value>
        /// The physical activity.
        /// </value>
        public PhysicalActivityStatus PhysicalActivity { get; set; }

        /// <summary>
        /// The user's risk from smoking.
        /// </summary>
        /// <value>
        /// The user's smoking status.
        /// </value>
        public DefaultStatus Smoker { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this user is walking.
        /// </summary>
        /// <remarks>
        /// This is used to modify the advice we offer based on a user's
        /// physical activity. This is because GPPAQ does not take into account
        /// a user's walking, but their walking may be helping them and we do
        /// not want them to stop.
        /// </remarks>
        /// <value>
        ///   <c>true</c> if walking; otherwise, <c>false</c>.
        /// </value>
        public bool Walking { get; set; }
    }
}