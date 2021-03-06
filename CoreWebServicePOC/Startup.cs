﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreWebServicePOC
{
    public class Startup
    {
        private Container container = new Container();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.OutputFormatters.RemoveType<TextOutputFormatter>();
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                options.FormatterMappings.SetMediaTypeMappingForFormat
                    ("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat
                    ("config", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat
                    ("js", MediaTypeHeaderValue.Parse("application/json"));
            })
            .AddXmlSerializerFormatters()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            IntegrateSimpleInjector(services);

        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "POC",
                    Description = "Core Web Service POC",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Brandon Sharp", Email = "bsharp1976@gmail.com", Url = "localhost" }
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath());
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
        private void InitializeContainer(IApplicationBuilder app)
        {            
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);
                        
            SimpleInjectorWrapper iocWrapper = new SimpleInjectorWrapper();
            container = iocWrapper.AddApplicationDependencies(container);
                        
            container.AutoCrossWireAspNetComponents(app);
        }

        private string GetXmlCommentsPath()
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CoreWebServicePOC.xml");
        }
    }
}
