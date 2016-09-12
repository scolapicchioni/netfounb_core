using System;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool ok = false;
            while(!ok){
                System.Console.WriteLine("Please insert a valid email address: ");
                string email = Console.ReadLine();
                ok = IsValidEmail(email);
            }
            System.Console.WriteLine("Valid email");
        }

        public static bool IsValidEmail(string strIn) {
            if (String.IsNullOrEmpty(strIn))
                return false;
            
            // Return true if strIn is in valid e-mail format.
            try {
                return Regex.IsMatch(strIn,
                        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException) {
                return false;
            }
        }

        private static string cleanInput(string strIn) {
            // Replace invalid characters with empty strings.
            try {
                return Regex.Replace(strIn, @"[^\w\.@-]", "", 
                    RegexOptions.None, TimeSpan.FromSeconds(1.5)); 
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException) {
                return String.Empty;   
            }
        }
    }
}
