using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Npgsql;
using System.Linq;
using AzureDBEntity.Models;

namespace AzureDBEntity
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            using (var db = new PostgresContext())
            {
                // Creating a new item and saving it to the database using Entity framework
                var newItem = new Table1();
                newItem.Id = 4;
                newItem.ProductId = 1;
                newItem.ErrorType = "authorization";
                db.Table1s.Add(newItem);

                // left join based on sting field called ErrorType
                var leftJoinString = db.Table1s.Join(db.Table2s,
                    dt1 => dt1.ErrorType,
                    dt2 => dt2.ErrorType,
                    (dt1, dt2) => new { ProductIdFromTable1 = dt1.ProductId, ProductIdFromTable2 = dt2.ProductId, ErrorType = dt1.ErrorType });

                // right join based on integer field called ProductId
                var rightJoinInteger = db.Table2s.Join(db.Table1s,
                    dt2 => dt2.ProductId,
                    dt1 => dt1.ProductId,
                    (dt2, dt1) => new { ErrorTypeFromTable2 = dt2.ErrorType, ErrorTypeFromTable1 = dt1.ErrorType, ProductId = dt2.ProductId });

                return new OkObjectResult(leftJoinString);
            }
        }
    }
}
