using CRM.Commands;
using CRM.Services;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels.ModalWindowViewModels
{
    public class ToolbarCreateViewModel : NotifyPropertyChange
    {
        public ICommand Brouse { get; }
        public ICommand Create { get; }
        public string? DatabaseName { get; set; }
        public string? SearchPlaceForDataBaseButtonLabel { get; set; }

        private string? createFolderPath { get; set; }

        public ToolbarCreateViewModel() 
        {
            Brouse = new RelayCommand(Click => BrouseAction());
            Create = new RelayCommand(Click => CreateAction(createFolderPath, DatabaseName));
        }
        private void BrouseAction()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            dialog.Title = "Confirm place for saving your database";
            if(dialog.ShowDialog() == true)
            {
                SearchPlaceForDataBaseButtonLabel = $"{dialog.FolderName}";
                createFolderPath = $"{dialog.FolderName}";
                OnPropertyChange(nameof(SearchPlaceForDataBaseButtonLabel));
            }
        }
        private void CreateAction(string? folderPath, string? databaseName)
        {
            try
            {
                DatabaseFactory.CreateDatabase(false, false, folderPath, databaseName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            DialogService.Instance.CloseDialog();
        }
    }
}
