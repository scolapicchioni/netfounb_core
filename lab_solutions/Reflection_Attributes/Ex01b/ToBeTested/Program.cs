using System;

namespace ToBeTested
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var landen = new Country().GetCountries('N');
			foreach(var land in landen)
			{
				Console.WriteLine("Land: " + land);	
			}
        }
    }
}
