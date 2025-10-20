using CRM.Models;
using System;
using System.Collections.ObjectModel;
namespace CRM.Services
{
    public class SQLService // TODO: вынести всё взаимодействие с базый данных в этот сервис, а вызывать в VM колько его методы (по принципу SRP) 
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
        public List<SourseCount> LoadSourseCountToDataGtid(int month)
        {
            return _db.LoadSourseCountToDataGtid(month);
        }
        public List<RangeWithOrders> SelectDataToWeekGraff(DateTime start, DateTime end)
        {
            return _db.SelectDataToWeekGraff(start, end);
        }
        public List<PriceByMonth> SelectAllPriceByMonth()
        {
            return _db.SelectAllPriceByMonth();
        }
    }
}
