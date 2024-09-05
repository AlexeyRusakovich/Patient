using Microsoft.EntityFrameworkCore;
using Patient.Api.Mapper.Configurations;
using Patient.Api.Services;
using Patient.Data.Context;
using Patient.Data.Interfaces;

namespace Patient.Api.Extensions
{
    public static class ServicesConfigurationExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddControllers();

            services.AddAutoMapper(typeof(PatientProfile));

            var connection = configuration?.GetSection("ConnectionStrings")?.GetSection("DefaultConnection")?.Value;
            services.AddDbContext<PatientContext>(options => options.UseSqlServer(connection));

            services.AddTransient<IRepository<Data.Models.Patient>, PatientRepository>();
            services.AddTransient<IByExpressionSearcher<Data.Models.Patient>, PatientRepository>();
            
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
