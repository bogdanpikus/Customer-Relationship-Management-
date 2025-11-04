using CRM.Commands;
using CRM.Services;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class CustomerAddUserControlViewModel
    {
        private readonly SQLService _sqlService = new();
        public ICommand CompanyConfirm { get; }

        public CustomerAddUserControlViewModel() 
        {
            CompanyConfirm = new RelayCommand(Click => CompanyConfirmMethod());
        }

        private void CompanyConfirmMethod()
        {
            _sqlService.SQLCompanyInsert();
        }
    }
}
