using System;
using TestLibrary;
using MetaData;
using System.Reflection;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TypeInfo t = (typeof(MyMath)).GetTypeInfo();
            var attributes = t.GetCustomAttributes<CodeChangesAttribute>(false);
            Console.WriteLine("Attributes on Class level");
            foreach (var attribute in attributes)
            {
                Console.WriteLine($"\t{attribute}");
            }
            MemberInfo[] members = t.GetMembers();

            foreach (var member in members)
            {
                Console.WriteLine(member.Name);
                foreach (var attribute in member.GetCustomAttributes<CodeChangesAttribute>())
                {
                    Console.WriteLine($"\t{attribute}");
                }
            }

            //Console.ReadKey();
        }
    }
}
