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

        IEnumerable<Todos> todosQuery = 
            from item in data
            where item.userId == userId
            select item;

        Console.WriteLine("the provided userId = " + userId +". The userId of every item in the filtered list is:");
        foreach (var item in todosQuery)
        {
            Console.WriteLine(item.userId);
        }
    }
}

