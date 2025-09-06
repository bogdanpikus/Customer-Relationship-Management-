using CRM.Commands;
using CRM.Models;
using CRM.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class OpenDatabaseViewModel : NotifyPropertyChange
    {
        private DuckDatabase? _duckdb;

        public ICommand GoToMainWindowFromOpenControl { get; }
        public ICommand SearchDataBaseFileToOpen { get; }
        public ICommand OpenDuckDatabaseFile { get; }

        private string? openFilePath { get; set; }
        public string SearchDatabaseFileToOpenButtonLabel { get; set; } = "...путь где находится файл базы данных";

        public bool OpenDatabaseVisiability { get; set; } = true;

        public OpenDatabaseViewModel()
        {
            GoToMainWindowFromOpenControl = new RelayCommand(Click => GoBackToMainWindowFromOpenControl());
            SearchDataBaseFileToOpen = new RelayCommand(Click => BrouseDataBaseFileToOpen());
            OpenDuckDatabaseFile = new RelayCommand(Click => OpenDuckDBFile(openFilePath));
        }
        private void BrouseDataBaseFileToOpen()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Find your database file";
            dialog.Filter = "Database files (*.duckdb) | *.duckdb";
            if (dialog.ShowDialog() == true)
            {
                SearchDatabaseFileToOpenButtonLabel = $"{dialog.FileName}"; //showing picking database file in OpenDatabaseControl
                openFilePath = $"{dialog.FileName}";
                OnPropertyChange(nameof(SearchDatabaseFileToOpenButtonLabel));
            }
            else { MessageBox.Show("Error picking database file"); }
        }
        private void OpenDuckDBFile(string filePath) // opening database file 
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            try
            {
                _duckdb = new DuckDatabase(false, true, filePath, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GoBackToMainWindowFromOpenControl()
        {
            OpenDatabaseVisiability = false;
            OnPropertyChange(nameof(OpenDatabaseVisiability));
        }
    }
}
