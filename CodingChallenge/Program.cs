using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CodingChallenge
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            //Create logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Starting Application"); 

            //Setup Dependency injection, logging and appsettings
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IBasicSolutionService, BasicSolutionService>();
                    services.AddTransient<IRecursiveSolutionService, RecursiveSolutionService>();
                })
                .UseSerilog()
                .Build();

            //Basic solution
            var basicSolutionservice = ActivatorUtilities.CreateInstance<BasicSolutionService>(host.Services);
            basicSolutionservice.Run();

            //Recursive solution
            var recursiveSolutionservice = ActivatorUtilities.CreateInstance<RecursiveSolutionService>(host.Services);
            recursiveSolutionservice.Run();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
