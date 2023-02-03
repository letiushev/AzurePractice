using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace AzureTestJson
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // get json data from webpage
            Todos todos = new Todos();
            string url = "https://jsonplaceholder.typicode.com/todos";
            var client = new HttpClient();
            var json = client.GetStringAsync(url).Result;
            List<Todos> data = JsonConvert.DeserializeObject<List<Todos>>(json);
            List<Todos> filtereData = new List<Todos>();

            // pass userId as a query parameter
            int id = int.Parse(req.Query["id"]);

            int userId = id;

            // LINQ sorting
            IEnumerable<Todos> todosQuery =
                from item in data
                where item.userId == userId
                select item;
            data.GroupBy(x => x.completed);
            var dataCompletedAndId = data.GroupBy(x => new { x.completed, x.id });

            return new OkObjectResult(todosQuery);
        }
    }
}
