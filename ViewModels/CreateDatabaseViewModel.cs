using CRM.Commands;
using CRM.Models;
using CRM.Services;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class CreateDatabaseViewModel : NotifyPropertyChange
    {
        public object? CurrentControl { get; set; }

        public ICommand GoToMainWindowFromCreateControl { get; }
        public ICommand SearchPlaceForDataBase { get; }
        public ICommand ConfirmDatabase { get; }

        public string SearchPlaceForDataBaseButtonLabel { get; set; } = "...путь для сохранения файла базы данных";

        public string? DatabaseName { get; set; }

        private string? createFolderPath { get; set; }

        public bool CreateDatabaseVisiability { get; set; } = true;

        public CreateDatabaseViewModel()
        {
            ConfirmDatabase = new RelayCommand(Click => ConfirmDatabaseName(createFolderPath, DatabaseName));
            GoToMainWindowFromCreateControl = new RelayCommand(Click => GoBackToMainWindowFromCreateControl());
            SearchPlaceForDataBase = new RelayCommand(Click => BrousePlaceForDataBase());
        }
        private void BrousePlaceForDataBase()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            dialog.Title = "Confirm place for saving your database";
            if (dialog.ShowDialog() == true)
            {
                SearchPlaceForDataBaseButtonLabel = $"{dialog.FolderName}"; //showing picking folder name for saving database CreateDatabaseControl
                createFolderPath = $"{dialog.FolderName}";
                OnPropertyChange(nameof(SearchPlaceForDataBaseButtonLabel));
            }
        }
        private void ConfirmDatabaseName(string? folderPath,string? dataBaseName)
        {
            try
            {
                DatabaseFactory.CreateDatabase(false, false, folderPath, dataBaseName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            CurrentControl = new OrderControlViewModel();
            OnPropertyChange(nameof(CurrentControl));
        }
        private void GoBackToMainWindowFromCreateControl()
        {
            CreateDatabaseVisiability = false;
            OnPropertyChange(nameof(CreateDatabaseVisiability));
        }
    }
}
