using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class StorageEditingViewModel
    {
        private readonly Storages _storage;
        private readonly SQLService _sqlService = new();
        private readonly DialogService _dialogService = DialogService.Instance;

        public string? StorageName { get; set; }
        public string? Address { get; set; }
        public string? Responsible { get; set; }
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
            _storage.StorageName = StorageName;
            _storage.Address = Address;
            _storage.Responsible = Responsible;
            _storage.Phone = Phone;

            Debug.Write($"_storage.id: {_storage.Id}");

           if (_sqlService.StorageUpdateData(_storage))
            {
                _dialogService.CloseDialog();
            }
            else
            {
                MessageBox.Show("There is some ERROR during running SQL UPDATE");
            }
        }
    }
}
