using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class GroupCreateFromStorageViewModel : NotifyPropertyChange
    {
        private readonly SQLService _sqlService = new();
        private readonly DialogService _dialogService = DialogService.Instance;

        private readonly ObservableCollection<Storages> _storageCollection;
        public ObservableCollection<string> StoragesSelection {  get; set; }

        public string? StorageName { get; set; }
        public string? Description { get; set; }

        private string? _selectedItem;
        public string? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if(_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChange(nameof(SelectedItem));
                    UpdateConfirmCommand();
                }
            }
        }

        public ICommand Confirm { get; set; }

        public GroupCreateFromStorageViewModel(ObservableCollection<Storages> storageCollection )
        {
            _storageCollection = storageCollection;
            StoragesSelection = new ObservableCollection<string>(_storageCollection.Select(s => s.StorageName));
            SelectedItem = StoragesSelection.FirstOrDefault();
        }
        private void UpdateConfirmCommand()
        {
            var selectedStorage = _storageCollection.FirstOrDefault(s => s.StorageName == SelectedItem);

            if (selectedStorage != null)
            {
                Confirm = new RelayCommand(Click => ConfirmGroupAction(selectedStorage));
                OnPropertyChange(nameof(Confirm)); 
            }
        }
        private void ConfirmGroupAction(Storages storage)
        {
            var productGroup = new ProductGroups
            {
                Name = StorageName,
                StorageId = storage.Id,
                Description = Description,
                AmountGoodsInGroup = 0
            };

            if (_sqlService.InsertGroup(productGroup))
            {
                _dialogService.CloseDialog();
            }
            else
            {
                MessageBox.Show("ERROR IN InsertGroup FROM Storage");
            }
        }
    }
}
