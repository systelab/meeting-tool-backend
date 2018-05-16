namespace Main
{
    using System.IO;
    using System.Text;

    using AutoMapper;

    using Main.Models;
    using Main.Services;
    using Main.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;
    using Microsoft.IdentityModel.Tokens;

    using Newtonsoft.Json.Serialization;

    using Swashbuckle.AspNetCore.Swagger;
    using System.Collections.Generic;


    // This is 
    public class Startup
    {
        private readonly IConfigurationRoot config;

        private readonly IHostingEnvironment env;

        public Startup(IHostingEnvironment _env)
        {
            this.env = _env;
            var builder = new ConfigurationBuilder().SetBasePath(this.env.ContentRootPath)
                .AddJsonFile("appsettings.json").AddEnvironmentVariables();
            this.config = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory factory)
        {
            // Configure how to display the errors and the level of severity
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                factory.AddDebug(LogLevel.Information);
            }
            else
            {
                factory.AddDebug(LogLevel.Error);
            }

            if (!env.IsEnvironment("Testing"))
            {
                factory.AddDebug(LogLevel.Information);
                app.UseSwagger();
            }
            else
            { 
                factory.AddConsole();
            }
            

            app.UseCors("MyPolicy");

            app.UseStaticFiles();

            app.UseMvc(
                config =>
                    {
                        config.MapRoute(
                            name: "Default",
                            template: "{controller}/{action}/{id?}",
                            defaults: new { controller = "Home", action = "index" });
                    });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meetings Tool"); });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.config);

                // Add Swagger reference to the project. Swagger is not needed when testing
                services.AddSwaggerGen(
                    c =>
                        {
                            c.SwaggerDoc(
                                "v1",
                                new Info
                                    {
                                        Version = "v1",
                                        Title = "Meetings Tool",
                                        Description = "This is a tool for control the meetings in the meetings rooms",
                                        TermsOfService = "None",
                                    });
                            // Set the comments path for the Swagger JSON and UI.
                            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                            var xmlPath = Path.Combine(basePath, "seed_dotnet.xml");
                            c.IncludeXmlComments(xmlPath);
                        });
            
            

            // Allow use the API from other origins 
            services.AddCors(
                o => o.AddPolicy(
                    "MyPolicy",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials(); }));

            // Set the context to the database
            services.AddDbContext<MeetingsContext>();
           
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddLogging();

            var automapConfiguration = new AutoMapper.MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Room, RoomViewModel>().ReverseMap();
                    cfg.CreateMap<RoomViewModel, Room>().ReverseMap();
                    cfg.CreateMap<CheckUpdate, CheckUpdateViewModel>().ReverseMap();
                    cfg.CreateMap<CheckUpdateViewModel, CheckUpdate>().ReverseMap();
                    cfg.CreateMap<Meeting, MeetingViewModel>().ReverseMap();
                    cfg.CreateMap<MeetingViewModel, Meeting>().ReverseMap();
                });

            var mapper = automapConfiguration.CreateMapper();

            services.AddSingleton(mapper);
            services.AddMvc().AddJsonOptions(
                config =>
                    {
                        config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });
   
        }
    }
}