using BFCAI.Nesyan;
using BFCAI.Nesyan.APIs.Extensions;
using BFCAI.Nesyan.APIs.HangfireExtensions;
using BFCAI.Nesyan.APIs.Middlewares;
using BFCAI.Nesyan.Application;
using BFCAI.Nesyan.Application.Services.TreatmentRequests;
using BFCAI.Nesyan.Controllers.Errors;
using BFCAI.Nesyan.Infrastructure.Presistence;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BFCAI.Nesyan.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            builder.Services.AddHangfire(
                config =>config.UseSqlServerStorage(
                builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            builder.Services.AddHangfireServer();
            

            builder.Services.AddScoped<
                BFCAI.Nesyan.Application.Abstraction.Services.IoT.ITelemetryNotifier,
                BFCAI.Nesyan.APIs.Hubs.SignalRTelemetryNotifier>();

            builder.Services.AddPresistenceService(builder.Configuration);
            builder.Services.AddApplicationService();

            // ✅ ADD CORS HERE
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:AccessKey"]!)
                        ),
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        ValidateLifetime = false
                    };
                });

            #endregion

            var app = builder.Build();

            #region Database Initialization
            await app.InitializerStoreContextAsync();
            await app.AutomaticDoctorRemoving();
            #endregion

            #region Configure Middlewares

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<ExceptionHandllerMiddleware>();

            // ❗ IMPORTANT: Use CORS BEFORE Auth
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseHangfireDashboard();
            app.MapControllers();
            app.MapHub<BFCAI.Nesyan.APIs.Hubs.TelemetryHub>("/telemetryHub");

            app.Run();

            #endregion
        }
    }
}