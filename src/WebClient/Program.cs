using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
      
        static Task Main(string[] args)
        {
            Task postRandom= PostRandomCustomer();
            postRandom.Wait();
            return  GetCustomer();
        }

        private static CustomerCreateRequest RandomCustomer()
        {
            var randomCustomer = new CustomerCreateRequest("Ivan" + new Random().Next(0, int.MaxValue).ToString(),
                                                          "Ivanov" + new Random().Next(0, int.MaxValue).ToString());

            return randomCustomer;
        }

        private static async Task GetCustomer()
        {
            string url = "https://localhost:5001/customers/";
            Console.WriteLine("Вводите ID пользователя");


            long id = 0;
            bool oneMoreTime = true;
            while (oneMoreTime)
            {
                if (!long.TryParse(Console.ReadLine(), out id) && id < 0)
                {
                    Console.WriteLine("Некорректное ID пользователя");

                }
                else
                    oneMoreTime = false;

            }

            using var client = new HttpClient();

            var response = await client.GetAsync(url + id.ToString());
            string responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseText);
            Console.WriteLine($"StatusCode {response.StatusCode.ToString()}");
            Console.ReadKey();
        }

        private static async Task PostRandomCustomer()
        {
            string url = "https://localhost:5001/customers/";
            CustomerCreateRequest customer = RandomCustomer();


            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            using var response = await client.PostAsync(url, data);
            string responseText = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Create a new customer. Id {responseText}" );
         
        }
    }
}