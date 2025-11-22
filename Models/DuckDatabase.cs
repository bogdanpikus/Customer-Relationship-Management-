using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Controls;
using CRM.ViewModels;
using DuckDB.NET.Data;

namespace CRM.Models
{
    public class DuckDatabase
    {
        private readonly DuckDBConnection _connection;

        public DuckDatabase(bool InMemory = false, bool OpenFile = false, string? DatabasePlace = null, string? DatabaseName = null)
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

            using(var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE SEQUENCE IF NOT EXISTS seq_customers START 1";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS customers (Id INTEGER DEFAULT nextval('seq_customers') PRIMARY KEY, SecondName VARCHAR," +
                                  "Name VARCHAR, Surname VARCHAR, Phone VARCHAR, AmountOrders INTEGER, CustomerSumIncome DECIMAL(18,2), " +
                                  "CustomerPurchases VARCHAR, CustomerLastOrderDate DATE)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE SEQUENCE IF NOT EXISTS seq_orders START 1";
                cmd.ExecuteNonQuery();
            }


            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS orders (Id INTEGER DEFAULT nextval('seq_orders') PRIMARY KEY, OrderDate DATE, Articul VARCHAR," +
                                  "OrderID VARCHAR, SecondName VARCHAR, Name VARCHAR, Surname VARCHAR, Phone VARCHAR, Item VARCHAR, Amount TINYINT, Price DECIMAL(18,2)," +
                                  "Pricecost DECIMAL(18,2), PaymentWay VARCHAR, DelivarWay VARCHAR, DeliverAdress VARCHAR," +
                                  "Status VARCHAR, Spending DECIMAL(18,2), Income DECIMAL(18,2), Organization VARCHAR, Comment VARCHAR)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE SEQUENCE IF NOT EXISTS seq_companies START 1";
                cmd.ExecuteNonQuery();
            }


            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS companies (Id INTEGER DEFAULT nextval('seq_companies') PRIMARY KEY, CompanyName VARCHAR, INN INTEGER," +
                                  "EDPNOU VARCHAR, Details VARCHAR, AmountOrders INTEGER, Email VARCHAR, Bank VARCHAR, CompanySumIncome DECIMAL(18,2), CompanyPurchases VARCHAR, " +
                                  "CompanyLastOrderDate DATE)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE SEQUENCE IF NOT EXISTS seq_storage START 1";
                cmd.ExecuteNonQuery();
            }


            using (var cmd = _connection.CreateCommand()) // NOTE: СОЗДАНИЕ ТАБЛИЦЫ STORAGES
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS storage (Id INTEGER DEFAULT nextval('seq_storage') PRIMARY KEY, StorageName VARCHAR, Address VARCHAR, 
                                    Responsible VARCHAR, Phone VARCHAR, AmountGoodsInStorage INTEGER)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE SEQUENCE IF NOT EXISTS seq_groups START 1";
                cmd.ExecuteNonQuery();
            }


