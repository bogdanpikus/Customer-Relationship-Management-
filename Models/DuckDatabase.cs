using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Net;
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
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS customers (Id INTEGER DEFAULT nextval('seq_customers') PRIMARY KEY, SecondName VARCHAR," +
                  " Name VARCHAR, Surname VARCHAR, Phone VARCHAR, AmountOrders TINYINT)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE SEQUENCE IF NOT EXISTS seq_orders START 1";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS orders (Id INTEGER DEFAULT nextval('seq_orders') PRIMARY KEY, IsSelected BOOLEAN DEFAULT FALSE, OrderDate DATE, Articul VARCHAR," +
                   "OrderID VARCHAR, CustomerID INTEGER, Item VARCHAR, Amount TINYINT, Price FLOAT, Pricecost FLOAT, PaymentWay VARCHAR, DelivarWay VARCHAR, DeliverAdress VARCHAR," +
                   "Status VARCHAR, Spending FLOAT, Income FLOAT, Comment VARCHAR)";
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertCustomer(Customer customer)
        {
            using (var crm = _connection.CreateCommand())
            {
                crm.CommandText = @"INSERT INTO customers (SecondName, Name, Surname, Phone, AmountOrders) VALUES (?,?,?,?,?)";
                crm.Parameters.Add(new DuckDBParameter { Value = customer.SecondName });
                crm.Parameters.Add(new DuckDBParameter { Value = customer.Name });
                crm.Parameters.Add(new DuckDBParameter { Value = customer.Surname });
                crm.Parameters.Add(new DuckDBParameter { Value = customer.Phone });
                crm.Parameters.Add(new DuckDBParameter { Value = customer.AmountOrders });
                crm.ExecuteNonQuery();
            }

        }

        public void InsertOrder(Order order)
        {
            using (var crm = _connection.CreateCommand())
            {
                crm.CommandText = @"INSERT INTO orders (OrderDate, Articul, OrderID, CustomerID, Item, Amount, Price,
                    Pricecost, PaymentWay, DelivarWay, DeliverAdress, Status, Spending, Income, Comment) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                crm.Parameters.Add(new DuckDBParameter { Value = order.OrderDate });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Articul });
                crm.Parameters.Add(new DuckDBParameter { Value = order.OrderID });
                crm.Parameters.Add(new DuckDBParameter { Value = order.CustomerID });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Item });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Amount });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Price });
                crm.Parameters.Add(new DuckDBParameter { Value = order.PrimeCost });
                crm.Parameters.Add(new DuckDBParameter { Value = order.PaymentWay });
                crm.Parameters.Add(new DuckDBParameter { Value = order.DelivarWay });
                crm.Parameters.Add(new DuckDBParameter { Value = order.DeliverAdress });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Status });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Spending });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Income });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Comment });
                crm.ExecuteNonQuery();
            }
        }

        public void ExtractOrdersFromDatabase(ObservableCollection<Order> Orders) //вытягивания заказов из базы
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT OrderDate, Articul, OrderID, Item, Amount, Price,
                    Pricecost, PaymentWay, DelivarWay, DeliverAdress, Status, Spending, Income, Comment FROM orders";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var order = new Order
                    {
                        OrderDate = reader.GetFieldValue<DateTime>(0),
                        Articul = reader.GetFieldValue<string>(1),
                        OrderID = reader.GetFieldValue<string>(2),
                        Item = reader.GetFieldValue<string>(3),
                        Amount = reader.GetFieldValue<int>(4),
                        Price = reader.GetFieldValue<decimal>(5),
                        PrimeCost = reader.GetFieldValue<decimal>(6),
                        PaymentWay = reader.GetFieldValue<string>(7),
                        DelivarWay = reader.GetFieldValue<string>(8),
                        DeliverAdress = reader.GetFieldValue<string>(9),
                        Status = reader.GetFieldValue<string>(10),
                        Spending = reader.GetFieldValue<decimal>(11),
                        Income = reader.GetFieldValue<decimal>(12),
                        Organization = reader.GetFieldValue<string>(13),
                        Comment = reader.GetFieldValue<string>(14)
                    };

                    Orders.Add(order);
                }
            }
        }

        public void ExtractCustomersFromDatabase() //вытягивание заказчиков из базы
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM customers";
                cmd.ExecuteNonQuery();
            }
        }
    }
}