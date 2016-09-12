using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> testValues = new List<string>() { 
                "1.00", "1.0", "3.14", "Pi", "123.45", "12.345", "-7.23", "+7.23", "00.80", "8", "-2,345,678.90", "23,45,67.89" 
            };
           
            testValues.ForEach((s) => checkValid(s));

            Console.ReadLine();
        }


        private static void checkValid(string s)
        {
            var originalColor = Console.ForegroundColor;
            string pattern = @"^-?\d+\.\d{2}$";
            //   pattern = @"^-?\d{1,3}((\d*)|((,\d{3})*))\.\d{2}$";//if time permits            

            if (Regex.IsMatch(s, pattern))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("{0}\tis valid",s);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0}\tNOT valid", s);
            }
            Console.ForegroundColor = originalColor;
        }
    }
}
