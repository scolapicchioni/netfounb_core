using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using CustomerLibrary;

namespace ConsoleApplication
{
    public class Program
    {
        private const string FILENAME = @"./../klanten.json";

        public static void Main(string[] args)
        {
            List<Customer> customers = new List<Customer>(){
                                         new Customer(1,"Hannah","Schoolstraat 4"),
                                         new Customer(2,"Reinier","Kerkstraat 13"),
                                         new Customer(3,"Vera","Langestraat 1"),
                                         new Customer(4,"Gerard","Kortesteeg 123")
                                     };
            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText(FILENAME, json);
            Console.WriteLine("Customers serialized.");
        }
    }
}