            using (var cmd = _connection.CreateCommand()) // NOTE: СОЗДАНИЕ ТАБЛИЦЫ GROUPS
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS productGroup (Id INTEGER DEFAULT nextval('seq_groups') PRIMARY KEY, StorageId INTEGER NOT NULL, Name VARCHAR,
                                    FOREIGN KEY (StorageId) REFERENCES storage(Id))"; 
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE SEQUENCE IF NOT EXISTS seq_products START 1";
                cmd.ExecuteNonQuery();
            }


            using (var cmd = _connection.CreateCommand()) // NOTE: СОЗДАНИЕ ТАБЛИЦЫ PRODUCTS
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS products (Id INTEGER DEFAULT nextval('seq_products') PRIMARY KEY, ProductId INTEGER NOT NULL,
                                    Articul VARCHAR, PhotoPath VARCHAR, Name VARCHAR, Price DECIMAL(18,2), PrimaryPrice DECIMAL(18,2), 
                                    IncomeFromSelling DECIMAL(18,2), Amount INTEGER, Comment VARCHAR,
                                    FOREIGN KEY (ProductId) REFERENCES productGroup(Id))";
                cmd.ExecuteNonQuery();
            }
        }

        public bool InsertCustomer(Customer customer)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO customers (SecondName, Name, Surname, Phone, AmountOrders) VALUES (?,?,?,?,?)";
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.SecondName });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Name });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Surname });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Phone });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.AmountOrders });

                return cmd.ExecuteNonQuery() > 0;
            }

        }

        public bool InsertOrder(Order order)
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

                return order.Id > 0;
            }
        }

        public List<Order> ExtractOrdersFromDatabase()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT Id, OrderDate, Articul, OrderID, SecondName, Name, Surname, Phone, Item, Amount, Price,
                    Pricecost, PaymentWay, DelivarWay, DeliverAdress, Status, Spending, Income, Organization, Comment FROM orders";
                using var reader = cmd.ExecuteReader();

                var orders = new List<Order>();
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

                    orders.Add(order);
                }
                return orders;
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

        public bool DeleteOrderInDatabase(int id)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM orders WHERE Id = ?";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new DuckDBParameter { Value = id });
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public bool UpdateOrder(Order order) //Возвращает true когда выполняется правильно и false, когда что-то идет не так
        {
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
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Status });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Spending });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Income });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Organization });
                cmd.Parameters.Add(new DuckDBParameter { Value = order.Comment });

                cmd.Parameters.Add(new DuckDBParameter { Value = order.Id });

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public List<RangeWithOrders> SelectDataToWeekGraff(DateTime start, DateTime end)
        {
            using (var cmd = _connection.CreateCommand())
            {
                var rangeList = new List<RangeWithOrders>();
                cmd.CommandText = @"SELECT OrderDate, COUNT(*) AS TotalOrdersForDay FROM orders WHERE OrderDate BETWEEN ? AND ? GROUP BY OrderDate ORDER BY OrderDate";
                cmd.Parameters.Add(new DuckDBParameter { Value = start });
                cmd.Parameters.Add(new DuckDBParameter { Value = end });
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ordersPerDayItem = new RangeWithOrders
                    {
                        Date = reader.GetDateTime(0),
                        Count = reader.GetInt32(1)
                    };

                    rangeList.Add(ordersPerDayItem);
                }
                return rangeList;
            }
        }
        private string GetMonthName(string monthName)
        {
            return monthName switch
            {
                "01" => "Январь",
                "02" => "Февраль",
                "03" => "Март",
                "04" => "Апрель",
                "05" => "Май",
                "06" => "Июнь",
                "07" => "Июль",
                "08" => "Август",
                "09" => "Сентябрь",
                "10" => "Октябрь",
                "11" => "Ноябрь",
                "12" => "Декабрь",
                _ => "НЕИЗВЕСТНО"
            };
        }
        public List<PriceByMonth> SelectAllPriceByMonth() // месяц | сумма оборота
        {
            using (var cmd = _connection.CreateCommand())
            {
                var priceList = new List<PriceByMonth>();
                cmd.CommandText = @"SELECT strftime('%m', OrderDate) AS Month,
                                    SUM(Price) AS Total From orders WHERE OrderDate IS NOT NULL AND
                                    EXTRACT(YEAR FROM OrderDate) = EXTRACT(YEAR FROM CURRENT_DATE)
                                    GROUP BY strftime('%m', OrderDate) ORDER BY Month";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var month = reader.GetString(0);
                    var total = reader.IsDBNull(1) ? 0m : reader.GetDecimal(1);

                    priceList.Add(new PriceByMonth
                    {
                        Month = GetMonthName(month),
                        SumOfPriceByMonth = (double)total
                    });
                }

                return priceList;
            }
        }
        public List<SourseCount> LoadSourseCountToDataGtid(int month, int year)
        {
            using (var cmd = _connection.CreateCommand())
            {
                var sourceList = new List<SourseCount>();
                cmd.CommandText = @"SELECT Organization, COUNT(*) AS CountTotal FROM orders WHERE strftime('%m', OrderDate) = ? 
                                   AND strftime('%Y', OrderDate) = ? AND Organization IS NOT NULL
                                   GROUP BY Organization ORDER BY Organization";
                cmd.Parameters.Add(new DuckDBParameter { Value = month });
                cmd.Parameters.Add(new DuckDBParameter { Value = year });
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var sourseModel = new SourseCount
                    {
                        Sourse = reader.IsDBNull(0) ? null : reader.GetString(0),
                        Count = reader.GetInt32(1)
                    };

                    sourceList.Add(sourseModel);
                }

                return sourceList;
            }
        }
        public List<TodayAnalizeData> LoadTodayOrdersIncomeData()
        {
            using (var cmd = _connection.CreateCommand())
            {
                var todayDataList = new List<TodayAnalizeData>();
                cmd.CommandText = @"SELECT COUNT(*) AS TotalOrders, COALESCE(SUM(Income), 0) AS TotalIncome FROM orders WHERE DATE(OrderDate) = CURRENT_DATE";
                //(посчитать все даты за сегодня) 2 заказа за сегодня : 300 прибыли за сегодня
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var todayList = new TodayAnalizeData
                    {
                        TodayOrdersCount = reader.GetInt32(0), // ERROR: ПЫТАЕТСЯ ЗАПИСАТЬ DateTime в Int(32)
                        TodayIncome = reader.GetDecimal(1)
                    };

                    todayDataList.Add(todayList);
                }

                return todayDataList;
            }
        }
        public List<MonthAnalizeData> LoadMonthOrdersData(int month, int year)
        {
            using (var cmd = _connection.CreateCommand())
            {
                var monthDataList = new List<MonthAnalizeData>();
                cmd.CommandText = @"SELECT COUNT(OrderDate) AS MonthOrders, SUM(Status = 'Отмененный') AS TotalRejections,
                                    COALESCE(SUM(Income), 0) AS MonthIncome, SUM(Status = 'Успешный') AS TotalSuccess,
                                    SUM(Status = 'Обрабатывается') AS TotalInProcess, SUM(Status = 'Доставляется') AS TotalDeliver FROM orders 
                                    WHERE strftime('%m', OrderDate) = ? AND strftime('%Y', OrderDate) = ?";
                cmd.Parameters.Add(new DuckDBParameter { Value = month });
                cmd.Parameters.Add(new DuckDBParameter { Value = year });
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var monthList = new MonthAnalizeData
                    {
                        MonthOrderCount = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        MonthOrderRejections = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                        MonthIncome = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        MonthSuccessCount = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        MonthInProcessCount = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        MonthDeliverCount = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                    };

                    monthDataList.Add(monthList);
                }
                return monthDataList;
            }
        }
        private string GetMonthNameForYearIncome(string monthName)
        {
            return monthName switch
            {
                "01" => "Янв",
                "02" => "Фев",
                "03" => "Мар",
                "04" => "Апр",
                "05" => "Май",
                "06" => "Июн",
                "07" => "Июл",
                "08" => "Авг",
                "09" => "Сен",
                "10" => "Окт",
                "11" => "Нояб",
                "12" => "Дек",
                _ => "НЕИЗВЕСТНО"
            };
        }

        public List<YearIncome> ExtractYearIncome()
        {
            using (var cmd = _connection.CreateCommand())
            {
                var yearList = new List<YearIncome>();
                cmd.CommandText = @"SELECT strftime('%m', OrderDate) AS Month,
                                    SUM(Income) AS TotalIncome From orders WHERE OrderDate IS NOT NULL AND
                                    EXTRACT(YEAR FROM OrderDate) = EXTRACT(YEAR FROM CURRENT_DATE)
                                    GROUP BY strftime('%m', OrderDate) ORDER BY Month";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var month = reader.GetString(0);
                    var income = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);

                    yearList.Add(new YearIncome
                    {
                        Month = GetMonthNameForYearIncome(month),
                        MonthIncome = income
                    });
                }
                return yearList;
            }
        }

        private string GetMonthNameForYearSpendings(string monthName)
        {
            return monthName switch
            {
                "01" => "Янв",
                "02" => "Фев",
                "03" => "Мар",
                "04" => "Апр",
                "05" => "Май",
                "06" => "Июн",
                "07" => "Июл",
                "08" => "Авг",
                "09" => "Сен",
                "10" => "Окт",
                "11" => "Нояб",
                "12" => "Дек",
                _ => "НЕИЗВЕСТНО"
            };
        }
        public List<YearSpendings> ExtractYearSpendings()
        {
            using (var cmd = _connection.CreateCommand())
            {
                var yearList = new List<YearSpendings>();
                cmd.CommandText = @"SELECT strftime('%m', OrderDate) AS Month,
                                    SUM(Spending) AS TotalSpendings From orders WHERE OrderDate IS NOT NULL AND
                                    EXTRACT(YEAR FROM OrderDate) = EXTRACT(YEAR FROM CURRENT_DATE)
                                    GROUP BY strftime('%m', OrderDate) ORDER BY Month";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var month = reader.GetString(0);
                    var spendings = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);

                    yearList.Add(new YearSpendings
                    {
                        Month = GetMonthNameForYearSpendings(month),
                        MonthSpendings = spendings
                    });
                }

                return yearList;
            }
        }

        //WARNING: с этого места возможны ошибки. BUG: есть баги
        public List<Customer> ExtractCustomers() // РАБОТАЕТ
        {
            using (var cmd = _connection.CreateCommand())
            {
                var customerList = new List<Customer>();
                cmd.CommandText = @"SELECT Id, SecondName, Name, Surname, Phone, AmountOrders, 
                                  CustomerSumIncome, CustomerPurchases, CustomerLastOrderDate FROM customers 
                                  ORDER BY LOWER(SecondName), LOWER(Name), LOWER(Surname), CustomerLastOrderDate DESC,
                                  CustomerSumIncome DESC";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customerList.Add(new Customer
                    {
                        Id = reader.GetInt32(0),
                        SecondName = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Surname = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                        AmountOrders = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        CustomerSumIncome = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                        CustomerPurchases = reader.IsDBNull(7) ? null : reader.GetString(7),
                        CustomerLastOrderDate = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8)
                    });
                }

                return customerList;
            }
        }
        public List<Company> ExtractCompanies() // РАБОТАЕТ
        {
            using (var cmd = _connection.CreateCommand())
            {
                var companiesList = new List<Company>();
                cmd.CommandText = @"SELECT Id, CompanyName, INN, EDPNOU, Details, AmountOrders, Email, CompanySumIncome,
                                   CompanyPurchases, CompanyLastOrderDate FROM companies ORDER BY LOWER(CompanyName), LOWER(CAST(INN AS VARCHAR)),
                                   LOWER(CAST(EDPNOU AS VARCHAR)),CompanyLastOrderDate DESC, CompanySumIncome DESC";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    companiesList.Add(new Company
                    {
                        Id = reader.GetInt32(0),
                        CompanyName = reader.IsDBNull(1) ? null : reader.GetString(1),
                        INN = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                        EDPNOU = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Details = reader.IsDBNull(4) ? null : reader.GetString(4),
                        AmountOrders = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        Email = reader.IsDBNull(6) ? null : reader.GetString(6),
                        CompanySumIncome = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                        CompanyPurchases = reader.IsDBNull(8) ? null : reader.GetString(8),
                        CompanyLastOrderDate = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9)
                    });
                }

                return companiesList;
            }
        }
        public bool SQLCustomerInsert(Customer customer) // РАБОТАЕТ
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO customers (SecondName, Name, Surname, Phone, AmountOrders, 
                                  CustomerSumIncome, CustomerPurchases, CustomerLastOrderDate) VALUES (?,?,?,?,?,?,?,?) RETURNING Id";
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.SecondName });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Name });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Surname });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Phone });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.AmountOrders });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerSumIncome });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerPurchases });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerLastOrderDate });
                customer.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return customer.Id > 0;
            }
        }
        public bool SQLCompanyInsert(Company company) // РАБОТАЕТ
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO companies (CompanyName, INN, EDPNOU, Details, AmountOrders, Email, CompanySumIncome,
                                   CompanyPurchases, CompanyLastOrderDate) VALUES (?,?,?,?,?,?,?,?,?) RETURNING Id";
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyName });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.INN });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.EDPNOU });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.Details });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.AmountOrders });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.Email });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanySumIncome });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyPurchases });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyLastOrderDate});
                company.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return company.Id > 0;
            }
        }
        public bool CustomerSelectedDelete(int id) // РАБОТАЕТ
        {
            using(var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM customers WHERE Id = ?";
                cmd.Parameters.Add(new DuckDBParameter { Value = id });
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool CompanySelectedDelete(int id)
        {
            using (var cmd = _connection.CreateCommand()) // РАБОТАЕТ
            {
                cmd.CommandText = @"DELETE FROM companies WHERE Id = ?";
                cmd.Parameters.Add(new DuckDBParameter { Value = id });
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool UpdateCompanySelectedField(Company company) // РАБОТАЕТ
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"UPDATE companies SET CompanyName = ?, INN = ?, EDPNOU = ?, Details = ?, AmountOrders = ?, Email = ?,
                                    Bank = ?, CompanySumIncome = ?, CompanyPurchases = ?, CompanyLastOrderDate = ? WHERE Id = ?";
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyName });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.INN });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.EDPNOU });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.Details });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.AmountOrders });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.Email });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.Bank });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanySumIncome });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyPurchases });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyLastOrderDate });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.Id });
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool UpdateCustomerSelectedField(Customer customer) // РАБОТАЕТ
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"UPDATE customers SET SecondName = ?, Name = ?, Surname = ?, Phone = ?, AmountOrders = ?,
                                    CustomerSumIncome = ?, CustomerPurchases = ?, CustomerLastOrderDate = ?  WHERE Id = ?";
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.SecondName });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Name });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Surname });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Phone });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.AmountOrders });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerSumIncome });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerPurchases });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerLastOrderDate });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Id });
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool InsertStocrage(Storages storage)
        {
            using(var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO storage (StorageName, Address, Responsible, 
                                    Phone, AmountGoodsInStorage) VALUES (?,?,?,?,?) RETURNING Id";
                cmd.Parameters.Add(new DuckDBParameter { Value = storage.StorageName });
                cmd.Parameters.Add(new DuckDBParameter { Value = storage.Address });
                cmd.Parameters.Add(new DuckDBParameter { Value = storage.Responsible });
                cmd.Parameters.Add(new DuckDBParameter { Value = storage.Phone });
                cmd.Parameters.Add(new DuckDBParameter { Value = storage.AmountGoodsInStorage });
                storage.Id = Convert.ToInt32(cmd.ExecuteScalar());

                return storage.Id > 0;
            }
        }
        public bool DeleteStorage(int id) // storages (exist) -> productGroup (if exists) -> provucts (if exists) НЕ РАБОТАЕТ
        {
            using(var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM products WHERE ProductId IN (SELECT Id FROM productGroup WHERE StorageId = ?)";
                cmd.Parameters.Add(new DuckDBParameter { Value =  id });
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = @"DELETE FROM productGroup WHERE StorageId = ?";
                cmd.Parameters.Add(new DuckDBParameter { Value = id });
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = @"DELETE FROM storage WHERE Id = ?";  // IF NOT NULL 
                cmd.Parameters.Add(new DuckDBParameter { Value = id });

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public List<Storages> LoadStorages() // РАБОТАЕТ
        {
            using (var cmd = _connection.CreateCommand())
            {
                var storages = new List<Storages>();
                cmd.CommandText = @"SELECT StorageName ,Address ,Responsible ,Phone, AmountGoodsInStorage FROM storage";
                var reader  = cmd.ExecuteReader();
                while (reader.Read())
                {
                    storages.Add(new Storages
                    {
                        StorageName = reader.IsDBNull(0) ? null : reader.GetString(0),
                        Address = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Responsible = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                        AmountGoodsInStorage = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                    });
                }

                return storages;
            }
        }
    }
}