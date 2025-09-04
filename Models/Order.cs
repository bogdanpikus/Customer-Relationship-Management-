using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    class Order
    {
        public bool isSelected;
        public int atricul;
        public string secondName = "undefined";
        public string name = "undefined";
        public string surname = "undefined";

        public void AddTable() //добавление в таблицу значений (еще не знаю как)
        {
            Console.WriteLine($"{atricul}:{secondName}:{name}:{surname}");
        } 
    }
}
