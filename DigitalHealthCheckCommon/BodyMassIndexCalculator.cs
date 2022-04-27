using DigitalHealthCheckEF;

namespace DigitalHealthCheckCommon
{
    public class BodyMassIndexCalculator : IBodyMassIndexCalculator
    {
        /// <summary>
        /// Calculates the body mass index from height and weight.
        /// </summary>
        /// <param name="height">Height in meters</param>
        /// <param name="weight">Weight in kilograms</param>
        /// <returns>
        /// A body mass index calculation.
        /// </returns>
        public double CalculateBodyMassIndex(double height, double weight) =>
            weight / (height * height);
    }

    public static class EthnicityBMIExtensions
    {
        public static bool IsHighBMIRisk(this Ethnicity ethnicity) =>
            ethnicity == Ethnicity.African ||
                ethnicity == Ethnicity.AsianOther ||
                ethnicity == Ethnicity.Bangladeshi ||
                ethnicity == Ethnicity.BlackOther ||
                ethnicity == Ethnicity.Caribbean ||
                ethnicity == Ethnicity.Chinese ||
                ethnicity == Ethnicity.Indian ||
                ethnicity == Ethnicity.Pakistani ||
                ethnicity == Ethnicity.WhiteBlackAfrican ||
                ethnicity == Ethnicity.WhiteBlackCaribbean ||
                ethnicity == Ethnicity.WhiteAsian;
    }
}