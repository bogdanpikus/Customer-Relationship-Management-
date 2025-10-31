using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class CustomerAddViewModel : NotifyPropertyChange
    {
        public ObservableCollection<string> ComboItems { get; } = ["Клиент", "Юр.лицо"];
        public object CurrentContent { get; set; }
        public bool CustomerModalControlVisiability { get; set; }

        public ICommand CustomerConfirm { get;}

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
        public CustomerAddViewModel(ObservableCollection<Customer> customers, ObservableCollection<Company> companies)
        {
            SelectedItems = ComboItems[0];

            CustomerConfirm = new RelayCommand(Click => CustomerConfirmMethod());
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
        private void CustomerConfirmMethod()
        {
            MessageBox.Show("CustomerConfirmMethod");
        }
    }
}
