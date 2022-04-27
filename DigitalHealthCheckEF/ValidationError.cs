using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalHealthCheckEF
{
    public class ValidationError
    {
        public int Id {get;set;}
        public HealthCheck HealthCheck { get; set; }

        public string Page {get;set;}

        public string ErrorMessage {get;set;}

        public string ErrorControl {get;set;}

    }
}