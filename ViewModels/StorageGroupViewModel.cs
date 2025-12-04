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
    public class StorageGroupViewModel : NotifyPropertyChange
    {
        public ObservableCollection<ProductGroups> GroupCollection { get; } = new();
        private readonly ObservableCollection<Storages> _storageCollection;
        private readonly DialogService _dialogService = DialogService.Instance;
        public bool CurrentVisiability { get; set; }

        public ICommand GoBackToStorages { get; set; }
        public ICommand CreateGroup {  get; set; }
        public ICommand Editing {  get; set; }
        public ICommand Delete { get; set; }
        public ICommand OpenProducts { get; set; }

        private readonly SQLService _sqlService = new();
        private readonly Storages _storage;

        public object CurrentView { get; set; }
        public bool ContentVisiability { get; set; }


        public StorageGroupViewModel(Storages storage, ObservableCollection<Storages> storageCollection)
        {
            _storage = storage;
            _storageCollection = storageCollection;

            CurrentVisiability = true;
            GoBackToStorages = new RelayCommand(Click => GoBackToStoragesAction());
            CreateGroup = new RelayCommand(Click => CreateGroupAction(_storage.Id));
            Editing = new RelayCommand(Click => EditingGroupAction(_storageCollection));
            Delete = new RelayCommand(Click => DeleteGroupAction());
            OpenProducts = new RelayCommand(obj => OpenProductsAction(obj));
            Load(_storage.Id);
        }
        private void Load(int id) // РАБОТАЕТ
        {
            GroupCollection.Clear();

            var groups = _sqlService.LoadGroups(id);
            foreach(var group in groups)
            {
                GroupCollection.Add(group);
                Debug.WriteLine($"{group.Name} | Hash: {group.GetHashCode()} | Selected: {group.IsSelected}");
            }
        }
        private void GoBackToStoragesAction()
        {
            CurrentVisiability = false;
            OnPropertyChange(nameof(CurrentVisiability));
        }
        private void CreateGroupAction(int id)
        {
            var storageIdCount = GroupCollection.Count(o => o.StorageId.Equals(id)) + 1; // например: storageIdCount = 2
            var group = new ProductGroups 
            {
                Name = $"Группа {storageIdCount}",
                StorageId = id
            };

            GroupCollection.Add(group);
            _sqlService.InsertGroup(group);
        }
        private void EditingGroupAction(ObservableCollection<Storages> storage) //  РАБОТАЕТ
        {
            var isSelected = GroupCollection.Where(o => o.IsSelected).ToList();
            foreach(var selected in isSelected)
            {
                _dialogService.ShowDialog(new GroupModalViewModel(selected, storage)); // FIX: НЕ ОБНОВЛЯЕТСЯ СРАЗУ ПОСЛЕ ПЕРЕМЕЩЕНИЯ САМО
            }
        }
        private void DeleteGroupAction() // РАБОТАЕТ
        {
            var isSelected = GroupCollection.Where(o => o.IsSelected).ToList();
            foreach (var group in isSelected)
            {
                if (_sqlService.DeleteGroupSQLAction(group.Id, group.StorageId))
                {
                    Debug.WriteLine($"{group.Name} was DELETED");
                    GroupCollection.Remove(group);
                }
                else
                {
                    MessageBox.Show($"{group.Name} was not DELETED, ERROR MESSASE");
                }
            }
        }
        private void OpenProductsAction(object obj) // РАБОТАЕТ
        {
            var group = obj as ProductGroups;
            if (group == null)
            {
                return;
            }

            CurrentView = new StorageProductViewModel();
            ContentVisiability = true;
            OnPropertyChange(nameof(CurrentView));
            OnPropertyChange(nameof(ContentVisiability));
        }
    }
}