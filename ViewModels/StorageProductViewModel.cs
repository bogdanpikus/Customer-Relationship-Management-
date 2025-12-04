using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class StorageProductViewModel : NotifyPropertyChange
    {
        public ObservableCollection<Products> ProductCollection { get; set; } = new();
        public bool CurrentVisiability { get; set; }

        public ICommand GoBackToGroups { get; set; }
        public ICommand CreateProduct { get; set; }

        public bool ContentControlVisiability { get; set; }
        public object ProductContentControl {  get; set; }

        public StorageProductViewModel()
        {
            CurrentVisiability = true;
            ContentControlVisiability = false;

            GoBackToGroups = new RelayCommand(Click => GoBackToGroupsAction());
            CreateProduct = new RelayCommand(Click => CreateProductAction());
        }
        private void GoBackToGroupsAction()
        {
            CurrentVisiability = false;
            OnPropertyChange(nameof(CurrentVisiability));
        }
        private void CreateProductAction()
        {
            ContentControlVisiability = true;
            ProductContentControl = new ProductCreateViewModel(ProductCollection);
            OnPropertyChange(nameof(ProductContentControl));
            OnPropertyChange(nameof(ContentControlVisiability));
        }
    }
}
