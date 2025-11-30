using CRM.Commands;
using CRM.Models;
using CRM.Services;
using CRM.ViewModels.ModalWindowViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class StorageGroupViewModel : NotifyPropertyChange
    {
        public ObservableCollection<ProductGroups> GroupCollection { get; } = new();
        private readonly DialogService _dialogService = DialogService.Instance;
        public bool CurremtVisiability { get; set; }

        public ICommand GoBackToStorages { get; set; }
        public ICommand CreateGroup {  get; set; }
        public ICommand Editing {  get; set; }
        public ICommand Delete { get; set; }
        public ICommand OpenProducts { get; set; }

        private readonly SQLService _sqlService = new();
        private readonly Storages _storage;


        public StorageGroupViewModel(Storages storage)
        {
            _storage = storage;

            CurremtVisiability = true;
            GoBackToStorages = new RelayCommand(Click => GoBackToStoragesAction());
            CreateGroup = new RelayCommand(Click => CreateGroupAction(_storage.Id));
            Editing = new RelayCommand(Click => EditingGroupAction());
            Delete = new RelayCommand(Click => DeleteGroupAction());
            OpenProducts = new RelayCommand(obj => OpenProductsAction(obj));
            Load(_storage.Id);
        }
        private void Load(int id) // РАБОТАЕТ
        {
            var groups = _sqlService.LoadGroups(id);
            foreach(var group in groups)
            {
                GroupCollection.Add(group);
            }
        }
        private void GoBackToStoragesAction()
        {
            CurremtVisiability = false;
            OnPropertyChange(nameof(CurremtVisiability));
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
        private void EditingGroupAction() // НЕ РАБОТАЕТ
        {
            var isSelected = GroupCollection.Where(o => o.isSelected).ToList();
            foreach(var selected in isSelected)
            {
                _dialogService.ShowDialog(new GroupModalViewModel());
            }
        }
        private void DeleteGroupAction() // НЕ РАБОТАЕТ
        {
            var isSelected = GroupCollection.Where(o => o.isSelected).ToList();
            foreach (var selected in isSelected)
            {
                MessageBox.Show("selected in Selected");
            }
        }
        private void OpenProductsAction(object obj) // НЕ РАБОТАЕТ
        {
            var group = obj as ProductGroups;
            if (group != null)
            {
                return;
            }

            MessageBox.Show("OpenProductsAction");
        }
    }
}