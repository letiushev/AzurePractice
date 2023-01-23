using System;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {
        Todos todos = new Todos();
        // Webpage URL
        string url = "https://jsonplaceholder.typicode.com/todos";
        var client = new HttpClient();
        var json = client.GetStringAsync(url).Result;
        List<Todos> data = JsonConvert.DeserializeObject<List<Todos>>(json);
        List<Todos> filtereData = new List<Todos>();

        int userId = 1;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].userId == userId)
            {
                filtereData.Add(data[i]);
            }
            else
            {
                continue;
            }
        }

        Console.WriteLine("the provided userId = " + userId +". The userId of every item in the filtered list is:");
        for (int i = 0; i < filtereData.Count; i++)
        {
            Console.WriteLine(filtereData[i].userId);
        }
    }
}

