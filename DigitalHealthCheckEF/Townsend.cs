using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalHealthCheckEF
{
    public class Townsend
    {
        [Key]
        [Column(TypeName="varchar(8)")]
        public string Postcode {get;set;}

        [Column(TypeName="varchar(12)")]
        public string Lsoa {get;set;}

        [Column("Townsend", TypeName="decimal(18,13)")]
        public double? Score {get;set;}

        public int? IMDQuintile {get;set;}
    }
}