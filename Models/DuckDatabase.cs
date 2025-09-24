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

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE SEQUENCE IF NOT EXISTS seq_customers START 1";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS customer (Id INTEGER DEFAULT nextval('seq_customers') PRIMARY KEY, SecondName VARCHAR," +
                  " Name VARCHAR, Surname VARCHAR, Phone TINYINT, AmountOrders TINYINT)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE SEQUENCE IF NOT EXISTS seq_orders START 1";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS order (Id INTEGER DEFAULT nextval('seq_orders') PRIMARY KEY, IsSelected BOOLEAN, OrderDate DATE, Articul VARCHAR," +
                   "OrderID VARCHAR, Item VARCHAR, Amount TINYINT, Price FLOAT, Pricecost FLOAT, PaymentWay VARCHAR, DelivarWay VARCHAR, DeliverAdress VARCHAR," +
                   "Status VARCHAR, Spending FLOAT, Income FLOAT, Comment VARCHAR)";
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertCustomer(Customer customer)
        {
            using (var crm = _connection.CreateCommand())
            {
                crm.CommandText = @"INSERT INTO customer () VALUES ()";
            }

        }

        public void InsertOrder(Order order)
        {
            using (var crm = _connection.CreateCommand())
            {
                crm.CommandText = @"INSERT INTO order () VALUES ()";
            }
        }
    }
}