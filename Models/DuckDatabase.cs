using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Security.Cryptography;
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
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS customers (SecondName VARCHAR," +
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
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS orders (Id INTEGER DEFAULT nextval('seq_orders') PRIMARY KEY, OrderDate DATE, Articul VARCHAR," +
                   "OrderID VARCHAR, SecondName VARCHAR, Name VARCHAR, Surname VARCHAR, Phone VARCHAR, Item VARCHAR, Amount TINYINT, Price DECIMAL(18,2), Pricecost DECIMAL(18,2), PaymentWay VARCHAR, DelivarWay VARCHAR, DeliverAdress VARCHAR," +
                   "Status VARCHAR, Spending DECIMAL(18,2), Income DECIMAL(18,2), Organization VARCHAR, Comment VARCHAR)";
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
                crm.CommandText = @"INSERT INTO orders (OrderDate, Articul, OrderID, SecondName, Name, Surname, Phone, Item, Amount, Price,
                    Pricecost, PaymentWay, DelivarWay, DeliverAdress, Status, Spending, Income, Organization, Comment) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?) RETURNING Id";

                crm.Parameters.Add(new DuckDBParameter { Value = order.OrderDate });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Articul });
                crm.Parameters.Add(new DuckDBParameter { Value = order.OrderID });
                crm.Parameters.Add(new DuckDBParameter { Value = order.SecondName });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Name });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Surname });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Phone });
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
                crm.Parameters.Add(new DuckDBParameter { Value = order.Organization });
                crm.Parameters.Add(new DuckDBParameter { Value = order.Comment });
                order.Id = Convert.ToInt32(crm.ExecuteScalar());
            }
        }

        public void ExtractOrdersFromDatabase(ObservableCollection<Order> Orders) //вытягивания заказов из базы
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT Id, OrderDate, Articul, OrderID, SecondName, Name, Surname, Phone, Item, Amount, Price,
                    Pricecost, PaymentWay, DelivarWay, DeliverAdress, Status, Spending, Income, Organization, Comment FROM orders";
                using var reader = cmd.ExecuteReader();

                /*
                 if (!reader.HasRows)
                 {
                    return;
                 } 
                */

                // TODO: как-то вписать имя/фам/отч/тел из таблиццы customer
                while (reader.Read())
                {
                    var order = new Order
                    {
                        Id = reader.GetFieldValue<int>(0),
                        OrderDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetFieldValue<DateTime>(1),
                        Articul = reader.IsDBNull(2) ? null : reader.GetFieldValue<string>(2),
                        OrderID = reader.IsDBNull(3) ? null : reader.GetFieldValue<string>(3),
                        SecondName = reader.IsDBNull(4) ? null : reader.GetFieldValue<string>(4),
                        Name = reader.IsDBNull(5) ? null : reader.GetFieldValue<string>(5),
                        Surname = reader.IsDBNull(6) ? null : reader.GetFieldValue<string>(6),
                        Phone = reader.IsDBNull(7) ? null : reader.GetFieldValue<string>(7),
                        Item = reader.IsDBNull(8) ? null : reader.GetFieldValue<string>(8),
                        Amount = reader.IsDBNull(9) ? 0 : reader.GetFieldValue<int>(9),
                        Price = reader.IsDBNull(10) ? 0 : reader.GetFieldValue<decimal>(10),
                        PrimeCost = reader.IsDBNull(11) ? 0 : reader.GetFieldValue<decimal>(11),
                        PaymentWay = reader.IsDBNull(12) ? null : reader.GetFieldValue<string>(12),
                        DelivarWay = reader.IsDBNull(13) ? null : reader.GetFieldValue<string>(13),
                        DeliverAdress = reader.IsDBNull(14) ? null : reader.GetFieldValue<string>(14),
                        Status = reader.IsDBNull(15) ? null : reader.GetFieldValue<string>(15),
                        Spending = reader.IsDBNull(16) ? 0 : reader.GetFieldValue<decimal>(16),
                        Income = reader.IsDBNull(17) ? 0 : reader.GetFieldValue<decimal>(17),
                        Organization = reader.IsDBNull(18) ? null : reader.GetFieldValue<string>(18),
                        Comment = reader.IsDBNull(19) ? null : reader.GetFieldValue<string>(19)
                    };

                    Orders.Insert(0,order);
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

        public void DeleteOrderInDatabase(ObservableCollection<Order> orders)
        {
            var selectedOrder = orders.Where(ordersItem => ordersItem.IsSelected).ToList();

            using (var cmd = _connection.CreateCommand())
            {
                foreach (var order in selectedOrder)
                {
                    cmd.CommandText = "DELETE FROM orders WHERE Id = ?"; //TODO: надо id, который уникальный PRIMARY KEY и не может поменятся
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new DuckDBParameter { Value = order.Id });
                    cmd.ExecuteNonQuery();

                    orders.Remove(order);
                }
            }
        }

        public bool UpdateOrder(Order order) //Возвращает true когда выполняется правильно и false, когда что-то идет не так
        {
            // TODO: загрузка изменений в базу
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"UPDATE orders SET OrderDate = ?, Articul = ?, OrderID = ?, SecondName = ?, Name = ?,
                                  Surname = ?, Phone = ?, Item = ?, Amount = ?, Price = ?,
                                  Pricecost = ?, PaymentWay = ?, DelivarWay = ?, DeliverAdress = ?, Status = ?,
                                  Spending = ?, Income = ?, Organization = ?, Comment = ?  WHERE Id = ?";

                cmd.Parameters.Add(new DuckDBParameter { Value = order.OrderDate });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Articul });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.OrderID });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.SecondName });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Name });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Surname });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Phone });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Item });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Amount });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Price });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.PrimeCost });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.PaymentWay });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.DelivarWay });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.DeliverAdress });
                cmd.Parameters.Add(new DuckDBParameter {Value = order.Status });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Spending });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Income });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Organization });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Comment });

                cmd.Parameters.Add(new DuckDBParameter { Value = order.Id });

                return cmd.ExecuteNonQuery()>0;
            }
        }
    }
}