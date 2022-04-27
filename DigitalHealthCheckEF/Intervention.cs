using System.Collections.Generic;

namespace DigitalHealthCheckEF
{
    public class Intervention
    {
        //If null: This should appear all the time, or has other mechanisms for when to appear.
        public Barrier Barrier { get; set; }

        public string Category { get; set; }

        public ICollection<HealthCheck> HealthChecks { get; set; }

        public int Id { get; set; }

        public string LinkDescription { get; set; }

        public string LinkTitle { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }
    }
}