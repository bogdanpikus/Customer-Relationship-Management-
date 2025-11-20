using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class StorageViewModel : NotifyPropertyChange
    {
        private readonly SQLService _sqlService = new();
        private readonly DialogService _dialogService = DialogService.Instance;
        public ObservableCollection<Storages> StoragesCollection { get; } = new();

        public ICommand CreateStorage { get; }
        public ICommand CreateGroup { get; }
        public ICommand CreateProduct { get; }
        public ICommand Editing {  get; }
        public ICommand Delete { get; }

        public ICommand OpenGroups { get; }

        public StorageViewModel() 
        {
            CreateStorage = new RelayCommand(Click => CreateStorageButton());
            Delete = new RelayCommand(Click => DeleteButton());
            OpenGroups = new RelayCommand(Click => OpenGroupsButton());
            Load();
        }
        private void Load()
        {
            //_sqlService.LoadStorages();
        }
        private void CreateStorageButton()
        {

            var storage = new Storages
            {
                StorageName = $"СКЛАД {StoragesCollection.Count + 1}",
                Address = null,
                Responsible = null,
                Phone = null,
                AmountGoodsInStorage = 0
            };

            _sqlService.InsertStorage(storage);
            StoragesCollection.Add(storage);
        }
        private void DeleteButton()
        {
            var selected = StoragesCollection.Where(o => o.IsSelected).ToList();
            foreach(var storage in selected)
            {
                _sqlService.DeleteStorage(storage.Id);
                StoragesCollection.Remove(storage);
            }

        }
        private void OpenGroupsButton()
        {
            MessageBox.Show("aHUIASHDI");
        }
    }
}
