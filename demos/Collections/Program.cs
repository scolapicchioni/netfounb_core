using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //lists();
            //dictionaries();
            //linkedlists();
            binarytrees();
        }

        private static void binarytrees()
        {
            //had to add the reference in project.json
            System.Collections.Specialized.OrderedDictionary orderedDictionary = 
            new System.Collections.Specialized.OrderedDictionary();
             
            //OrderedDictionary is implemented using a binary tree. 
             
            orderedDictionary.Add("9780596807269", "Programming Entity Framework");
            orderedDictionary.Add("9780596521301", "Windows PowerShell in Action");
            orderedDictionary.Add("9781932394900", "Programming WCF Services");
             
            System.Collections.ICollection keyCollection = orderedDictionary.Keys;
             
            foreach (var item in keyCollection)
            {
                Console.WriteLine("Key: {0}, Value {1}",item,orderedDictionary[item]);
            }

        }

        private static void linkedlists()
        {
            string[] initialCities = { "Veenendaal", "Utrecht" };
            LinkedList<string> cities = new LinkedList<string>(initialCities);
            printLinkedList(cities);
            cities.AddLast("Ede");
            printLinkedList(cities);
            LinkedListNode<string> node = cities.Find("Utrecht");
            cities.AddAfter(node, "Amersfoort");
            printLinkedList(cities);
            cities.Remove("Utrecht");
            printLinkedList(cities);
            cities.RemoveFirst();
            printLinkedList(cities);
        }

        private static void printLinkedList(LinkedList<string> cities)
        {
            System.Console.WriteLine("**********************");
            foreach (string city in cities)
            {
                Console.WriteLine(city); //Amersfoort Ede
            }
        }

        private static void dictionaries()
        {
            Dictionary<string, string> books = new Dictionary<string, string>();
            books.Add("9780596807269", "Programming Entity Framework");
            books.Add("9780596521301", "Windows PowerShell in Action");
            books.Add("9781932394900", "Programming WCF Services");
             
            foreach (var item in books)
            {
                Console.WriteLine("ISBN:{0}, Title:{1}",item.Key,item.Value);
            }
             
            if (books.ContainsKey("9780123743190") == false)
            {
                books.Add("9780123743190", "DW2.0");
            }
            string searchISBN = "9780596807269";
            Console.WriteLine("Book with ISBN:{0} has title:{1}",searchISBN,books[searchISBN]);

        }

        private static void lists()
        {
            System.Collections.Generic.List<string> cities = new List<string>() { "Veenendaal", "Utrecht", "Amersfoort" };
            cities.Add("Ede");
             
            for (int i=0; i < cities.Count; i++ )
            {
                Console.WriteLine(cities[i]);
            }

            if (cities.Contains("Utrecht"))
                Console.WriteLine("Found");

        }
    }
}
