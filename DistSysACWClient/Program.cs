using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DistSysACWClient
{
    #region Task 10 and beyond
    public class User
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string ApiKey { get; set; }
    }
    class Client
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello. What would you like to do?");
            string input = Console.ReadLine();
            if (input == "Exit")
            {
                return;
            }
            Console.WriteLine("...please wait...");
            string response = ParseInput(input);
            Console.WriteLine(response);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            do
            {
                Console.WriteLine("What would you like to do next?");
                input = Console.ReadLine();
                if (input == "Exit")
                {
                    return;
                }
                Console.WriteLine("...please wait...");
                response = ParseInput(input);
                Console.WriteLine(response);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            } while (true);
        }

        private static string ParseInput(string input)
        {
            if (input.Contains("TalkBack"))
            {
                if (input.Contains("Hello"))
                {
                    
                }
                if (input.Contains("Sort"))
                {

                }
                return "Invalid TalkBack command";
            }
            else if (input.Contains("User"))
            {
                if (input.Contains("Get"))
                {

                }
                if (input.Contains("Post"))
                {

                }
                if (input.Contains("Set"))
                {

                }
                if (input.Contains("Delete"))
                {

                }
                if (input.Contains("Role"))
                {

                }
                return "Invalid User command";
            }
            else if (input.Contains("Protected"))
            {
                if (input.Contains("Hello") && !input.Contains("SHA1") && !input.Contains("SHA256"))
                {

                }
                if (input.Contains("SHA1"))
                {

                }
                if (input.Contains("SHA256"))
                {

                }
                return "Invalid Protected command";
            }
            return "Invalid command";
        }
    }
    #endregion
}
