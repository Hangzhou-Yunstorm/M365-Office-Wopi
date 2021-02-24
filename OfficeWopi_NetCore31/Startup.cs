using Hangfire;
using Hangfire.Storage.SQLite;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace OfficeWopi_NetCore31
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            repository = LogManager.CreateRepository("OfficeWOPI_NetCore");
            //ָ�������ļ�     
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        public static ILoggerRepository repository { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //�����ļ��ϴ�����С
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1024_000_000;
            });

            // ���ÿ�����
            services.AddCors(option => option.AddPolicy("Cors", policy =>
              policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

            //���ÿ���ͬ�������ȡ������
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            // Hangfire
            services.AddHangfire(configuration => configuration
              .UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSQLiteStorage());

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            // �������
            app.UseCors("Cors");

            app.UseMiddleware<CorsMiddleware>();

            app.UseAuthorization();

            // Hangfire
            app.UseHangfireServer();
            var options = new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            };
            app.UseHangfireDashboard("/hangfire", options);

            UtilHelper.StartHanfireWork(env.ContentRootPath);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
