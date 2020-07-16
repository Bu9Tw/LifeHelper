using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Model.GoogleSheet;
using Model.LineBot;
using Service.Crawler;
using Service.Crawler.Interface;
using Service.GoogleSheet;
using Service.GoogleSheet.Interface;

namespace LifeHepler
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
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.Configure<LineBotSecretKeyModel>(Configuration.GetSection("LineBot"));
            services.Configure<GoogleSheetCredential>(Configuration.GetSection("installed"));
            services.Configure<GoogleSheetModel>(Configuration.GetSection("GoogleSheet"));
            services.AddScoped<IGoogleSheetService, GoogleSheetService>();
            services.AddScoped<IOneOFourCrawlerService, OneOFourCrawlerService>();
            //每次Call Method都注入一個新的
            //services.AddTransient
            //每個LifeCycle注入一個新的
            //services.AddScoped   
            //只會在站台啟動時注入一個新的
            //services.AddSingleton
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
