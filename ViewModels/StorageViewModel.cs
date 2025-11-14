using CRM.Models;
using System.Collections.ObjectModel;

namespace CRM.ViewModels
{
    public class StorageViewModel
    {
        public ObservableCollection<Storages> Storages { get; } = new();

        public StorageViewModel() 
        {

        }
    }
}
