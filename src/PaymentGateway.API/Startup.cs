using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.API.CQRS.Handlers;
using PaymentGateway.API.Mappers;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.SimulatorBank;

namespace PaymentGateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddScoped<IPaymentEventDispatcher, MakePaymentEventDispatcherHandler>();
            services.AddScoped<IBankConnector, BankConnector>();
            services.AddInfrastructure();
            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(PaymentToBankPaymentRequest_Mapper));
            services.AddAutoMapper(typeof(PaymentToGetPaymentResponse_Mapper));
            services.AddAutoMapper(typeof(PaymentToMakePaymentResponse_Mapper));
            services.AddControllers();
            //services.AddSecurity();
            //services.AddRateLimiting();
            services.AddAutoDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API");
                option.RoutePrefix = "docs";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}