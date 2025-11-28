using CRM.Commands;
using CRM.Models;
using CRM.Services;
using CRM.ViewModels.ModalWindowViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public ICommand OpenGroups { get; set; }
        private string _storageName;
        public string StorageName { get => _storageName; set { if (_storageName != value) { _storageName = value; OnPropertyChange(nameof(StorageName)); } } }

        private int _count { get; set; } = 0;
        public int Count
        {
            get => _count;
            set
            {
                if(_count != value)
                {
                    _count = value;
                    OnPropertyChange(nameof(Count));
                }
            }
        }

        public object? CurrentView { get; set; }
        public bool ContentVisiability { get; set; }

        public StorageViewModel() 
        {
            CreateStorage = new RelayCommand(Click => CreateStorageButton());
            Delete = new RelayCommand(Click => DeleteButton());
            OpenGroups = new RelayCommand(obj => OpenGroupsAction(obj));
            Editing = new RelayCommand(Click => StorageEditing());
            Load();
        }
        private void Load()
        {
            var storages = _sqlService.LoadStorages();
            foreach(var storage in storages)
            {
                StoragesCollection.Insert(0, storage);
            }
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
            var isSelected = StoragesCollection.Where(o => o.IsSelected).ToList();
            foreach (var storage in isSelected)
            {
                Debug.WriteLine($"storage.id: {storage.Id}");
                if (_sqlService.DeleteStorage(storage.Id))
                {
                    StoragesCollection.Remove(storage);
                }
                else
                {
                    MessageBox.Show("ERROR SQL DELETE, TRY LATER");
                }
            }

        }
        private void OpenGroupsAction(object obj)
        {
            var storage = obj as Storages;
            if (storage == null)
            {
                return;
            }

            ContentVisiability = true;
            CurrentView = new StorageGroupViewModel(storage);
            OnPropertyChange(nameof(ContentVisiability));
            OnPropertyChange(nameof(CurrentView));
        }
        private void StorageEditing()
        {
           var isSelected = StoragesCollection.Where(o => o.IsSelected).ToList();
            foreach(var storage in isSelected)
            {
                _dialogService.ShowDialog(new StorageEditingViewModel(storage));
            }
        }
    }
}
