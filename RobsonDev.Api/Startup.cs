using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using RobsonDev.Api.Helpers;
using RobsonDev.Api.Services;
using RobsonDev.Data.Context;
using RobsonDev.Data.Repositories;
using System.IO;
using System.Linq;

namespace RobsonDev.Api
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

            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                //options.UseSqlite(Configuration.GetConnectionString("Sqlite"));
                options.UseInMemoryDatabase("RobsonDev");
            });

            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllParametersInCamelCase();
                c.EnableAnnotations();

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RobsonDev.Api", Version = "v1" });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var directoryInfo = new DirectoryInfo(basePath);
                directoryInfo.EnumerateFiles("*.xml")
                .ToList()
                .ForEach(file => c.IncludeXmlComments(file.FullName));

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                    {securityScheme , new string[] {}}
               });

            });

            services.AddJWTService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RobsonDev.Api v1");
            });

            app.UsersSeedingStart().ConfigureAwait(false);
            app.PeoplesSeedingStart().ConfigureAwait(false);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
