using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Patient.Api.Helpers
{
    public class FHIRDatesAttribute : ValidationAttribute
    {

        public override bool IsValid(object? value)
        {
            var fhirDateStrings = (string[])value;

            if (fhirDateStrings == null ||
                fhirDateStrings.Length == 0)
                return false;

            return fhirDateStrings.All(x => Regex.IsMatch(x, Constants.FHIR.FhirDateRegex));
        }
    }
}
