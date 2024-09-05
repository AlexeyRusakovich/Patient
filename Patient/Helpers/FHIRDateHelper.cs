using Patient.Api.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Patient.Api.Helpers
{
    public static class FHIRDateHelper
    {
        public static FHIRDate ToFhirDate(this string FhirDateString)
        {
            if (string.IsNullOrEmpty(FhirDateString))
                return null;

            var match = Regex.Match(FhirDateString, Constants.FHIR.FhirDateRegex);
            
            if (match.Groups.Count > 0)
            {
                var prefix = match.Groups["Prefix"].Value;
                var date = match.Groups["Date"].Value;
                return new FHIRDate
                {
                    Prefix = string.IsNullOrEmpty(prefix) ? FHIRDatePrefix.None : Enum.Parse<FHIRDatePrefix>(prefix),
                    Date = DateTime.Parse(date)
                };
            }
            else
            {
                return null;
            }
        }

        public static Expression<Func<Data.Models.Patient, bool>> ToExpression(this FHIRDate fhirDate) =>
            fhirDate switch
            {
                { Prefix: FHIRDatePrefix.Eq } => (x) => x.BirthDate == fhirDate.Date,
                { Prefix: FHIRDatePrefix.Ne } => (x) => x.BirthDate != fhirDate.Date,
                { Prefix: FHIRDatePrefix.Gt } => (x) => x.BirthDate > fhirDate.Date,
                { Prefix: FHIRDatePrefix.Lt } => (x) => x.BirthDate < fhirDate.Date,
                { Prefix: FHIRDatePrefix.Ge } => (x) => x.BirthDate >= fhirDate.Date,
                { Prefix: FHIRDatePrefix.Le } => (x) => x.BirthDate <= fhirDate.Date,
                { Prefix: FHIRDatePrefix.Sa } => (x) => x.BirthDate >= fhirDate.Date,
                { Prefix: FHIRDatePrefix.Eb } => (x) => x.BirthDate <= fhirDate.Date,
                { Prefix: FHIRDatePrefix.Ap } => (x) => x.BirthDate == fhirDate.Date,
                _ => (x) => x.BirthDate == fhirDate.Date,
            };
    }
}
