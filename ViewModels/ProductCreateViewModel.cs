using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class ProductCreateViewModel : NotifyPropertyChange
    {
        public bool GridVisiability { get; set; }

        public ICommand GoBackToProducts { get; }

        public ProductCreateViewModel(ObservableCollection<Products> productCollection) 
        {
            GridVisiability = true;
            GoBackToProducts = new RelayCommand(Click => GoBackToProductsAction());
        }
        private void GoBackToProductsAction()
        {
            GridVisiability = false;
            OnPropertyChange(nameof(GridVisiability));
        }
    }
}
