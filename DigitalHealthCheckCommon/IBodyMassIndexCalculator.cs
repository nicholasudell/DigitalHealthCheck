namespace DigitalHealthCheckCommon
{
    public interface IBodyMassIndexCalculator
    {
        /// <summary>
        /// Calculates the body mass index from height and weight.
        /// </summary>
        /// <param name="height">Height in meters</param>
        /// <param name="weight">Weight in kilograms</param>
        /// <returns>A body mass index calculation.</returns>
        double CalculateBodyMassIndex(double height, double weight);
    }
}