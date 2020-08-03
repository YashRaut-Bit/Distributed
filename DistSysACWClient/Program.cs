using System;
using System.Collections.Generic;
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
        static User User;
        static HttpClient client;
        static void Main(string[] args)
        {
            
            User = new User();
            client = new HttpClient();
            Console.WriteLine("Hello. What would you like to do?");
            string input = Console.ReadLine();
            if (input == "Exit")
            {
                return;
            }
            Console.WriteLine("...please wait...");
            RunAsync(input).GetAwaiter().GetResult();
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
                RunAsync(input).GetAwaiter().GetResult();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            } while (true);
        }

        static async Task RunAsync(string input)
        {
            client.BaseAddress = new Uri("http://localhost:44307/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                if (input.Contains("TalkBack"))
                {
                    if (input.Contains("Hello"))
                    {
                        await TBHello();
                    }
                    if (input.Contains("Sort"))
                    {
                        string[] seperated = input.Split(' ');
                        await TBSort(seperated[2]);
                    }
                    Console.WriteLine("Invalid TalkBack command");
                }
                else if (input.Contains("User"))
                {
                    if (input.Contains("Get"))
                    {
                        string[] seperated = input.Split(' ');
                        await UserGet(seperated[2]);
                    }
                    if (input.Contains("Post"))
                    {
                        string[] seperated = input.Split(' ');
                        await UserPost(seperated[2]);
                    }
                    if (input.Contains("Set"))
                    {
                        string[] seperated = input.Split(' ');
                        User.Username = seperated[2];
                        User.ApiKey = seperated[3];
                        Console.WriteLine("Stored");
                    }
                    if (input.Contains("Delete"))
                    {
                        if (User.Username == null || User.ApiKey == null)
                        {
                            Console.WriteLine("You need to do a User Post or User Set first");
                        }
                        else
                        {
                            await UserDelete();
                        }
                    }
                    if (input.Contains("Role"))
                    {
                        if (User.ApiKey == null)
                        {
                            Console.WriteLine("You need to do a User Post or User Set first");
                        }
                        else
                        {
                            string[] seperated = input.Split(' ');
                            await UserRole(seperated[2],seperated[3]);
                        }
                    }
                    Console.WriteLine("Invalid User command");
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
                    Console.WriteLine("Invalid Protected command");
                }
                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task TBHello()
        {
            HttpResponseMessage responseMessage = await client.GetAsync("talkback/hello");
            Console.WriteLine(responseMessage);
        }
        
        static async Task TBSort(string numbers)
        {
            string numbersURI = null;
            for (int i = 1; i < numbers.Length; i++)
            {

            }
            numbers = numbers.Remove(numbers.Length - 1, 1);
            numbers = numbers.Remove(0, 1);
            numbers = numbers.Replace(',', ' ');
            foreach (char num in numbers)
            {
                if (!char.IsWhiteSpace(num))
                {
                    numbersURI += "integers=" + num + "&";
                }
            }
            numbersURI = numbersURI.Remove(numbersURI.Length - 1, 1);
            HttpResponseMessage responseMessage = await client.GetAsync("talkback/sort?" + numbersURI);
            Console.WriteLine(responseMessage);
        }

        static async Task UserGet(string username)
        {
            HttpResponseMessage responseMessage = await client.GetAsync("talkback/user/new?username=" + username);
            Console.WriteLine(responseMessage);
        }
        //incomplete
        static async Task UserPost(string username)
        {
            StringContent usernamecontent = new StringContent(username);
            HttpResponseMessage responseMessage = await client.PostAsync("talkback/user/new", usernamecontent);
            Console.WriteLine(responseMessage);
        }

        static async Task UserDelete()
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Delete, "talkback/user/removeuser?username=" + User.Username);
            httpRequest.Headers.Add("ApiKey", JsonConvert.SerializeObject(User.ApiKey));
            HttpResponseMessage responseMessage = await client.SendAsync(httpRequest);
            Console.WriteLine(responseMessage);
        }

        static async Task UserRole(string username, string role)
        {

        }
    }
    #endregion
}
