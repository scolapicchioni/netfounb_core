using System.IO;

namespace Tester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Om de applicatie "even snel te kunnen draaien" wordt hier een standaardwaarde gedefinieerd als assembly om in te lezen
			string assemblyPad = Path.Combine(Directory.GetCurrentDirectory() , @"\..\ToBeTested\bin\Debug\netcoreapp1.0\ToBeTested.dll");
            
			if (args.Length > 0)
			{
				assemblyPad = args[0];
			}
            System.Console.WriteLine("current directory: " + Directory.GetCurrentDirectory() );
            System.Console.WriteLine("Loading " + assemblyPad);
			new TesterComponent(assemblyPad);
        }
    }
}
