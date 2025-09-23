using System;
using System.Data;
using System.IO;
using System.Windows;
using DuckDB.NET.Data;
using DuckDB.NET.Native;

namespace CRM.Models
{
    public class DuckDatabase
    {
        private readonly DuckDBConnection _connection;
    
        public DuckDatabase(bool InMemory = false, bool OpenFile = false,  string? DatabasePlace = null, string? DatabaseName = null)
        {
             if (string.IsNullOrWhiteSpace(DatabaseName)) { throw new ArgumentNullException(nameof(DatabaseName)); }
             if (string.IsNullOrWhiteSpace(DatabasePlace)) { throw new ArgumentNullException(nameof(DatabasePlace)); }
             string dbFilePath = InMemory ? ":memory:" : Path.Combine(DatabasePlace, $"{DatabaseName}.duckdb");
             string dbFileOpen = InMemory ? ":memory:" : $"{DatabasePlace}";

             if (!InMemory && File.Exists(dbFilePath)) { throw new InvalidOperationException("Файл уже существует в этой директории"); }
             if (!InMemory && File.Exists(dbFileOpen) && OpenFile) { dbFilePath = $"{DatabasePlace}"; }

             string dbConnection = InMemory ? "Data Source=:memory:" : $"Data Source={dbFilePath}";
             _connection = new DuckDBConnection(dbConnection);
             _connection.Open();

            using var command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS customer (Id INTEGER)";
            command.ExecuteNonQuery();
            command.CommandText = "CREATE TABLE IF NOT EXISTS orders (Id INTEGER)";
            command.ExecuteNonQuery();
        }

        public void InsertIntoTable(string table,string data)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"INSERT INTO {table} VALUES ({data})";
        }
    }
}