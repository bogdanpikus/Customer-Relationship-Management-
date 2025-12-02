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
        public ObservableCollection<string> StoragesSelection { get; set; }
        private readonly ObservableCollection<Storages> _storageCollection;
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
                    UpdateConfirmAction();
                }
            }
        }

        private readonly string? _defaultStorageName;

        public ICommand Confirm { get; set; }

        public GroupModalViewModel(ProductGroups group, ObservableCollection<Storages> storageCollection)
        {
            _group = group;
            _storageCollection = storageCollection; // для перемещения групп в разные StoragesSelection

            GroupName = _group.Name;
            GroupDescription = _group.Description;

            StoragesSelection = new ObservableCollection<string>(_storageCollection.Select(o => o.StorageName));
            SelectedStorage = StoragesSelection.FirstOrDefault();
            var defaultStorage = _storageCollection.FirstOrDefault(o => o.StorageName == SelectedStorage);
            _defaultStorageName = SelectedStorage;

            Confirm = new RelayCommand(Click => ConfirmAction(defaultStorage, _defaultStorageName));
        }
        private void UpdateConfirmAction()
        {
            var selectedStorage = _storageCollection.FirstOrDefault(o => o.StorageName == SelectedStorage);
            if(selectedStorage != null)
            {
                Confirm = new RelayCommand(Click => ConfirmAction(selectedStorage, _defaultStorageName));
                OnPropertyChange(nameof(Confirm));
            }
            
        }
        private void ConfirmAction(Storages newStorage, string defaultStorageName)
        {
            if (defaultStorageName == newStorage.StorageName)
            {
                MessageBox.Show("StorageName == default"); // НЕ меняем положения группы
            }
            else
            {
                MessageBox.Show("StorageName != default"); // МЕНЯЕМ на выбранный склад по средством переписывание StorageId группы?
            }
        }
    }
}
