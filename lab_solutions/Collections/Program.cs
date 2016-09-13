using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        private delegate void PrintListHandler(List<string> testStrings);
        private delegate bool CheckHandler(List<string> testStrings);
        private delegate List<string> FilterHandler(List<string> testStrings);
        private delegate List<double> ToDoublesHandler(List<string> doubleStrings);
        private delegate double AddDoublesHandler(List<double> doubles);

        static void Main(string[] args)
        {
            List<string> testStrings = new List<string>() {"123", "1.23","12.3","Erno","3678","36.78","3.678","-367.8","-3678"};
            
            /* Without delegates
            PrintList(testStrings);
            bool containsNegatives = CheckForNegatives(testStrings);
            bool numbersOnly = CheckForNumbers(testStrings);
            List<string> justDoubles = FilterDoubles(testStrings);
            List<double> toDoubles = ToDoubles(justDoubles);
            double total = AddDoubles(toDoubles);
            */

            //with delegates
            //PrintListHandler ph = PrintList;
            //ph(testStrings);
            //CheckHandler ch = CheckForNegatives;
            //bool containsNegatives = ch(testStrings);
            //ch = CheckForNumbers;
            //bool numberOnly = ch(testStrings);
            //FilterHandler fh = FilterDoubles;
            //List<string> justDoubles = fh(testStrings);
            //ToDoublesHandler dh = ToDoubles;
            //List<double> toDoubles = dh(justDoubles);
            //AddDoublesHandler ah = AddDoubles;
            //double total = ah(toDoubles);

            //with lambda expressions
            //Check for values lt zero
            testStrings.ForEach(s => Console.WriteLine(s));
            if (testStrings.Find(s =>
            {
                double result;
                 double.TryParse(s, out result);
                return result < 0;
            }) != null)
            {
                Console.WriteLine("Contains negative values");
            }

            //check for non double values
            if (testStrings.Find((s) =>
            {
                double result;
                return !double.TryParse(s, out result);
            }) != null)
            {
                Console.WriteLine("Contains non double values");
            }

            //get all values of type double
            List<string> doubles = testStrings.FindAll((s) => IsDouble(s));
            doubles.ForEach((s) => Console.WriteLine(s));

            //create List of doubles
            List<double> doubleList = new List<double>();
            doubles.ForEach((s)=>doubleList.Add(double.Parse(s)));

            //the sum of all doubles.
            Console.WriteLine( doubleList.Sum());

            Console.ReadLine();
        }

        private static bool IsDouble(string s)//predicate used to get all doubles
        {
            double d;
            if (double.TryParse(s, out d))
            {
                int i;
                if (int.TryParse(s, out i))
                {
                    return false;
                }
                return true;
            }
            return false;
        }


        //the next methods are used by the delegates
        private static double AddDoubles(List<double> doubles)
        {
            double total = 0;
            foreach (double d in doubles)
            {
                total += d;
            }
            return total;
        }

        private static List<double> ToDoubles(List<string> justDoubles)
        {
            List<double> doubles = new List<double>();
            foreach (string doubleString in justDoubles)
            {
                double d;
                if (double.TryParse(doubleString, out d))
                {
                    doubles.Add(d);
                }
            }
            return doubles;
        }

        private static List<string> FilterDoubles(List<string> testStrings)
        {
            int i = 0;
            List<string> doubles = new List<string>();
            while (i < testStrings.Count)
            {
                int intResult;
                if (!Int32.TryParse(testStrings[i], out intResult))
                {
                    double doubleResult;
                    if (double.TryParse(testStrings[i], out doubleResult))
                    {
                        doubles.Add(testStrings[i]);
                    }
                }
                i++;
            }
            return doubles;
        }

        private static bool CheckForNumbers(List<string> testStrings)
        {
            int i = 0;
            bool containsNonNumber = false;
            while (i < testStrings.Count && !containsNonNumber)
            {
                double result;
                if (!double.TryParse(testStrings[i], out result))
                {
                    containsNonNumber = true;
                }
                else
                {
                    // ignore numbers
                }
                i++;
            }
            return containsNonNumber;
        }

        private static bool CheckForNegatives(List<string> testStrings)
        {
            int i = 0;
            bool containsNegative = false;
            while (i < testStrings.Count && !containsNegative)
            {
                double result;
                if (double.TryParse(testStrings[i], out result))
                {
                    containsNegative = result < 0;
                }
                else
                {
                    // ignore non-numbers for now
                }
                i++;
            }
            return containsNegative;
        }

        private static void PrintList(List<string> testStrings)
        {
            for (int i = 0; i < testStrings.Count; i++)
            {
                Console.WriteLine(testStrings[i]);
            }
        }
    }
}
