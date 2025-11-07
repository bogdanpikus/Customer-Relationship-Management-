using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class CustomerAddViewModel : NotifyPropertyChange
    {
        private readonly DialogService _dialogService = new();
        private readonly SQLService _sqlService = new();
        public ObservableCollection<string> ComboItems { get; } = ["Клиент", "Юр.лицо"];
        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set
            {
                if(_companies != value)
                {
                    _companies = value;
                    OnPropertyChange(nameof(Companies));
                }
            }
        }

        public object CurrentContent { get; set; }
        public bool CustomerModalControlVisiability { get; set; }

        public ICommand CustomerConfirm { get;}

        private string _selectedItem;
        public string SelectedItems
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChange(nameof(SelectedItems));
                    UpdateCurrentView();
                }
            }
        }

        public DateTime? CustomerLastOrderDate { get; set; }
        public string? SecondName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone {  get; set; }
        public int? AmountOrders { get; set; }
        public decimal? CustomerSumIncome { get; set; }
        public string? CustomerPurchases { get; set; }


        public CustomerAddViewModel(ObservableCollection<Customer> customers, ObservableCollection<Company> companies)
        {
            Companies = companies;
            SelectedItems = ComboItems[0];
            CustomerConfirm = new RelayCommand(Click => CustomerConfirmMethod(customers));
        }
        private void UpdateCurrentView()
        {
            switch (SelectedItems)
            {
                case "Клиент": GoBackToMainView(); break;
                case "Юр.лицо": OpenUserControl(); break;
            }
        }
        private void GoBackToMainView()
        {
            CustomerModalControlVisiability = false;
            OnPropertyChange(nameof(CustomerModalControlVisiability));
        }
        private void OpenUserControl()
        {
            CurrentContent = new CompanyAddViewMode(Companies);
            CustomerModalControlVisiability = true;
            OnPropertyChange(nameof(CurrentContent));
            OnPropertyChange(nameof(CustomerModalControlVisiability));
        }
        private void CustomerConfirmMethod(ObservableCollection<Customer> customers)
        {
            var customer = new Customer
            {
                SecondName = SecondName,
                Name = Name,
                Surname = Surname,
                Phone = Phone,
                AmountOrders = AmountOrders,
                CustomerSumIncome = CustomerSumIncome,
                CustomerPurchases = CustomerPurchases,
                CustomerLastOrderDate = CustomerLastOrderDate
            };

            _sqlService.SQLCustomerInsert(customer);
             customers.Insert(0, customer);
            _dialogService.CloseDialog();
        }
    }
}
