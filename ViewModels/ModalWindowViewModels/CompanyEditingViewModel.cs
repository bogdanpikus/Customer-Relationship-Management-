using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
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

        public CompanyEditingViewModel(Company company) 
        {
            _company = company;

            CompanyName = company.CompanyName;
            INN = company.INN;
            EDPNOU = company.EDPNOU;
            Details = company.Details;
            Email = company.Email;
            Bank = company.Bank;
            CompanyPurchases = company.CompanyPurchases;
            AmountOrders = company.AmountOrders;
            CompanySumIncome = company.CompanySumIncome;
            CompanyLastOrderDate = company.CompanyLastOrderDate;

            CompanyConfirm = new RelayCommand(Click => ConfirmButton());
        }
        private void ConfirmButton()
        {
            _company.CompanyName = CompanyName;
            _company.INN = INN;
            _company.EDPNOU = EDPNOU;
            _company.Details = Details;
            _company.Email = Email;
            _company.Bank = Bank;
            _company.CompanyPurchases = CompanyPurchases;
            _company.AmountOrders = AmountOrders;
            _company.CompanySumIncome = CompanySumIncome;
            _company.CompanyLastOrderDate = CompanyLastOrderDate;

            if (_sqlService.UpdateCompanySelectedField(_company))
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
