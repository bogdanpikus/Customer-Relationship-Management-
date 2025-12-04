using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class GroupModalViewModel : NotifyPropertyChange
    {
        private readonly DialogService _dialogService = DialogService.Instance;
        public ObservableCollection<string> StoragesSelection { get; set; }
        private readonly ObservableCollection<Storages> _storageCollection;
        private readonly SQLService _sqlService = new();
        private ProductGroups _group;

        public string? GroupName { get; set; }
        public string? GroupDescription { get; set; }

        private string? _selectedStorage;
        public string? SelectedStorage
        {
            get => _selectedStorage;
            set
            {
                if(_selectedStorage != value)
                {
                    _selectedStorage = value;
                    OnPropertyChange(nameof(SelectedStorage));
                }
            }
        }

        private readonly string _originalStorageName;

        public ICommand Confirm { get; set; }

        public GroupModalViewModel(ProductGroups group, ObservableCollection<Storages> storageCollection)
        {
            _group = group;
            _storageCollection = storageCollection; // для перемещения групп в разные StoragesSelection

            GroupName = _group.Name;
            GroupDescription = _group.Description;

            StoragesSelection = new ObservableCollection<string>(_storageCollection.Select(o => o.StorageName));
            _originalStorageName = _storageCollection.First(s => s.Id == _group.StorageId).StorageName;
            SelectedStorage = _originalStorageName;

            Confirm = new RelayCommand(Click => ConfirmAction());
        }

        private void ConfirmAction()
        {
            var newStorage = _storageCollection.First(s => s.StorageName == SelectedStorage);

            if (newStorage.StorageName == _originalStorageName)
            {
                _group.Name = GroupName;
                _group.Description = GroupDescription;
            }
            else
            {
                _group.Name = GroupName;
                _group.Description = GroupDescription;
                _group.StorageId = newStorage.Id;
                MessageBox.Show($"{_group.Name} was mooved to {newStorage.StorageName}");
            }

            if (_sqlService.UpdateGroupSQLAction(_group))
            {
                _dialogService.CloseDialog();
            }
            else
            {
                MessageBox.Show("SQL ERROR, TRY LATER");
            }
        }
    }
    
}
