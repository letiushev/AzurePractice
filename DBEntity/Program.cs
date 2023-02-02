using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DBEntity.Models;

namespace MyConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new PostgresContext())
            {
                // Creating a new item and saving it to the database using Entity framework
                var newItem = new Table1();
                newItem.Id = 4;
                newItem.ProductId = 1;
                newItem.ErrorType = "authorization";
                db.Table1s.Add(newItem);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                // Retrieving and displaying data
                Console.WriteLine();
                Console.WriteLine("All items in the database :");
                foreach (var item in db.Table1s)
                {
                    Console.WriteLine("{0} | {1} | {2}", item.Id, item.ProductId, item.ErrorType);
                }
                Console.WriteLine();

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
                
                // console output of left/right joinin
                foreach (var item in leftJoinString)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();
                foreach (var item in rightJoinInteger)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
