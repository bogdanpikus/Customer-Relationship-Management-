using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.ViewModels
{
    public class OrderModalViewModel : NotifyPropertyChange
    {
        private readonly DuckDatabase _db = DatabaseFactory.Instance;
        public ICommand Confirm {  get; }

        public DateTime Date { get; set; }
        public string? Articul {  get; set; }
        public string? OrderID { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? Phone {  get; set; }
        public string? ItemName { get; set; }
        public int AmountItem { get; set; }
        public decimal? Price { get; set; }
        public decimal? PrimeCost { get; set; }
        public string? PaymentWay { get; set; }
        public string? DeliverWay { get; set; }
        public string? DeliverAdress { get; set; }
        public string? Status { get; set; }
        public decimal? Spending { get; set; }
        public decimal? Income { get; set; }
        public string? Comment { get; set; }

        public OrderModalViewModel()
        {
            Confirm = new RelayCommand(Click => ConfirmOrder());
        }
     
        private void ConfirmOrder()
        {
            var customer = new Customer
            {
                SecondName = SecondName,
                Name = Name,
                Surname = Surname,
                Phone = Phone,
                AmountOrders = 0
            };
            _db.InsertIntoTable("customer", $"{customer.SecondName},{customer.Name},{customer.Surname},{customer.Phone},{customer.AmountOrders}");
            var orders = new Order
            {
                IsSelected = false,
                OrderDate = Date,
                Articul = Articul,
                OrderID = OrderID,
                CustomerID = customer.Id,
                Customer = customer,
                Item = ItemName,
                Amount = AmountItem,
                Price = Price,
                PrimeCost = PrimeCost,
                PaymentWay = PaymentWay,
                DelivarWay = DeliverWay,
                DeliverAdress = DeliverAdress,
                Status = Status,
                Spending = Spending,
                Income = Income,
                Comment = Comment
            };
            _db.InsertIntoTable("orders", $"{orders.IsSelected},{orders.OrderDate},{orders.Articul},{orders.OrderID}," +
                $"{orders.CustomerID},{orders.Customer},{orders.Item},{orders.Amount},{orders.Price},{orders.PrimeCost},{orders.PaymentWay}," +
                $"{orders.DelivarWay},{orders.DeliverAdress},{orders.Status},{orders.Spending},{orders.Income},{orders.Comment}");
        }
    }
}
