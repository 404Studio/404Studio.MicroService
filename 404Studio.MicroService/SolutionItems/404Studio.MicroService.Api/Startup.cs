using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using YH.Etms.Settlement.Api.Infrastructure;
using YH.Etms.Settlement.Api.Infrastructure.AutofacModules;
using YH.Etms.Settlement.Api.Infrastructure.Configuration.AutoMapper;
using YH.Etms.Settlement.Api.Infrastructure.Filters;
using YH.Etms.Settlement.Api.IntegrationEvents.EventHandling;
using YH.Framework.CAP;
using YH.Framework.ServiceAgent;

namespace YH.Etms.Settlement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            #region 配置MVC

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//格式化DateTime
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            }).AddControllersAsServices();

            #endregion

            #region 配置应用程序一般设置
            services.Configure<AppSettings>(Configuration);
            #endregion

            #region 配置AutoMapper
            services.AddAutoMapper(typeof(SettlementAutoMapperProfile).Assembly);

            //var u = new UpdateVehicleCommand();
            //var map = Mapper.Map(u, typeof(UpdateVehicleCommand), typeof(Vehicle));

            #endregion

            #region 配置DbContext

            services.AddDbContext<SettlementContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("Master"),
                    mySqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });

            #endregion

            #region 配置主数据

            services.AddServiceAgent(new AgentConfig()
            {
                Address = Configuration["YHGateway:Address"],//api网关地址
                TryAgainTimes = int.Parse(Configuration["YHGateway:TryAgainTimes"]),//重试次数
                TryAginInterval = int.Parse(Configuration["YHGateway:TryAginInterval"])//重试间隔秒

            });

            #endregion

            #region 配置Swagger

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "越海运输管理系统服务 - 结算模块 HTTP API",
                    Version = "v1",
                    Description = "越海运输管理系统-结算模块微服务",
                    TermsOfService = "Terms Of Service"
                });
                options.DocInclusionPredicate((docName, description) => true);
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "YH.Etms.Settlement.Api.xml");
                options.IncludeXmlComments(xmlPath);
            });

            #endregion

            #region 配置跨域

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            #endregion

            #region 配置CAP
            services.AddYhCap(
                (options) =>
                {
                    options.ConfigCap(new CapConfig()
                    {
                        EndpointName = Configuration["YhCap:EndpointName"]
                    });

                    options.UseEntityFramework<SettlementContext>();

                    options.UseRabbitMQ(config =>
                    {
                        config.HostName = Configuration["RabbitMq:HostName"];
                        config.UserName = Configuration["RabbitMq:UserName"];
                        config.Password = Configuration["RabbitMq:Password"];
                    });
                }, (s) =>
                {
                    //注册事件订阅者
                    //services.AddSingleton<IEventHandler, TestEventHandler>();
                    s.AddTransient<ISubscribeEventHandler, SubscribeEventHandler>();
                    //s.AddTransient<ISettlementPriceMessageEventHanlder, SettlementPriceMessageEventHanlder>();
                });
            #endregion

            #region 配置Ioc

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule(Configuration.GetConnectionString("Slave")));

            return new AutofacServiceProvider(container.Build());

            #endregion


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            #region 日志
            app.UseExceptionless(Configuration);
            loggerFactory.AddExceptionless();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            #endregion
            #region 主数据
            app.UseServiceAgent();
            #endregion

            #region CAP
            app.UseYhCap();
            #endregion

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger("init").LogDebug($"Using PATH BASE '{pathBase}'");
                app.UsePathBase(pathBase);
            }

            app.UseCors("CorsPolicy");

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
                .UseHSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Catalog.API V1");
                    c.DocExpansion("none");
                });
        }
    }
}
