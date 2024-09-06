namespace Patient.Api.Helpers
{
    public static class Constants
    {
        public static class FHIR
        {
            // Regex have taken from https://www.hl7.org/fhir/datatypes.html
            public const string DateRegex = "([0-9]([0-9]([0-9][1-9]|[1-9]0)|[1-9]00)|[1-9]000)(-(0[1-9]|1[0-2])(-(0[1-9]|[1-2][0-9]|3[0-1]))?)?";
            public const string DateTimeRegex = "([0-9]([0-9]([0-9][1-9]|[1-9]0)|[1-9]00)|[1-9]000)(-(0[1-9]|1[0-2])(-(0[1-9]|[1-2][0-9]|3[0-1])(T([01][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60)(\\.[0-9]{1,9})?)?)?(Z|(\\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00)?)?)?";
            public const string InstantRegex = "([0-9]([0-9]([0-9][1-9]|[1-9]0)|[1-9]00)|[1-9]000)-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])T([01][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60)(\\.[0-9]{1,9})?(Z|(\\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00))";
            public const string eq = nameof(eq); // the resource value is equal to or fully contained by the parameter value
            public const string ne = nameof(ne); // the resource value is not equal to the parameter value
            public const string gt = nameof(gt); // the resource value is greater than the parameter value
            public const string lt = nameof(lt); // the resource value is less than the parameter value
            public const string ge = nameof(ge); // the resource value is greater or equal to the parameter value
            public const string le = nameof(le); // the resource value is less or equal to the parameter value
            public const string sa = nameof(sa); // the resource value starts after the parameter value
            public const string eb = nameof(eb); // the resource value ends before the parameter value
            public const string ap = nameof(ap); // the resource value is approximately the same to the parameter value.
            public const string FhirDateRegex = $"^((?<Prefix>{eq}|{ne}|{gt}|{lt}|{ge}|{le}|{sa}|{eb}|{ap})?)(?<Date>{DateRegex}|{DateTimeRegex}|{InstantRegex})$";
        }
    }
}
