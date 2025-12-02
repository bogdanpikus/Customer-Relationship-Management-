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

        public StorageProductViewModel()
        {
            CurrentVisiability = true;

            GoBackToGroups = new RelayCommand(Click => GoBackToGroupsAction());
        }
        private void GoBackToGroupsAction()
        {
            CurrentVisiability = false;
            OnPropertyChange(nameof(CurrentVisiability));
        }
    }
}
