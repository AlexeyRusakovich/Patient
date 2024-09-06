using Microsoft.Extensions.Configuration;
using Patient.Seeder.Models;
using System.Net.Http.Json;

namespace Patient.Seeder
{
    internal class Program
    {
        static Random _R = new Random();

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            var applicationUrl = config["applicationUrl"];
            if (!string.IsNullOrEmpty(applicationUrl))
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(applicationUrl);
                foreach (var _ in Enumerable.Range(0, 10))
                {
                    var tasks = new List<Task<HttpResponseMessage>>();
                    var tenPatients =  Enumerable.Range(0, 10).Select(x => CreatePatient());
                    tasks.AddRange(tenPatients.Select(patient => httpClient.PostAsJsonAsync("/Patient", patient)));
                    await Task.WhenAll(tasks);
                    Console.WriteLine("Seeded 10 patients");
                }
            }
            else
            {
                Console.WriteLine($"{nameof(applicationUrl)} can't be null");
            }

        }

        private static PatientDto CreatePatient()
        {
            var patient = new PatientDto()
            {  
                Name = new PatientName
                {
                    Id = Guid.NewGuid(),
                    Family = $"Family name",
                    Use = "official",
                    Given = new[]
                    {
                        "First",
                        "Second"
                    }
                },
                BirthDate = GetDateAround6Month(),
                Gender = RandomEnumValue<Gender>(),
                Active = true
            };

            return patient;
        }

        private static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_R.Next(v.Length));
        }

        private static DateTime GetDateAround6Month()
        {
            return DateTime.Now.AddDays(_R.NextInt64(-90, 90));
        }
    }
}
