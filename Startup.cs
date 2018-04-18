using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ObsApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace ObsApi
{
    public class Startup
    {       
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ObsContext>(opt => opt.UseInMemoryDatabase("ObsList"));
            services.AddDbContext<AzureDbContext>(_ => _.UseSqlServer("Server=observationserver.database.windows.net,1433;Initial Catalog=ObservationFacileDB;Persist Security Info=False;User ID=administrateur;Password=xxxxx;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            services.AddMvc();
            services.AddCors();

            

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                    var context = serviceScope.ServiceProvider.GetRequiredService<AzureDbContext>();
                    
                    //context.Database.EnsureCreated();
            }
                    app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseCors( options => options.WithOrigins("http://localhost:4200").AllowAnyMethod() );
            app.UseMvc();
 
        }
    }
}
