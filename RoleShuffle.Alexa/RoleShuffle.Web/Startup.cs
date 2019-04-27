using System;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoleShuffle.IoC;
using RoleShuffle.Web.Services;
using RoleShuffle.Web.Validation;

namespace RoleShuffle.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            
            Configuration = builder.Build();
        }
            

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(
            IServiceCollection services)
        {
            services.RegisterDependencies();
            services.AddSingleton(Configuration);
            services.AddSingleton<IAlexaRequestValidator,AlexaRequestValidator>();

            services.AddHostedService<WarmUpService>();
            services.AddHostedService<ConfigurationWorkerService>();
            services.AddMvc();

            services.ConfigureDynamicProxy(config =>
            {
                config.Interceptors.AddDelegate((ctx, next) => next(ctx));
            });

            return services.BuildDynamicProxyServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            app.UsePathBase("/roleservant");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
