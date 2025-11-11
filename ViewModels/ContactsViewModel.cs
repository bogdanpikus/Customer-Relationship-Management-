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
        private readonly DialogService _dialogService = DialogService.Instance;
        private readonly SQLService _sqlService = new();
        public ObservableCollection<Customer> CustomerCollection { get; } = new();
        public ObservableCollection<Company> CompanyCollection { get; } = new();
       

        public bool CustomerModalControlVisiability { get; set; } = false;
        public ICommand OpenModalWindowButton { get; }
        public ICommand EditingModalWindowButton { get; }
        public ICommand DeleteButton { get; }


        public ContactsViewModel() 
        {
            OpenModalWindowButton = new RelayCommand(Click => OpenModalWindow());
            EditingModalWindowButton = new RelayCommand(Click => EditingModalWindow());
            DeleteButton = new RelayCommand(Click => SelectedDelete());
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
        private void OpenModalWindow() // TIP: добавление клиента или компании
        {
            _dialogService.ShowDialog(new CustomerAddViewModel(CustomerCollection, CompanyCollection));   
        }

        //FIX: ИСПРАВЛЕНО
        private void EditingModalWindow()
        {
            var customerIsSelected = CustomerCollection.Where(o => o.IsSelected).ToList();
            foreach (var customer in customerIsSelected)
            {
                _dialogService.ShowDialog(new CustomerEditingViewModel(customer));
            }

            var companyIsSelected = CompanyCollection.Where(o => o.IsSelected).ToList();
            foreach (var company in companyIsSelected)
            {
                _dialogService.ShowDialog(new CompanyEditingViewModel(company));
            }
        }
        //FIX: ИСПРАВЛЕНО
        private void SelectedDelete()
        {
            var customerSelected = CustomerCollection.Where(o => o.IsSelected).ToList();
            foreach (var customer in customerSelected)
            {
                _sqlService.CustomerSelectedDelete(customer.Id);
                Debug.WriteLine($"Company ID = {customer.Id}");
                CustomerCollection.Remove(customer);
            }

            var companySelected = CompanyCollection.Where(o => o.IsSelected).ToList();
            foreach (var company in companySelected)
            {
                _sqlService.CompanySelectedDelete(company.Id);
                Debug.WriteLine($"Company ID = {company.Id}");
                CompanyCollection.Remove(company);
            }

        }
    }
}
