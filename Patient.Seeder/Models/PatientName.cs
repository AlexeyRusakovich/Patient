using System.ComponentModel.DataAnnotations;

namespace Patient.Seeder.Models
{
    public class PatientName
    {
        public Guid? Id { get; set; }

        public string? Use { get; set; }

        [Required]
        public string Family { get; set; }

        public IEnumerable<string>? Given { get; set; }
    }
}
