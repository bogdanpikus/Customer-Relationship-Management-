using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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

        public CustomerEditingViewModel(Customer customer, ObservableCollection<Customer> customers) 
        {
            CustomerConfirm = new RelayCommand(Click => ConfirmButton(customers));
            _customer = customer;
            SecondName = customer.SecondName;
            Name = customer.Name;
            Surname = customer.Surname;
            Phone = customer.Phone;
            AmountOrders = customer.AmountOrders;
            CustomerSumIncome = customer.CustomerSumIncome;
            Items = customer.CustomerPurchases;
            LastDateOrder = customer.CustomerLastOrderDate;
        }
        private void ConfirmButton(ObservableCollection<Customer> customers)
        {
            var customer = new Customer
            {
                SecondName = SecondName,
                Name = Name,
                Surname = Surname,
                Phone = Phone,
                AmountOrders = AmountOrders,
                CustomerSumIncome = CustomerSumIncome,
                CustomerPurchases = Items,
                CustomerLastOrderDate = LastDateOrder
            };

            customers.Insert(0, customer);
            _sqlService.UpdateCustomerSelectedField();
        }
    }
}
