using IslandIs.Skjalaveita.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace IslandIs.Skjalaveita.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add controllers
            services.AddControllers();

            // Make all urls in lowercase.
            services.AddRouting(options => options.LowercaseUrls = true);

            // Add JWT Bearer Authentication 
            var authenticationBuilder = services.AddAuthentication();

            var authorities = Configuration.GetSection("IdPSettings:Authorities").Get<List<string>>();
            List<string> authenticationSchemas = new List<string>();
            int i = 1;

            foreach (var authority in authorities)
            {
                var authSchema = "IdP" + i;
                authenticationSchemas.Add(authSchema);
                // Add all Authorities
                authenticationBuilder.AddJwtBearer(authSchema, options =>
                   {
                       // base-address of the authority server
                       options.Authority = authority;
                       // name of your API.
                       options.Audience = Configuration["IdPSettings:Audience"];
                       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true
                       };
                   }); ;
                i++;
            }

            // Add authorization and optionally set scope validation
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(authenticationSchemas.ToArray())
                        // This is optional (i.e. validating the scope)
                        .RequireClaim("scope", Configuration["IdPSettings:Scope"])
                        .Build();
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Skjalaveita API", Version = "v1" });
            });

            // Create singleton instance of Demo DocumentService.
            services.AddSingleton<IDocumentService, DemoDocumentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Skjalaveita API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
