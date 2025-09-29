using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class EditingViewModal
    {
        private readonly Order _order;
        private readonly DuckDatabase _duck = DatabaseFactory.Instance;
        public ICommand Confirm { get; }

        public DateTime? Date { get; set; }
        public string? Articul { get; set; }
        public string? OrderID { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? Phone { get; set; }
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

        public EditingViewModal(Order order)
        {
            _order = order;

            Date = order.OrderDate;
            Articul = order.Articul;
            OrderID = order.OrderID;
            Surname = order.Surname;
            Name = order.Name;
            SecondName = order.SecondName;
            Phone = order.Phone;
            ItemName = order.Item;
            AmountItem = order.Amount;
            Price = order.Price;
            PrimeCost = order.PrimeCost;
            PaymentWay = order.PaymentWay;
            DeliverWay = order.DelivarWay;
            DeliverAdress = order.DeliverAdress;
            Status = order.Status;
            Spending = order.Spending;
            Income = order.Income;
            Comment = order.Comment;

            Confirm = new RelayCommand(Click => ConfirmEdit());
        }

        private void ConfirmEdit()
        {
            _order.OrderDate = Date.Value;
            _order.Articul = Articul;
            _order.OrderID = OrderID;
            _order.Surname = Surname;
            _order.Name = Name;
            _order.SecondName = SecondName;
            _order.Phone = Phone;
            _order.Item = ItemName;
            _order.Amount = AmountItem;
            _order.Price = Price;
            _order.PrimeCost = PrimeCost;
            _order.PaymentWay = PaymentWay;
            _order.DelivarWay = DeliverWay;
            _order.DeliverAdress = DeliverAdress;
            _order.Status = Status;
            _order.Spending = Spending;
            _order.Income = Income;
            _order.Comment = Comment;

            if (_duck.UpdateOrder(_order)) // true = успешно
            {
                DialogService.Instance.CloseDialog();
            }
            else
            {
                MessageBox.Show("Error. Try again.");
            }
        }
    }
}
