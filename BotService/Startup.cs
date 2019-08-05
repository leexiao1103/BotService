using BotService.Model.Line;
using BotService.Service.API;
using BotService.Service.Google;
using BotService.Service.Line;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace BotService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Service
            services.AddScoped<ILineService, LineService>();
            services.AddScoped<IGoogleService, GoogleService>();
            services.AddScoped<IAPIService, APIService>();
            services.AddSingleton<ILineDBService, LineDBService>();
            services.AddSingleton<ILineEmoji, LineEmoji>();

            //HttpClient
            services.AddHttpClient("LineMessageAPI", c =>
            {
                c.BaseAddress = new Uri("https://api.line.me/v2/bot/message/");
                c.DefaultRequestHeaders.Add("Authorization", $"Bearer {Configuration.GetValue<string>("LINE:Channel_Access_Token")}");
            });
            services.AddHttpClient("GoogleCustomSearchAPI", c =>
            {
                c.BaseAddress = new Uri("https://www.googleapis.com/customsearch/v1/");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
