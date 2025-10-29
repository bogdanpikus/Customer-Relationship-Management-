using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class CustomerAddViewModel : NotifyPropertyChange
    {
        public ObservableCollection<string> ComboItems { get; } = ["Клиент", "Юр.лицо"];
        public object CurrentContent { get; set; }
        public bool CustomerModalControlVisiability { get; set; }

        private string _selectedItem;
        public string SelectedItems
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChange(nameof(SelectedItems));
                    UpdateCurrentView();
                }
            }
        }
        public CustomerAddViewModel()
        {
            SelectedItems = ComboItems[0];
        }
        private void UpdateCurrentView()
        {
            switch (SelectedItems)
            {
                case "Клиент": GoBackToMainView(); break;
                case "Юр.лицо": OpenUserControl(); break;
            }
        }
        private void GoBackToMainView()
        {
            CustomerModalControlVisiability = false;
            OnPropertyChange(nameof(CustomerModalControlVisiability));
        }
        private void OpenUserControl()
        {
            CurrentContent = new CustomerAddUserControlViewModel();
            CustomerModalControlVisiability = true;
            OnPropertyChange(nameof(CurrentContent));
            OnPropertyChange(nameof(CustomerModalControlVisiability));
        }
    }
}
