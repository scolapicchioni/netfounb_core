using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            process();
            //debugging();
            //extensions_logging();
        }

/*
had to add in project.json
"dependencies": {
    "Microsoft.Extensions.Logging": "1.0.0",
    "Microsoft.Extensions.Logging.Console": "1.0.0",
    "Microsoft.Extensions.Logging.Debug": "1.0.0",
    "Microsoft.Extensions.Logging.TraceSource": "1.0.0",
    "Microsoft.Extensions.Configuration.FileProviderExtensions": "1.0.0-rc1-final"
  }



  "Microsoft.Extensions.Logging.EventLog": "1.0.0" is incompatible with core app 1.0, only usable under net 451
*/

private static IConfigurationRoot Configuration { get; set;}

        private static void extensions_logging(){
            //https://github.com/aspnet/Logging/blob/master/samples/SampleApp/Program.cs

            //this is for the configuration File

            Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange:true)
				.Build();

            //https://msdn.microsoft.com/en-us/magazine/mt694089.aspx
            
            ILoggerFactory loggerFactory = new LoggerFactory()
                // .WithFilter(
                //     new FilterLoggerSettings(){
                //     { "Microsoft", LogLevel.Warning },
                //     { "System", LogLevel.Warning },
                //     { "ConsoleApplication.Program", LogLevel.Trace }
                // })
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();

#if !NETCOREAPP1_0
            Trace.AutoFlush = true;
            loggerFactory.AddEventLog();
            var testSwitch = new SourceSwitch("ProgramSwitch", "Logging Sample");
            testSwitch.Level = SourceLevels.Verbose;
            loggerFactory.AddTraceSource(testSwitch, new TextWriterTraceListener("ProgramLoggingService.txt"));
#endif
            
            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("This is an INFORMATION about a test of the emergency broadcast system.");
            logger.LogCritical("This is a CRITICAL information about a test of the emergency broadcast system.");
            logger.LogError("This is an ERROR of a test of the emergency broadcast system.");
            logger.LogDebug("This is a DEBUG message of a test of the emergency broadcast system.");
            logger.LogTrace("This is a TRACE message of a test of the emergency broadcast system.");
            logger.LogWarning("This is a WARNING message of a test of the emergency broadcast system.");
            
        }

        private static void debugging(){
            //Had to add the Microsoft Extensions packages in order to use Trace. Debug was already there.
            Trace.Write("Invalid numeric value.");
            Debug.WriteLine("Invalid numeric value.");
            bool myFlag = false;
            Debug.WriteIf(myFlag,"Data input error in Proc1 procedure.");
            Trace.WriteLineIf(myFlag,"Data input error in Proc1 procedure.");
        }

        private static void eventLogging(){
            //not the way to go. Use Microsoft.Extensions.Logging instead
            // EventLog log1 = new EventLog();
            // if (!EventLog.SourceExists("OrdersApp"))
            //     EventLog.CreateEventSource("OrdersApp", "Application");
            // log1.Source = "OrdersApp";
            // log1.WriteEntry("Application startup");
        }

        private static void process()
        {
            foreach (var proc in Process.GetProcesses())
            {
                try
                {
                    System.Console.WriteLine($"{proc.Id} {proc.ProcessName} {proc.MainModule.FileName}");
                }
                catch
                {
                    System.Console.WriteLine($"{proc.Id} {proc.ProcessName}");
                }
            }

            Process currentProcess = Process.GetCurrentProcess();

            System.Console.WriteLine($"{currentProcess.Id} {currentProcess.ProcessName} {currentProcess.MainModule.FileName}");

            Process.Start("notepad");
        }
    }
}
