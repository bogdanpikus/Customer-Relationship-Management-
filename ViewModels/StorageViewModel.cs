using CRM.Commands;
using CRM.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class StorageViewModel
    {
        public ObservableCollection<Storages> StoragesCollection { get; } = new();

        public string StorageName { get; set; }
        public ICommand CreateStorage { get; }
        public ICommand CreateGroup { get; }
        public ICommand CreateProduct { get; }
        public ICommand Editing {  get; }
        public ICommand Delete { get; }

        public ICommand OpenStorage { get; }

        public StorageViewModel() 
        {
            CreateStorage = new RelayCommand(Click => CreateStorageButton());
        }
        private void CreateStorageButton()
        {
            StorageName = "Storage 1";

            var storage = new Storages
            {
                Address = null,
                Responsible = null,
                Phone = null,
                AmountGoodsInStorage = 0
            };

            StoragesCollection.Add(storage);
        }
    }
}
