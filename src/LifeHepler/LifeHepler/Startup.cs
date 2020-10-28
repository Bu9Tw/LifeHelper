using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Model.Crawler;
using Model.GoogleSheet;
using Model.Jwt;
using Model.LineBot;
using Service.Crawler;
using Service.Crawler.Interface;
using Service.GoogleSheet;
using Service.GoogleSheet.Interface;
using System;
using System.Text;

namespace LifeHepler
{
    public class Startup
    {
        readonly string CorsPolicy = "CorsPolicy";


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
            services.Configure<GoogleSheetCredential>(Configuration.GetSection("GoogleSheetCredential"));
            services.Configure<GoogleSheetModel>(Configuration.GetSection("GoogleSheet"));
            services.Configure<OneOFourJobInfoSourceUrlModel>(Configuration.GetSection("OneOFourJobInfoSourceUrl"));
            services.Configure<JwtModel>(Configuration.GetSection("Jwt"));
            services.AddScoped<IGoogleSheetService, GoogleSheetService>();
            services.AddScoped<IOneOFourCrawlerService, OneOFourCrawlerService>();

            services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                            ClockSkew = TimeSpan.Zero
                        };
                    });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, policy =>
                {
                    policy.WithOrigins("http://localhost:8080");
                });
            });

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

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                            .RequireCors(CorsPolicy);
            });

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                !System.IO.Path.HasExtension(context.Request.Path.Value) &&
                !context.Request.Path.Value.StartsWith("/api"))
                {
                    context.Request.Path = "/index.html";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });
        }
    }
}
