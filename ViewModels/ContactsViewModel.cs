using CRM.Commands;
using CRM.Models;
using CRM.Services;
using CRM.ViewModels.ModalWindowViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ICommand EditingModalWindowButton { get; }

        public ContactsViewModel() 
        {
            OpenModalWindowButton = new RelayCommand(Click => OpenModalWindow());
            EditingModalWindowButton = new RelayCommand(Click => EditingModalWindow());
            SQLCustomersLoad();
            SQLCompanyLoad();
        }

        private void SQLCustomersLoad() // TIP: изначальная загрузка клиентов в первую таблицу при загрузке страницы 
        {
            var customersList = _sqlService.ExtractCustomers();
            foreach (var customer in customersList)
            {
                CustomerCollection.Add(customer);
            }
        }
        private void SQLCompanyLoad() // TIP: изначальная загрузка компаний во вторую таблицу при загрузке страницы
        {
            var companiesList = _sqlService.ExtractCompanies();
            foreach (var company in companiesList)
            {
                CompanyCollection.Add(company);
            }
        }
        private void OpenModalWindow()
        {
            _dialogService.ShowDialog(new CustomerAddViewModel(CustomerCollection, CompanyCollection));
            
        }

        //TODO: эти 2 метода в один в метод EditingModalWindow()
        private void EditingModalWindow()
        {
            var customerIsSelected = CustomerCollection.Where(o => o.IsSelected).ToList();
            if(customerIsSelected == null)
            {
                var companyIsSelected = CompanyCollection.Where(o => o.IsSelected).ToList();
                foreach(var company in companyIsSelected)
                {
                    _dialogService.ShowDialog(new CompanyEditingViewModel());
                }
            }
            else
            {
                foreach (var item in customerIsSelected)
                {
                    _dialogService.ShowDialog(new CustomerEditingViewModel());
                }
            }
        }
    }
}
