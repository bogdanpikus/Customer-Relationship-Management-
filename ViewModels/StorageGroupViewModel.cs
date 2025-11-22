using CRM.Commands;
using CRM.Services;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class StorageGroupViewModel : NotifyPropertyChange
    {
        public ICommand GoBackToStorages { get; set; }
        public bool CurremtVisiability { get; set; }

        public StorageGroupViewModel()
        {
            CurremtVisiability = true;
            GoBackToStorages = new RelayCommand(Click => GoBackToStoragesAction());
        }
        private void GoBackToStoragesAction()
        {
            CurremtVisiability = false;
            OnPropertyChange(nameof(CurremtVisiability));
        }
    }
}
