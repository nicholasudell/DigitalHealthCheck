using System;
using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckEF
{
    [Keyless]
    public class AnonymisedBarrier
    {
        public Guid PatientIdentifier {get;set;}
        public string Category {get;set;}
        public string Barrier {get;set;}
    }
}