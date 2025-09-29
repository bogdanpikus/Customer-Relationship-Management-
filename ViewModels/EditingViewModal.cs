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

        public EditingViewModal(ObservableCollection<Order> orders,DateTime? orderDate, string? orderArticul, string? orderOrderID, string? orderSecondName,
            string? orderName, string? orderSurname, string? orderPhone, string? orderItem, int orderAmount, decimal? orderPrimecost, decimal? orderPrice, string? orderDeliverWay,
            string? orderDeliverAdress, string? orderPaymentWay, string? orderStatus, decimal? orderIncome, decimal? orderSpending, string? orderOrganization, string? orderComment)
        {
            Confirm = new RelayCommand(Click => ConfirmEdit());
            Date = orderDate;
            Articul = orderArticul;
            OrderID = orderOrderID;
            Surname = orderSurname;
            Name = orderName;
            SecondName = orderSecondName;
            Phone = orderPhone;
            ItemName = orderItem;
            AmountItem = orderAmount;
            Price = orderPrice;
            PrimeCost = orderPrimecost;
            PaymentWay = orderPaymentWay;
            DeliverWay = orderDeliverWay;
            DeliverAdress = orderDeliverAdress;
            Status = orderStatus;
            Spending = orderSpending;
            Income = orderIncome;
            Comment = orderComment;
        }

        private void ConfirmEdit()
        {
            if (_duck.UpdateOrder()) // return: true
            {
                DialogService.Instance.CloseDialog();
            }

            MessageBox.Show("Error. Try again."); // return: false
        }
    }
}
