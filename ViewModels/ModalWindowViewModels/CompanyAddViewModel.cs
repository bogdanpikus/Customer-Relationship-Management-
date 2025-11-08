using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class CompanyAddViewMode
    {
        private readonly DialogService _dialogService = new();
        private readonly SQLService _sqlService = new();
        public ICommand CompanyConfirm { get; }

        public DateTime? CompanyLastOrderDate { get; set; }
        public string? CompanyName { get; set; }
        public int? INN { get; set; }
        public int? EDPNOU { get; set; }
        public string? Details { get; set; }
        public string Email { get; set; }
        public int? AmountOrders { get; set; }
        public string? CompanyPurchases { get; set; }
        public decimal? CompanySumIncome { get; set; }

        public CompanyAddViewMode(ObservableCollection<Company> companies) 
        {
            CompanyConfirm = new RelayCommand(Click => CompanyConfirmMethod(companies));
        }

        private void CompanyConfirmMethod(ObservableCollection<Company> companies)
        {
            var company = new Company
            {
                CompanyName = CompanyName,
                INN = INN,
                EDPNOU = EDPNOU,
                Details = Details,
                Email = Email,
                AmountOrders = AmountOrders,
                CompanyPurchases = CompanyPurchases,
                CompanySumIncome = CompanySumIncome,
                CompanyLastOrderDate = CompanyLastOrderDate
            };

            _sqlService.SQLCompanyInsert(company);
            companies.Insert(0, company);
            _dialogService.CloseDialog(); // НЕ РАБОТАЕТ
        }
    }
}
