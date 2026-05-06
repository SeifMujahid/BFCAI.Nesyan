using BFCAI.Nesyan;
using BFCAI.Nesyan.APIs.Extensions;
using BFCAI.Nesyan.Application;
using BFCAI.Nesyan.Infrastructure.Presistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BFCAI.Nesyan.Controllers.Errors;
using BFCAI.Nesyan.APIs.Middlewares;

namespace BFCAI.Nesyan.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.
            builder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = false;
                    options.InvalidModelStateResponseFactory = context =>
                        {
                            var errors = context.ModelState.Where(P => P.Value!.Errors.Count > 0)
                            .Select(P => new ApiValidationErrorResponse.ValidationError
                            {
                                Field = P.Key,
                                Errors = P.Value!.Errors.Select(E => E.ErrorMessage)
                            });
                            return new BadRequestObjectResult(new ApiValidationErrorResponse
                            {
                                Errors = errors

                            });
                        };

                })
                .AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<BFCAI.Nesyan.Application.Abstraction.Services.IoT.ITelemetryNotifier, BFCAI.Nesyan.APIs.Hubs.SignalRTelemetryNotifier>();

            builder.Services.AddPresistenceService(builder.Configuration);
            builder.Services.AddApplicationService();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:AccessKey"]!)),
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        ValidateLifetime = false // No expire date to the token as requested
                    };
                });
            #endregion

            var app = builder.Build();

            #region Database Initialization
            await app.InitializerStoreContextAsync();
            #endregion  

            #region Configure Kestrel Middlewares

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandllerMiddleware>();
            // app.UseHttpsRedirection(); // Commented out to allow ESP32 HTTP traffic without 307 redirects

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<BFCAI.Nesyan.APIs.Hubs.TelemetryHub>("/telemetryHub");

            app.Run();
            #endregion
        }
    }
}
