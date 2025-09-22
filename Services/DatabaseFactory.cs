using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
    public static class DatabaseFactory
    {
        private static DuckDatabase? _instance;

        public static DuckDatabase Instance => _instance ?? throw new InvalidOperationException("База ещё не создана.");

        public static void CreateDatabase(bool inMemory, bool openFile, string? folderPath, string? databaseName)
        {
            if (_instance == null)
                _instance = new DuckDatabase(inMemory, openFile, folderPath, databaseName);
        }
    }
}
