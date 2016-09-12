using System;
using System.Reflection;

namespace Reflection_Assemblies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            example02();

            
        }

        private static void example02(){
            var type = typeof(Customer).GetTypeInfo();
            var attributes = type.GetCustomAttributes<DeveloperInfo>(false); 
            foreach (var attribute in attributes)
            {
                System.Console.WriteLine($"{attribute.EmailAddress} - {attribute.Revision}");
            }
        }

        private static TypeInfo example01()
        {
            Type typeOfString = typeof(string);
            System.Console.WriteLine(typeOfString.AssemblyQualifiedName);
            System.Console.WriteLine(typeOfString.FullName);

            //this is a new extension method on Type
            TypeInfo info = typeOfString.GetTypeInfo(); //extension method
            System.Console.WriteLine("**************constructors***************");
            foreach (var constructor in info.GetConstructors())
            {
                System.Console.WriteLine($"{constructor.Name}");
            }

            System.Console.WriteLine("**************fields***************");
            foreach (var field in info.GetFields())
            {
                System.Console.WriteLine($"{field.Name} {field.FieldType.Name}");
            }

            System.Console.WriteLine("**************constructors***************");
            foreach (var method in info.GetMethods())
            {
                System.Console.WriteLine($"{method.ReturnType} {method.Name}");
            }

            return info;
        }
    }
}
