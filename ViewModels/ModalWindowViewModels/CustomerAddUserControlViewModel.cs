using CRM.Commands;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class CustomerAddUserControlViewModel
    {
        public ICommand CompanyConfirm { get; }

        public CustomerAddUserControlViewModel() 
        {
            CompanyConfirm = new RelayCommand(Click => CompanyConfirmMethod());
        }

        private void CompanyConfirmMethod()
        {
            MessageBox.Show("CompanyConfirmMethod");
        }
    }
}
