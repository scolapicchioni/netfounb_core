using System.Linq;

namespace Tester
{
    public static class ArrayExtensions
    {
		/// <summary>
		/// Doet een vergelijking tussen twee arrays op basis van de inhoud van de twee arrays.
		/// </summary>
		/// <param name="first">De eerste array.</param>
		/// <param name="second">De tweede array.</param>
		public static bool SmartCompare<T>(this T[] first, T[] second) {
            return (first.Length == second.Length && first.Intersect(second).Count() == first.Length);
		}
    }
}
