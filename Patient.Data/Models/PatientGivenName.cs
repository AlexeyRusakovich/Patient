using Microsoft.EntityFrameworkCore;

namespace Patient.Data.Models
{
    [PrimaryKey(nameof(PatientId), nameof(GivenName))]  
    public class PatientGivenName
    {
        public Guid PatientId { get; set; }

        public string GivenName { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
