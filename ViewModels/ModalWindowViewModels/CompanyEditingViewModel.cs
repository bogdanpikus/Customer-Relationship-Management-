using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class CompanyEditingViewModel
    {
        private readonly Company _company;
        private readonly SQLService _sqlService = new();

        public string? CompanyName { get; set; }
        public int? INN { get; set; }
        public int? EDPNOU { get; set; }
        public string? Details { get; set; }
        public string? Email { get; set; }
        public string? Bank {  get; set; }
        public string? CompanyPurchases { get; set; } // ?? вопрос по "Как это будет работать?"
        public int? AmountOrders { get; set; }
        public decimal? CompanySumIncome { get; set; }
        public DateTime? CompanyLastOrderDate { get; set; }

        public ICommand CompanyConfirm { get; }

        public CompanyEditingViewModel(Company company, ObservableCollection<Company> companies) 
        {
            CompanyConfirm = new RelayCommand(Click => ConfirmButton(companies));
            _company = company;
            CompanyName = _company.CompanyName;
            INN = _company.INN;
            EDPNOU = _company.EDPNOU;
            Details = _company.Details;
            Email = _company.Email;
            Bank = _company.Bank;
            CompanyPurchases = _company.CompanyPurchases;
            AmountOrders = _company.AmountOrders;
            CompanySumIncome = _company.CompanySumIncome;
            CompanyLastOrderDate = _company.CompanyLastOrderDate;
        }
        private void ConfirmButton(ObservableCollection<Company> companies)
        {
            var company = new Company
            {
                CompanyName = CompanyName,
                INN = INN,
                EDPNOU = EDPNOU,
                Details = Details,
                Email = Email,
                Bank = Bank,
                CompanyPurchases = CompanyPurchases,
                AmountOrders = AmountOrders,
                CompanySumIncome= CompanySumIncome,
                CompanyLastOrderDate= CompanyLastOrderDate
            };

            companies.Insert(0, company);
            _sqlService.UpdateCompanySelectedField();
        }
    }
}
