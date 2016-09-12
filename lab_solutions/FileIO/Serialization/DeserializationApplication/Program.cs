using System;
using System.Collections.Generic;
using System.IO;
using CustomerLibrary;
using Newtonsoft.Json;

namespace ConsoleApplication
{
    public class Program
    {
        private const string FILENAME = @"./../klanten.json";

        public static void Main(string[] args)
        {
            var json = File.ReadAllText(FILENAME);
            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            foreach(var c in customers){
                System.Console.WriteLine(c);
            }
        }
    }
}
