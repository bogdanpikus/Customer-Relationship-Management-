using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class CustomerEditingViewModel
    {
        private readonly Customer _customer;
        private readonly SQLService _sqlService = new();

        public string? SecondName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone {  get; set; }
        public int? AmountOrders { get; set; }
        public decimal? CustomerSumIncome { get; set; }
        public string? Items {  get; set; }
        public DateTime? LastDateOrder { get; set; }

        public ICommand CustomerConfirm { get; }

        public CustomerEditingViewModel(Customer customer) 
        {
            _customer = customer;
         
            SecondName = customer.SecondName;
            Name = customer.Name;
            Surname = customer.Surname;
            Phone = customer.Phone;
            AmountOrders = customer.AmountOrders;
            CustomerSumIncome = customer.CustomerSumIncome;
            Items = customer.CustomerPurchases;
            LastDateOrder = customer.CustomerLastOrderDate;

            CustomerConfirm = new RelayCommand(Click => ConfirmButton());
        }
        private void ConfirmButton()
        {
            _customer.SecondName = SecondName;
            _customer.Name = Name; 
            _customer.Surname = Surname;
            _customer.Phone = Phone;
            _customer.AmountOrders = AmountOrders;  
            _customer.CustomerSumIncome = CustomerSumIncome;
            _customer.CustomerPurchases = Items;
            _customer.CustomerLastOrderDate = LastDateOrder;


            if (_sqlService.UpdateCustomerSelectedField(_customer))
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
