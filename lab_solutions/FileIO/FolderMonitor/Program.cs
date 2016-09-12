using System;
using System.IO;
using System.Threading;

namespace ConsoleApplication
{
    public class Program
    {
        private const string DIRECTORY = @"C:\Temp\";
		private const string VERPLAATSDIRECTORY = @"C:\Temp2\"; // Inclusief \ aan het einde
		private const string LOGFILE = @"C:\Temp\log.txt";

		/// <summary>
		/// Verplaatst een aangemaakt naar een subdirectory
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.IO.FileSystemEventArgs"/> instance containing the event data.</param>
		private static void OnCreate(object sender, FileSystemEventArgs e)
		{
			// Uitzondering: het logbestand en de directory die worden aangemaakt willen we niet verplaatsen
			if(e.Name != "log.txt")
			{
				// Rekening houden met dat het bestand al bestaat
				var nieuweNaam = VERPLAATSDIRECTORY + e.Name;
				
                var i = 0;
                while(File.Exists(nieuweNaam)){
                    nieuweNaam += (++i).ToString();
                }

				// Na het verplaatsen van een bestand lijkt Windows nog een lijntje open te houden. Misschien dat het bestand nog niet volledig 
				// gekopieerd heeft op het moment dat ons event wordt aangeroepen. De Thread.Sleep() hieronder voorkomt een hoop IOExceptions.
				Thread.Sleep(100);

				var fileInfo = new FileInfo(e.FullPath);
				bool kopierenGelukt = false;
				try
				{
					fileInfo.MoveTo(nieuweNaam);
					kopierenGelukt = true;
				}
				catch (IOException ex)
				{
					Console.WriteLine("Kon bestand niet verplaatsen: " + ex.Message);
				}

				// Loggen van activiteit
				if (kopierenGelukt)
				{
					File.AppendAllText(LOGFILE, "[" + DateTime.Now.ToString() + "] " + e.Name + " van " + fileInfo.Length.ToString() + " bytes" + Environment.NewLine);
				}
				else
				{
					File.AppendAllText(LOGFILE, "[" + DateTime.Now.ToString() + "] " + e.Name + " kon niet worden gekopieerd");
				}
			}
		}

		static void Main(string[] args)
		{
			var watcher = new FileSystemWatcher(DIRECTORY);
			watcher.Created += OnCreate;
			watcher.EnableRaisingEvents = true;
			Console.WriteLine("Bezig met directory " + DIRECTORY + " in de gaten te houden...");
			Console.ReadLine();
		}
    }
}
