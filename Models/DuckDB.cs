using System;
using System.Data;
using DuckDB.NET.Data;
using DuckDB.NET.Native;

namespace CRM.Models
{
    public class DuckDB
    {
       private readonly DuckDBConnection _connection;
       
        public DuckDB(bool InMemory = false, string? DatabasePlace = null)
        {
            string isInMemory = InMemory ? "Data Sourse=:memory:" : $"Data Sourse={DatabasePlace}/mydatabase.duckdb";
            _connection = new DuckDBConnection(isInMemory);
            _connection.Open();
        }
    }
}
