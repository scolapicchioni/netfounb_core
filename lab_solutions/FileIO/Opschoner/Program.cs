﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication
{
    public class Program
    {
        static void Main(string[] args)
		{
			// Zoeken naar alle .test1234-bestanden

			// Een aanroep naar Directory.GetFiles() met als derde parameter SearchOption.AllDirectories zou voldoen
			// voor het zoeken naar de bestanden, maar dat gooit een exception als hij een directory gaat doorzoeken 
			// waar hij niet mag wezen.
			Console.WriteLine("Bezig met zoeken...");
			var bestanden = GetFiles("C:\\", "test1234");
			foreach (var bestand in bestanden)
			{
				if (args.Length > 0 && args[0] == "/console")
				{
					Console.WriteLine(bestand);
				}
				else
				{
					File.Delete(bestand);
					Console.WriteLine(bestand + " is verwijderd.");
				}
			}
		}

		/// <summary>
		/// Doorzoekt directories recursief naar bestanden met een bepaalde extensie.
		/// </summary>
		/// <param name="pad">Het pad.</param>
		/// <param name="extensie">De extensie.</param>
		/// <returns></returns>
		private static string[] GetFiles(string pad, string extensie)
		{
			var bestanden = new List<string>();

			// Doorloop alle directories in opgegeven pad en doorzoek deze directories naar bestanden.
			try
			{
				var directories = Directory.GetDirectories(pad);
				foreach (var directory in directories)
				{
					bestanden.AddRange(GetFiles(directory, extensie));
				}
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine("Mag directories niet ophalen in " + pad + ". Exception message: " + e.Message);
			}

			// Klaar met doorlopen van directories in opgegeven pad, zoek nu de bestanden
			try
			{
				bestanden.AddRange(Directory.GetFiles(pad, "*." + extensie));
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine("Mag bestanden niet ophalen in " + pad + ". Exception message: " + e.Message);
			}

			// Opgegeven directory doorzocht, geef bestanden terug
			return bestanden.ToArray();
		}
    }
}
