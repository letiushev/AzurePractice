﻿using GetDBSort;
using Npgsql;
using System.Data;

string connString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345;";

using (var conn = new NpgsqlConnection(connString))
{
    conn.Open();

    // define global variables where we will store data from tables
    List<ErrorTrack> dataTable1 = new List<ErrorTrack>();
    List<ErrorTrack> dataTable2 = new List<ErrorTrack>();

    // get data from table1
    using (var cmd = new NpgsqlCommand("SELECT * FROM table1", conn))
    {
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            dataTable1.Add(new ErrorTrack
            {
                Id = reader.GetInt32(0),
                ProductId = reader.GetInt32(1),
                ErrorType= reader.GetString(2),
            });
        }
        reader.Close();
    }

    // get data from table2
    using (var cmd = new NpgsqlCommand("SELECT * FROM table2", conn))
    {
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            dataTable2.Add(new ErrorTrack
            {
                Id = reader.GetInt32(0),
                ProductId = reader.GetInt32(1),
                ErrorType = reader.GetString(2),
            });
        }
        reader.Close();
    }

    // left join based on sting field called ErrorType
    var leftJoinString = dataTable1.Join(dataTable2,
        dt1 => dt1.ErrorType,
        dt2 => dt2.ErrorType,
        (dt1, dt2) => new { ProductIdFromTable1 = dt1.ProductId, ProductIdFromTable2 = dt2.ProductId, ErrorType = dt1.ErrorType });

    // right join based on integer field called ProductId
    var rightJoinInteger = dataTable2.Join(dataTable1,
        dt2 => dt2.ProductId,
        dt1 => dt1.ProductId,
        (dt2, dt1) => new { ErrorTypeFromTable2 = dt2.ErrorType, ErrorTypeFromTable1 = dt1.ErrorType, ProductId = dt2.ProductId });

    // console output
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