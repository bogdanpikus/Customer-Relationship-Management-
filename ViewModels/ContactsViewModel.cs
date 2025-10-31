using CRM.Commands;
using CRM.Models;
using CRM.Services;
using CRM.ViewModels.ModalWindowViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class ContactsViewModel : NotifyPropertyChange
    {
        private readonly DialogService _dialogService = new();
        private readonly SQLService _sqlService = new();
        public ObservableCollection<Customer> CustomerCollection { get; } = new();
        public ObservableCollection<Company> CompanyCollection { get; } = new();
       

        public bool CustomerModalControlVisiability { get; set; } = false;
        public ICommand OpenModalWindowButton { get; }

        public ContactsViewModel() 
        {
            OpenModalWindowButton = new RelayCommand(Click => OpenModalWindow());
        }

        private void SQLCustomersLoad()
        {
            _sqlService.ExtractCustomers();
        }
        private void SQLCompanyLoad()
        {
            _sqlService.ExtractCompanies();
        }
        private void OpenModalWindow()
        {
            _dialogService.ShowDialog(new CustomerAddViewModel(CustomerCollection, CompanyCollection));
            
        }
        private void OpenCustomerEditing()
        {
            var isSelected = CustomerCollection.Where(o => o.IsSelected).ToList();
            foreach (var item in isSelected) 
            {
                _dialogService.ShowDialog(new CustomerEditingViewModel());
            }
        }
        private void OpenCompanyEditing()
        {
            var isSelected = CompanyCollection.Where(o => o.IsSelected).ToList();
            foreach (var item in isSelected)
            {
                _dialogService.ShowDialog(new CompanyEditingViewModel());
            }
        }
    }
}
