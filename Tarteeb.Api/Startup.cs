//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Brokers.Tokens;
using Tarteeb.Api.Services.Foundations;
using Tarteeb.Api.Services.Foundations.Teams;
using Tarteeb.Api.Services.Foundations.Tickets;
using Tarteeb.Api.Services.Foundations.Users;
using Tarteeb.Api.Services.Orchestrations;
using Tarteeb.Api.Services.Processings;

namespace Tarteeb.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<StorageBroker>();
            RegisterBrokers(services);
            AddFoundationServices(services);
            AddProcessingService(services);
            AddOrchestrationService(services);
            
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo { Title = "Tarteeb.Api", Version = "v1" });
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(config => config.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: "Tarteeb.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }

        private static void RegisterBrokers(IServiceCollection services)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();

            services.AddScoped(typeof(TokenBroker));
        }

        private static void AddFoundationServices(IServiceCollection services)
        {
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITeamService, TeamService>();

            services.AddScoped(typeof(SecurityService));
        }
        private void AddOrchestrationService(IServiceCollection services)
        {
            services.AddTransient<IUserOrchestrationService, UserOrchestrationService>();
        }
        
        private void AddProcessingService(IServiceCollection services)
        {
            services.AddScoped(typeof(UserSecurityService));
            services.AddScoped(typeof(UserProcessingService));
        }

    }
}