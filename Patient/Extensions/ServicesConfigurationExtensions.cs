using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Patient.Api.Mapper.Configurations;
using Patient.Api.Services;
using Patient.Data.Context;
using Patient.Data.Interfaces;
using System.Reflection;

namespace Patient.Api.Extensions
{
    public static class ServicesConfigurationExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, ConfigurationManager configuration, bool IsDevelopment)
        {
            services.AddControllers();

            services.AddAutoMapper(typeof(PatientProfile));

            //Db
            var connection = configuration?.GetSection("ConnectionStrings")?.GetSection("DefaultConnection")?.Value;
            if (!IsDevelopment)
            {
                var password = Environment.GetEnvironmentVariable("SA_PASSWORD");
                connection = string.Format(connection, password);
            }
            services.AddDbContext<PatientContext>(options => options.UseSqlServer(connection));

            // Repository
            services.AddTransient<IRepository<Data.Models.Patient>, PatientRepository>();
            services.AddTransient<IByDateSearcher<Data.Models.Patient>, PatientRepository>();

            //Swagger Documentation Section
            var info = new OpenApiInfo()
            {
                Title = "Patient API Documentation",
                Version = "v1",
                Description = "Patient API allows you to create, search, update and delete patients",
                Contact = new OpenApiContact()
                {
                    Name = "Aliaksei",
                    Email = "alexrusakovich1@gmail.com",
                }
            };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddEndpointsApiExplorer();

            return services;
        }

        public static WebApplication CreateDatabaseIfNotExist(this WebApplication app)
        {
            using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<PatientContext>();
                context.Database.EnsureCreated();
            }

            return app;
        }
    }
}
