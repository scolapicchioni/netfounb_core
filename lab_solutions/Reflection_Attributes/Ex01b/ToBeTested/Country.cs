using System.Linq;
using MetaData;

namespace ToBeTested
{
    public class Country
    {
        public Country()
        {
        }
        /// <summary>
		/// Geeft een simpel lijstje van landen terug.
		/// </summary>
		/// <param name="beginletter">De beginletter van de landnaam.</param>
		/// <returns></returns>
		[Test(new string[] { "Nederland", "Nigeria", "Noorwegen", "Nepal" }, 'N')]
		//[Test(new string[] { "Syrië", "Suriname", "Spanje" }, 'S')]
		public string[] GetCountries(char beginletter)
		{
			string[] landen = new string[] { "Nederland", "Nigeria", "Polen", "Syrië", "Frankrijk", "Suriname", "Noorwegen", "België", "Duitsland", "Zweden", "Spanje", "Nepal" };
			return landen.Where(land => land.StartsWith(beginletter.ToString())).ToArray();
		} 
    }
}
