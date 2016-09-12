using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleApplication
{
    public class Program
    {
        private const string ONDERSTEUNDECOMMANDOS = "Ondersteunde commando's: 'exit', 'ververs', 'start [proces/pad]' en een process ID om te killen";

		private static IConfigurationRoot Configuration { get; set;}
		static void Main(string[] args)
		{

			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();
			
			ILoggerFactory loggerFactory = new LoggerFactory()
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

			// Print lijst van draaiende processen
			ToonProcessen();
			Console.WriteLine("");
			Console.WriteLine(ONDERSTEUNDECOMMANDOS);

			// Wacht commando af
			string commando = null;
			int processId = -1;
			while (true)
			{
				commando = Console.ReadLine();
				
				if (commando == "ververs")
				{
					ToonProcessen();
					Console.WriteLine("");
					Console.WriteLine(ONDERSTEUNDECOMMANDOS);
				}
				else if (commando == "exit")
				{
					break;
				}
				else if (commando.StartsWith("start "))
				{
					var procesStarter = commando.Substring("start ".Length);
					StartProces(procesStarter);
				}
				else if (int.TryParse(commando, out processId))
				{
					KillProcess(processId);
				}
				else
				{
					Console.WriteLine("Onbekend commando. " + ONDERSTEUNDECOMMANDOS);
				}
			}
		}

		private static void ToonProcessen()
		{
			Console.Clear();

			foreach (var proces in System.Diagnostics.Process.GetProcesses())
			{
				Console.WriteLine("[{0}] {1}", proces.Id, proces.ProcessName);
			}
		}

		/// <summary>
		/// Sluit een proces abrupt af.
		/// </summary>
		/// <param name="processId">De identifier van het proces om te killen.</param>
		private static void KillProcess(int processId)
		{
			Process proces = null;
			try
			{
				proces = Process.GetProcessById(processId);
			}
			catch (ArgumentException e)
			{
				Console.WriteLine("Kon proces niet benaderen: " + e.Message);
				return;
			}

			proces.Kill();
			Console.WriteLine("Proces {0} gekilld.", proces.ProcessName);
			Trace.WriteLine("Proces gekilld: " + proces.ProcessName);
		}

		/// <summary>
		/// Starts een proces.
		/// </summary>
		/// <param name="procesStarter">Een string waarmee een proces gestart kan worden.</param>
		private static void StartProces(string procesStarter)
		{
			try
			{
				Process.Start(procesStarter);
			}
			catch (Exception e)
			{
				Console.WriteLine("Kon proces niet starten: " + e.Message);
				return;
			}
			Console.WriteLine("Proces {0} gestart.", procesStarter);
			Trace.WriteLine("Proces gestart: " + procesStarter);
		}
    }
}
