using System;
using System.IO;
using LogBook.Cli.Commands;
using LogBook.Database;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogBook.Cli
{
    [HelpOption(Inherited = true)]
    [Command(Description = "Logbook CLI"),
     Subcommand(typeof(PopulateDatabaseCommand))
    ]
    class Program
    {
        public static int Main(string[] args)
        {
            var configuration = CreateConfiguration();
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(PhysicalConsole.Singleton);
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.AddDbContext<LogBookDbContext>(dbContextOptionsBuilder =>
            {
                var connectionString = configuration.GetConnectionString("Default");
                    dbContextOptionsBuilder.UseNpgsql(connectionString,
                            npgsqlDbContextOptionsBuilder =>
                            {
                                npgsqlDbContextOptionsBuilder.UseNetTopologySuite();
                            })
                        
                        .ConfigureWarnings(w => w.Default(WarningBehavior.Log));
                });
            
            var services = serviceCollection.BuildServiceProvider();


            var app = new CommandLineApplication<Program>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);

            var console = (IConsole)services.GetService(typeof(IConsole));

            try
            {
                return app.Execute(args);
            }
            catch (UnrecognizedCommandParsingException ex)
            {
                console.WriteLine(ex.Message);
                return -1;
            }
        }
        
        public static IConfiguration CreateConfiguration()
        {
           var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment}.json", true, true);
            var configuration = builder.Build();
            return configuration;
        }

        public int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("Please specify a command.");
            app.ShowHelp();
            return 1;
        }
    }
}
