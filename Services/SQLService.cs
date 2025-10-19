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
    }
}
