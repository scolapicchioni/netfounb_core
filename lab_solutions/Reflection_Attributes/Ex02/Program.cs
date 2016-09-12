using System;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Ex02
{
    public class Program
    {
        /// <summary>
		/// Een voorbeeldmethode waarvan ik de signature met reflectie als string wil tonen.
		/// </summary>
		public string[] GetData<T, L>(int? getal, string tekst, T target, params L[] metadata)
		{
			return null;
		}

		static void Main(string[] args)
		{
			var assembly =  Program.LoadAssembly(args.Length>0 ? args[0] : "");

			System.Console.WriteLine("Loaded Assembly " + assembly.FullName);
			// Loop door alle types (classes) in de assembly heen
			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				// Haal van elke class de methode op om te kijken of er het [Test]-attribuut op is geplaatst
				var methodes = type.GetMethods();
				foreach (var methode in methodes)
				{
					Console.WriteLine(GetMethodSignature(methode));
				}
			}
		}

		/// <summary>
		/// Laadt een assembly in.
		/// </summary>
		/// <param name="assemblyPath">The assembly pad.</param>
		/// <returns></returns>
		private static Assembly LoadAssembly(string assemblyPath)
		{
			Assembly assembly = null;
			try
			{
				assembly = AssemblyLoadContext.GetLoadContext(Assembly.GetEntryAssembly()).LoadFromAssemblyPath(assemblyPath);
			}
			catch (Exception e)
			{
				Console.WriteLine("Kon assembly niet laden: " + e.ToString() + " " + e.Message);
			}

			return assembly ??  Assembly.GetEntryAssembly();
		}

		private static string GetMethodSignature(MethodInfo methode)
		{
			// Haal het returntype en de naam van de methode op
			StringBuilder builder = new StringBuilder();
			builder.Append(methode.ReturnType.ToString());
			builder.Append(" " + methode.Name);

			// Rekening houden met generics
			if (methode.ContainsGenericParameters)
			{
				builder.Append("<");
				// string.Join() is een hele handige functie waarmee dit soort zaken makkelijk aan elkaar kunnen
				// worden geknoopt zonder dat je zelf nog moeilijk moet gaan doen met dat er na het laatste item 
				// geen komma meer moet volgen.
				// string.Join() roept onder water de ToString()-methode aan, die exact de goede syntax teruggeeft
				builder.Append(string.Join<Type>(", ", methode.GetGenericArguments()));
				builder.Append(">");
			}
			builder.Append("(");
			
			// Doorloop parameters, rekening houdend met het "params" keyword
			var parameters = methode.GetParameters();
			var parameterStrings = new string[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				var paramString = parameters[i].ToString();
				if (parameters[i].IsDefined(typeof(ParamArrayAttribute), false))
				{
					// De StringBuilder is een idealere keus als je strings aan elkaar gaat plakken.
					// In deze situatie weet ik dat het "params" keyword alleen op de laatste parameter 
					// geplakt kan worden, dus deze code wordt maximaal één keer doorlopen. De performancewinst 
					// van het gebruik van de StringBuilder zal daardoor niet heel groot zal worden.
					paramString = "params " + paramString;
				}

				parameterStrings[i] = paramString;
			}

			builder.Append(string.Join(", ", parameterStrings));
			builder.Append(")");

			return builder.ToString();
		}
    }
}
