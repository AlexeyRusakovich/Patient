using Patient.Data.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Patient.Api.Helpers
{
    public static class FHIRDateHelper
    {
        public static FHIRDate ToFhirDate(this string fhirDateString)
        {
            if (string.IsNullOrEmpty(fhirDateString))
                return null;

            var match = Regex.Match(fhirDateString, Constants.FHIR.FhirDateRegex);
            
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
                { Prefix: FHIRDatePrefix.eq } => (x) => x.BirthDate == fhirDate.Date,
                { Prefix: FHIRDatePrefix.ne } => (x) => x.BirthDate != fhirDate.Date,
                { Prefix: FHIRDatePrefix.gt } => (x) => x.BirthDate > fhirDate.Date,
                { Prefix: FHIRDatePrefix.lt } => (x) => x.BirthDate < fhirDate.Date,
                { Prefix: FHIRDatePrefix.ge } => (x) => x.BirthDate >= fhirDate.Date,
                { Prefix: FHIRDatePrefix.le } => (x) => x.BirthDate <= fhirDate.Date,
                { Prefix: FHIRDatePrefix.sa } => (x) => x.BirthDate >= fhirDate.Date,
                { Prefix: FHIRDatePrefix.eb } => (x) => x.BirthDate <= fhirDate.Date,
                { Prefix: FHIRDatePrefix.ap } => (x) => x.BirthDate == fhirDate.Date,
                _ => (x) => x.BirthDate == fhirDate.Date,
            };
    }
}
