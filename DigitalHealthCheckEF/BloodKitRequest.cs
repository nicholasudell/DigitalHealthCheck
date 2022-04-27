using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalHealthCheckEF
{
    public class BloodKitRequest
    {
        public int Id { get; set; }
        [Required]
        public HealthCheck Check { get; set; }
        public DateTime DateRequested { get; set; }
        [Required]
        public string Email { get; set; }
    }
}