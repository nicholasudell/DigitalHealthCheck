using System;

namespace DigitalHealthCheckEF
{


    public class IdentityVariant
    {
        /// <summary>
        /// Id for this gender variant
        /// </summary>
        /// <remarks>
        /// This is the same as the Id field in HealthCheck
        /// </remarks>
        public Guid Id { get; set; }

        public int? AUDIT { get; set; }

        public AUDITFrequency? MSASQ { get; set; }

        public int? GAD2 { get; set; }

        public int? GPPAQ { get; set; }

        public int? HeartAge { get; set; }

        public Sex? Sex { get; set; }
        public bool? PolycysticOvaries { get; set; }

        public double? QDiabetes { get; set; }

        public double? QRisk { get; set; }
        public bool? GestationalDiabetes { get; set; }
    }
}