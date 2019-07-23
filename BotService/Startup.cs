using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotService.Model.Line;
using BotService.Service.API;
using BotService.Service.Google;
using BotService.Service.Line;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            services.AddScoped<ILineService, LineService>();
            services.AddScoped<IGoogleService, GoogleService>();
            services.AddScoped<IAPIService, APIService>();
            services.AddHttpClient("LineMessageAPI", c =>
            {
                c.BaseAddress = new Uri("https://api.line.me/v2/bot/message/");
                c.DefaultRequestHeaders.Add("Authorization", "Bearer l9rA12B/CgX4MF49n9zjTPTeO8P3GRTlGz78lAp9Zls3CUSk9G+q9EAouJeYWuNIwubo/QZ/XfQsLS3+sAwOcskfbu3Z98SONoE+cSwOXLQRHyo5utY4IKwDm+FIxwWqfMmwmgNJOYY3rPgwpXgA+gdB04t89/1O/w1cDnyilFU=");
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
