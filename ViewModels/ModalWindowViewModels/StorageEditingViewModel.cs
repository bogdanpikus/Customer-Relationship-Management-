using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Net.Http.Headers;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class StorageEditingViewModel
    {
        private readonly Storages _storage;
        private readonly SQLService _sqlService = new();

        public string? StorageName { get; set; }
        public string? Address { get; set; }
        public string? Phone {  get; set; }

        public ICommand Confirm {  get; }

        public StorageEditingViewModel(Storages storage) 
        {
            _storage = storage;
            StorageName = _storage.StorageName;
            Address = _storage.Address;
            Phone = _storage.Phone;

            Confirm = new RelayCommand(Click => ConfirmAction());
        }
        private void ConfirmAction()
        {
            var storage = new Storages
            {
                StorageName = StorageName,
                Address = Address,
                Phone = Phone
            };

            _sqlService.StorageUpdateData(storage);
        }
    }
}
