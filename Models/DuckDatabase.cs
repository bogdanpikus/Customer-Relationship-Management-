using System;
using System.Data;
using System.Windows;
using DuckDB.NET.Data;
using DuckDB.NET.Native;

namespace CRM.Models
{
    public class DuckDatabase
    {
       private readonly DuckDBConnection _connection;
       
        public DuckDatabase(bool InMemory = false, string? DatabasePlace = null, string? DatabaseName = null)
        {
            //здесь все проверки на базу данных, не в VM
            if(DatabaseName == "" || DatabasePlace == "")
            {
                MessageBox.Show("Заполните все поля");
            }
              string isInMemory = InMemory ? "Data Source=:memory:" : $"Data Source={DatabasePlace}/{DatabaseName}.duckdb";
              _connection = new DuckDBConnection(isInMemory);
              _connection.Open();
        }
    }
}
