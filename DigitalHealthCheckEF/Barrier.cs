using System.Collections.Generic;

namespace DigitalHealthCheckEF
{


    public class Barrier
    {
        public string Category { get; set; }

        public ICollection<HealthCheck> HealthChecks { get; set; }

        public int Id { get; set; }

        public ICollection<Intervention> Interventions { get; set; }

        public int Order { get; set; }

        public string Text { get; set; }
    }
}