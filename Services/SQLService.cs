using CRM.Models;
using System;

namespace CRM.Services
{
    public class SQLService
    {
        private readonly DuckDatabase _db = DatabaseFactory.Instance;

        public List<Order> LoadOrdersFromDatabase()
        {
            return _db.ExtractOrdersFromDatabase();
        }
        public bool DeleteOrderInDatabase(int id)
        {
            return _db.DeleteOrderInDatabase(id);
        }
        public bool InsertOrder(Order order)
        {
            return _db.InsertOrder(order);
        }
        public bool InsertCustomer(Customer customer)
        {
            return _db.InsertCustomer(customer);
        }
        public bool UpdateOrder(Order order)
        {
            return _db.UpdateOrder(order);
        }
        public List<SourseCount> LoadSourseCountToDataGtid(int month, int year)
        {
            return _db.LoadSourseCountToDataGtid(month, year);
        }
        public List<RangeWithOrders> SelectDataToWeekGraff(DateTime start, DateTime end)
        {
            return _db.SelectDataToWeekGraff(start, end);
        }
        public List<PriceByMonth> SelectAllPriceByMonth()
        {
            return _db.SelectAllPriceByMonth();
        }
        public List<TodayAnalizeData> LoadTodayOrdersData()
        {
            return _db.LoadTodayOrdersIncomeData();
        }
        public List<MonthAnalizeData> LoadMonthOrdersData(int month, int year)
        {
            return _db.LoadMonthOrdersData(month, year);
        }
        public List<YearIncome> ExtractYearIncome()
        {
            return _db.ExtractYearIncome();
        }
        public List<YearSpendings> ExtractYearSpendings()
        {
            return _db.ExtractYearSpendings();
        }
        public List<Customer> ExtractCustomers() //вытягивания клиентов из базы данных
        {
            return _db.ExtractCustomers();
        }
        public List<Company> ExtractCompanies() //вытягивания компаний из базы данных
        {
            return _db.ExtractCompanies();
        }
        public bool SQLCustomerInsert(Customer customer) // запрос INSERT в таблицу customers
        {
            return _db.SQLCustomerInsert(customer);
        }
        public bool SQLCompanyInsert(Company company) // запрос INSERT вв таблицу companies
        {
            return _db.SQLCompanyInsert(company);
        }
        public bool CustomerSelectedDelete(int id)
        {
            return _db.CustomerSelectedDelete(id);
        }
        public bool CompanySelectedDelete(int id)
        {
            return _db.CompanySelectedDelete(id);
        }
        public bool UpdateCompanySelectedField(Company company)
        {
            return _db.UpdateCompanySelectedField(company);
        }
        public bool UpdateCustomerSelectedField(Customer customer)
        {
            return _db.UpdateCustomerSelectedField(customer);
        }
        public bool InsertStorage(Storages storage)
        {
            return _db.InsertStocrage(storage);
        }
        public bool DeleteStorage(int id)
        {
            return _db.DeleteStorage(id);
        }
        public List<Storages> LoadStorages()
        {
            return _db.LoadStorages();
        }
        public bool InsertGroup(ProductGroups group)
        {
            return _db.InsertGroup(group);
        }
        public List<ProductGroups> LoadGroups(int id)
        {
            return _db.LoadGroups(id);
        }
    }
}
