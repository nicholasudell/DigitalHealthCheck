using System;
using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckEF
{
    [Keyless]
    public class AnonymisedIntervention
    {
        public Guid PatientIdentifier {get;set;}
        public string Category {get;set;}
        public string Barrier {get;set;}
        public string Intervention {get;set;}
        public string Url {get;set;}
    }
}