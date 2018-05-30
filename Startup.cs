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

        public IConfiguration Configuration { get; private set; }

         public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ObsContext>(opt => opt.UseInMemoryDatabase("ObsList"));
            var connString = Configuration.GetConnectionString("DefaultConnection");
            System.Console.WriteLine($"Voici la config : {connString}");
            var connDocker = Configuration.GetConnectionString("DockerConnection");
            System.Console.WriteLine($"Voici la config : {connDocker}");
            
            services.AddDbContext<AzureDbContext>(_ => _.UseSqlServer(connDocker));
 //           Server=127.0.0.1,4433;Initial Catalog=GeoDB;Persist Security Info=False;User ID=SA;Password=Test@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
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
