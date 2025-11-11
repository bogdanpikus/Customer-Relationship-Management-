using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Windows.Input;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace CRM.ViewModels
{
    public class OrderModalViewModel : NotifyPropertyChange
    {
        private readonly DialogService _dialogService = DialogService.Instance;
        private readonly SQLService _sqlService = new SQLService();

        public ICommand Confirm {  get; }
        public ICommand TestOrder { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
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
        public string? Organization {  get; set; }
        public string? Comment { get; set; }

        public OrderModalViewModel(ObservableCollection<Order> orders)
        {
            Confirm = new RelayCommand(Click => ConfirmOrder(orders));
            TestOrder = new RelayCommand(Click => AddTestOrderMackup(orders));
        }
     
        private void ConfirmOrder(ObservableCollection<Order> orders)
        {
            // TODO: где-то тут нужна проверка на типизацию и валидацию ввода данных

            int count = orders.Count;

            var customer = new Customer
            {
                SecondName = SecondName,
                Name = Name,
                Surname = Surname,
                Phone = Phone,
                AmountOrders = 1
            };
            _sqlService.InsertCustomer(customer);

            var order = new Order
            {
                PersonalNumber = count + 1,
                IsSelected = false,
                OrderDate = Date,
                Articul = Articul,
                OrderID = OrderID,
                Customer = customer,
                Name = customer.Name,
                SecondName = customer.SecondName,
                Surname = customer.Surname,
                Phone = customer.Phone,
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
                Organization = Organization,
                Comment = Comment
            };
            _sqlService.InsertOrder(order);
            orders.Insert(0,order);
            _dialogService.CloseDialog();
        }
        private void AddTestOrderMackup(ObservableCollection<Order> orders)
        {
            int count = orders.Count;
            var order = new Order
            {
                PersonalNumber = count + 1,
                IsSelected = false,
                OrderDate = DateAndTime.Now,
                Articul = "1001",
                OrderID = "5001",
                Name = "Bogdan",
                SecondName = "Pikus",
                Surname = "Vladimirovic",
                Phone = "0633215780",
                Item = "Яблоко",
                Amount = 1,
                Price = 100,
                PrimeCost = 0,
                PaymentWay = "PrivatBank",
                DelivarWay = "Nova Poshta",
                DeliverAdress = "Dnipro",
                Spending = 0,
                Income = 100,
                Organization = "Valley",
                Comment = "-"
            };

            _sqlService.InsertOrder(order);
            orders.Insert(0,order);
        }
    }
}
