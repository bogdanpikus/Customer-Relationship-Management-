using System;
using System.IO;
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

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE SEQUENCE IF NOT EXISTS seq_customers START 1";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS customers (Id INTEGER DEFAULT nextval('seq_customers') PRIMARY KEY, SecondName VARCHAR," +
                  " Name VARCHAR, Surname VARCHAR, Phone VARCHAR, AmountOrders INTEGER, CustomerSumIncome DECIMAL(18,2), " +
                  "CustomerPurchases VARCHAR, CustomerLastOrderDate DATE)";
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
                   "OrderID VARCHAR, SecondName VARCHAR, Name VARCHAR, Surname VARCHAR, Phone VARCHAR, Item VARCHAR, Amount TINYINT, Price DECIMAL(18,2)," +
                   " Pricecost DECIMAL(18,2), PaymentWay VARCHAR, DelivarWay VARCHAR, DeliverAdress VARCHAR," +
                   "Status VARCHAR, Spending DECIMAL(18,2), Income DECIMAL(18,2), Organization VARCHAR, Comment VARCHAR)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE SEQUENCE IF NOT EXISTS seq_company START 1";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS companies (Id INTEGER DEFAULT nextval('seq_company') PRIMARY KEY, CompanyName VARCHAR, INN INTEGER," +
                    "EDPNOU INTEGER, Details VARCHAR, AmountOrders INTEGER, Email VARCHAR, CompanySumIncome DECIMAL(18,2), CompanyPurchases VARCHAR, " +
                    "CompanyLastOrderDate DATE)";
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

                return true;
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
        public List<Customer> ExtractCustomers()
        {
            using (var cmd = _connection.CreateCommand())
            {
                var customerList = new List<Customer>();
                cmd.CommandText = @"SELECT SecondName, Name, Surname, Phone, AmountOrders, 
                                  CustomerSumIncome, CustomerPurchases, CustomerLastOrderDate FROM customers 
                                  ORDER BY LOWER(SecondName), LOWER(Name), LOWER(Surname), CustomerLastOrderDate DESC,
                                  CustomerSumIncome DESC";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customerList.Add(new Customer
                    {
                        SecondName = reader.IsDBNull(0) ? null : reader.GetString(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Surname = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                        AmountOrders = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        CustomerSumIncome = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                        CustomerPurchases = reader.IsDBNull(6) ? null : reader.GetString(6),
                        CustomerLastOrderDate = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7)
                    });
                }

                return customerList;
            }
        }
        public List<Company> ExtractCompanies()
        {
            using (var cmd = _connection.CreateCommand())
            {
                var companiesList = new List<Company>();
                cmd.CommandText = @"SELECT CompanyName, INN, EDPNOU, Details, AmountOrders, Email, CompanySumIncome,
                                   CompanyPurchases, CompanyLastOrderDate FROM companies ORDER BY LOWER(CompanyName), LOWER(CAST(INN AS VARCHAR)),
                                   LOWER(CAST(EDPNOU AS VARCHAR)),CompanyLastOrderDate DESC, CompanySumIncome DESC";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    companiesList.Add(new Company
                    {
                        CompanyName = reader.IsDBNull(0) ? null : reader.GetString(0),
                        INN = reader.IsDBNull(1) ? null : reader.GetInt16(1),
                        EDPNOU = reader.IsDBNull(2) ? null : reader.GetInt16(2),
                        Details = reader.IsDBNull(3) ? null : reader.GetString(3),
                        AmountOrders = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                        CompanySumIncome = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                        CompanyPurchases = reader.IsDBNull(7) ? null : reader.GetString(7),
                        CompanyLastOrderDate = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8)
                    });
                }

                return companiesList;
            }
        }
        public bool SQLCustomerInsert(Customer customer)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO customers (SecondName, Name, Surname, Phone, AmountOrders, 
                                  CustomerSumIncome, CustomerPurchases, CustomerLastOrderDate) VALUES (?,?,?,?,?,?,?,?)";
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.SecondName });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Name });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Surname });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.Phone });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.AmountOrders });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerSumIncome });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerPurchases });
                cmd.Parameters.Add(new DuckDBParameter { Value = customer.CustomerLastOrderDate });
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool SQLCompanyInsert(Company company)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO companies (CompanyName, INN, EDPNOU, Details, AmountOrders, Email, CompanySumIncome,
                                   CompanyPurchases, CompanyLastOrderDat) VALUES (?,?,?,?,?,?,?,?,?)";
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyName });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.INN });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.EDPNOU });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.Details });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.AmountOrders });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.Email });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanySumIncome });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyPurchases });
                cmd.Parameters.Add(new DuckDBParameter { Value = company.CompanyLastOrderDate});
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool CustomerSelectedDelete(int id)
        {
            using(var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM customers WHERE Id = ?";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new DuckDBParameter { Value = id });
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
        public bool CompanySelectedDelete(int id)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM companies WHERE Id = ?";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new DuckDBParameter { Value = id });
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
    }
}