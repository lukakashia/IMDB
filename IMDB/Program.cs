using IMDB.DBContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace IMDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .CreateLogger();
            
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddControllers();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomPolicy", policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireClaim("CustomClaimType", "SomeClaimValue");
                  
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerGen(options=>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });



            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();


            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.SignIn.RequireConfirmedEmail = false; 
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value!))
                };
            });
            

            var app = builder.Build();
         
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}