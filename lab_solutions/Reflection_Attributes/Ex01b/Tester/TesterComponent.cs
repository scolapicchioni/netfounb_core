using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using MetaData;

namespace Tester
{
    public class TesterComponent
    {
        public TesterComponent(string path)
        {
            // Laad assembly in
			var assembly = this.LoadAssembly(path);
			if (assembly == null)
			{
				return;
			}

			// Loop door alle types (classes) in de assembly heen
			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				// Haal van elke class de methode op om te kijken of er het [Test]-attribuut op is geplaatst
				var methodes = type.GetMethods();
				foreach (var methode in methodes)
				{
					// Is het [Test]-attribuut er op geplaatst?
					if(methode.IsDefined(typeof(TestAttribute), false))
					{
						// Test de methode met behulp van de data in het attribuut
						bool werktCorrect = TestMethod(methode);
						if (werktCorrect)
						{
							Console.WriteLine("Methode {0} is getest en werkt correct!", methode.Name);
						}
						else
						{
							Console.WriteLine("Methode {0} is getest en werkt NIET correct!", methode.Name);
						}
					}
				}
			}
        }

        /// <summary>
		/// Laadt een assembly in.
		/// </summary>
		/// <param name="assemblyPath">The assembly pad.</param>
		/// <returns></returns>
		private Assembly LoadAssembly(string assemblyPath)
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

			return assembly;
		}

		/// <summary>
		/// Test of een methode het verwachte resultaat teruggeeft op basis van gedefinieerde waarden in het attribuut.
		/// </summary>
		/// <param name="methode">The methode.</param>
		private bool TestMethod(MethodInfo methode)
		{
			// Maak een nieuwe instantie van de class met de aanname dat er een parameterloze public constructor beschikbaar is
			var constructor = methode.DeclaringType.GetConstructor(new Type[0]);
			var instantie = constructor.Invoke(null);

			// De te testen methode wil mogelijk parameters hebben. Deze zijn opgegeven bij het [Test]-attribute.
			var attributen = methode.GetCustomAttributes(typeof(TestAttribute), false);
			var attribuut = attributen.First(); // Voor deze oefening nemen we aan dat er maar één [Test]-attribuut is gedeclareerd
			var testattribuut = attribuut as TestAttribute;

			// Roep de te testen methode aan
			var result = methode.Invoke(instantie, testattribuut.Parameters);

			// Het resultaat hoort gelijk te zijn aan wat er verwacht wordt
			// Arraywaarden met elkaar vergelijken is iets wat we zelf moeten doen in .NET
			if (result.GetType().IsArray && testattribuut.ExpectedResult.GetType().IsArray)
			{
				var resultArray = result as object[];
				var verwachtResultaatArray = testattribuut.ExpectedResult as object[];
				return resultArray.SmartCompare(verwachtResultaatArray);
			}
			else
			{
				return result == testattribuut.ExpectedResult;
			}
		}
    }
}
