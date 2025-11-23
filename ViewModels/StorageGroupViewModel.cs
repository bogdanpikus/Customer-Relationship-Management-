using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class StorageGroupViewModel : NotifyPropertyChange
    {
        public ObservableCollection<ProductGroups> GroupCollection { get; } = new();
        public bool CurremtVisiability { get; set; }

        public ICommand GoBackToStorages { get; set; }
        public ICommand CreateGroup {  get; set; }

        private readonly SQLService _sqlService = new();
        private readonly Storages _storage;

        public StorageGroupViewModel(Storages storage)
        {
            _storage = storage;

            CurremtVisiability = true;
            GoBackToStorages = new RelayCommand(Click => GoBackToStoragesAction());
            CreateGroup = new RelayCommand(Click => CreateGroupAction(_storage.Id));
            Load(_storage.Id);
        }
        private void Load(int id)
        {
            _sqlService.LoadGroups(idaas);
        }
        private void GoBackToStoragesAction()
        {
            CurremtVisiability = false;
            OnPropertyChange(nameof(CurremtVisiability));
        }
        private void CreateGroupAction(int id)
        {
            var group = new ProductGroups 
            {
                Name = "Группа 1",
                StorageId = id
            };

            GroupCollection.Add(group);
            _sqlService.InsertGroup(group);
        }
    }
}
