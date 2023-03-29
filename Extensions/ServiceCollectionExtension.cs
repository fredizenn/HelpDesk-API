using AutoMapper;
using HD_Backend.Data;
using HD_Backend.Data.Entities;
using HD_Backend.Data.Interfaces;
using HD_Backend.Data.Mappings;
using HD_Backend.Data.Services;
using HD_Backend.Filters.ActionFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace HD_Backend.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<HelpDeskDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("HD-Backend")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureMapping(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true;  });
            var mapperConfig = new MapperConfiguration(map =>
            {
                map.AddProfile<DepartmentMappingProfile>();
                map.AddProfile<FacultyMappingProfile>();
                map.AddProfile<TicketMappingProfile>();
                map.AddProfile<UserMappingProfile>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("30SecondsCaching", new CacheProfile
                {
                    Duration = 30
                });
            }).AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Help Desk API",
                    Version = "v1",
                    Description = "Help Desk API Services",
                    Contact = new OpenApiContact
                    {
                        Name = "Fred Akuffo"
                    },
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<HelpDeskDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }


        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateFaculty>();
            services.AddScoped<ValidateTicket>();
            services.AddScoped<ValidateDepartment>();
        }

    }
}
