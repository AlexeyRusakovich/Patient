using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient.Data.Models
{
    public class Patient
    {
        [Key]
        public Guid Id { get; set; }

        public string? Use { get; set; }

        [Required]
        public string Family { get; set; }

        public int? Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public bool? Active { get; set; }

        public virtual IEnumerable<PatientGivenName>? GivenNames { get; set; }
    }
}
