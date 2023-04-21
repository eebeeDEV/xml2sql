
using Xml2SqlImport.Interfaces;
using Xml2SqlImport.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xml2SqlImport.Controllers;
using Xml2SqlImport.Data.Context;
using Xml2SqlImport.Helpers;
using Xml2SqlImport.Helpers.Extensions;

namespace Xml2SqlImport
{


    internal static class Program
    {
        
        public static IConfiguration? Configuration { get; set; }
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>

        public static void Main()
        {   
            Globals.isForTest = true;
            
            MainAsync().GetAwaiter().GetResult();
        }


        private static async Task MainAsync()
        {

            try
            {
                // use appsettings.json as _config _file
                // DON'T FORGET TO SET THE APPSETTINGS.JSON FILE build action to CONTENT and COPY TO OUTPUT directory to ALWAYS
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                Configuration = builder.Build();
                var host = CreateHostBuilder().Build();

                if (host != null)
                {
                    // _inject the logservice into the logExtensions
                    LogExtensions.LogExtensionsConfigure(host.Services.GetService<ILogService>()!);
                    // start the bulkimport
                    //host.Services.GetService<IXml2SqlController>()!.copyXsdFiles();
                    await host.Services.GetService<IXml2SqlController>()!.injectXml();
                    await host.Services.GetService<IXml2SqlController>()!.sendTeamsLastImportDates();
                    await host.Services.GetService<IXml2SqlController>()!.sendTeamsLastKpiDateValues();
                    Environment.Exit(0);

                }
            }
            catch (Exception e)
            {
                //- innerexception: {e.InnerException!.Message}
                var error = $"error: {e.Message}";
                Console.WriteLine(error);
            }
        }

        static IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
        {
            // install the services
            InstallServices(services, Configuration!); 
        });

        private static void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // get the connectionstrings         
            var localConn = configuration.GetConnectionString("LocalConnection");
            var logConn = configuration.GetConnectionString("LogConnection");            
            // create the DBContext
            // to add _db entities: these are defined in the AppDbContext class OnModelCreating
            services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(localConn, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(60).TotalSeconds)));
            services.AddDbContext<LogDbContext>(options => options.UseSqlServer(logConn, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds)));



            // create the dependency injection objects
            services.AddScoped<ILocalDataService, LocalDataService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IXsdService, XsdService>();
            services.AddScoped<IXmlService, XmlService>();
            services.AddScoped<ITeamsService, TeamsService>();
            services.AddScoped<IFileService, FileService>();            
            services.AddSingleton<IXml2SqlController, Xml2SqlController>();
            services.AddSingleton <IAppSettings, AppSettings>(implementationFactory: provider =>
            {
                // load the AppSettings
                var appConf = new AppSettings();
                configuration.GetSection("AppConfig").Bind(appConf);
                return appConf;
            });
            services.AddAutoMapper(typeof(Program));          
            


            
        }


    }
}
