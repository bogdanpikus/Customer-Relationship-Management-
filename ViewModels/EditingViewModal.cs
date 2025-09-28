using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class EditingViewModal
    {
        //private readonly DuckDatabase _duck = DatabaseFactory.Instance;
        public ICommand Confirm { get; }

        public DateTime Date { get; set; } 
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

        public EditingViewModal()
        {
            Confirm = new RelayCommand(Click => ConfirmEdit());
        }

        private void ConfirmEdit()
        {
            DialogService.Instance.CloseDialog();
        }
    }
}
