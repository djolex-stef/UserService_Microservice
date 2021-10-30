using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommunicationKeyAuthClassLibrary;
using FluentValidation;
using FluentValidation.AspNetCore;
using LoggingClassLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.Data;
using UserService.Entities;
using UserService.Models.Country;
using UserService.Models.Roles;
using UserService.Models.Users;
using UserService.Services;
using UserService.Services.Countries;
using UserService.Services.Roles;
using UserService.Services.Users;
using UserService.Validators;

namespace UserService
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
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }
            ).AddXmlDataContractSerializerFormatters()
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<Startup>())
            .ConfigureApiBehaviorOptions(setupAction => 
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                  
                    ProblemDetailsFactory problemDetailsFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();

                    ValidationProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                        context.HttpContext,
                        context.ModelState);

                    problemDetails.Detail = "Check error field for details";
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    var actionExecutiongContext = context as ActionExecutingContext;

                    if ((context.ModelState.ErrorCount > 0) &&
                        (actionExecutiongContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                    {
                        problemDetails.Type = "https://google.com";
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Title = "Failed to validate";

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    }

                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Failed to parse the content";
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            }); 
            services.AddDbContext<UserDbContext>();
            services.AddDbContext<IdentityUserDbContext>();

            services.AddIdentity<AccountInfo, AccountRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<IdentityUserDbContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<ILoggerProvider, LoggerProvider>();

            services.AddScoped<IPersonalUserRepository, PersonalUserRepository>();
            services.AddScoped<ICorporationUserRepository, CorporationUserRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IPersonalUsersService, PersonalUsersService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ICorporationUsersService, CorporationUsersService>();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("UserServiceOpenApiSpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "User Service API",
                        Version = "1",
                        Description = "API for creating, updating and fetcing users, roles and countries",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Djordje Stefanovic",
                            Email = "djolex3211@mail.com",
                        }
                    });
                
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, ILoggerProvider loggerProvider, ILogger logger)
        {
            loggerFactory.AddProvider(loggerProvider);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An error has occurred. Please try again.");
                    });
                });
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/UserServiceOpenApiSpecification/swagger.json", "User API");
                setupAction.RoutePrefix = "";
            });

            app.UseMiddleware<CommunicationKeyAuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
