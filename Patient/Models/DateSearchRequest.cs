using Patient.Api.Helpers;

namespace Patient.Api.Models
{
    public class DateSearchRequest
    {
        [FHIRDate]
        public string Date1 { get; set; }

        [FHIRDate]
        public string? Date2 { get; set; }
    }
}
