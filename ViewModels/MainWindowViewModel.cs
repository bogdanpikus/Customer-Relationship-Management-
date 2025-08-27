using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CRM.Commands;
using CRM.Models;
using DuckDB.NET.Data;
using Microsoft.Win32;

namespace CRM.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //private DuckDB _duckdb;

        public ICommand CreateDatabase { get; }
        public ICommand ExitSystem { get; }
        public ICommand OpenDatabase { get; }
        public ICommand OpenDocumentation { get; }
        public ICommand ConfirmDatabase { get; }
        public ICommand GoToMainWindowFromCreateControl { get; }
        public ICommand GoToMainWindowFromOpenControl { get; }
        public ICommand SearchPlaceForDataBase { get; }
        public ICommand SearchDataBaseFileToOpen { get; }
        public ICommand OpenDuckDatabaseFile { get; }
        public string? DatabaseName { get; set; }

        public string SearchPlaceForDataBaseButtonLabel { get; set; } = "...путь для сохранения файла базы данных";
        public string SearchDatabaseFileToOpenButtonLabel { get; set; } = "...путь где находится файл базы данных";

        public bool CreateDatabaseControlVisibility { get; set; }
        public bool OpenDatabaseControlVisibility { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly string? FolderName = null;

        public MainWindowViewModel()
        {
            CreateDatabase = new RelayCommand(Click => CreateDB());
            OpenDatabase = new RelayCommand(Click => OpenDB());
            OpenDocumentation = new RelayCommand(Click => Documentation());
            ExitSystem = new RelayCommand(Click => System.Environment.Exit(0));
            ConfirmDatabase = new RelayCommand(Click => ConfirmDatabaseName(FolderName, DatabaseName));
            GoToMainWindowFromCreateControl = new RelayCommand(Click => GoBackToMainWindowFromCreateControl());
            GoToMainWindowFromOpenControl = new RelayCommand(Click => GoBackToMainWindowFromOpenControl());
            SearchPlaceForDataBase = new RelayCommand(Click => BrousePlaceForDataBase());
            SearchDataBaseFileToOpen = new RelayCommand(Click => BrouseDataBaseFileToOpen());
            OpenDuckDatabaseFile = new RelayCommand(Click => OpenDuckDBFile());
        }
        private void CreateDB()
        {
            CreateDatabaseControlVisibility = true;
            OnPropertyChange(nameof(CreateDatabaseControlVisibility));
        }
        private void OpenDB()
        {
            OpenDatabaseControlVisibility = true;
            OnPropertyChange(nameof(OpenDatabaseControlVisibility));
        }
        private void Documentation()
        {
            Process.Start(new ProcessStartInfo {
            FileName = ".\\Documentation.pdf",
            UseShellExecute = true });
        }
        private void ConfirmDatabaseName(string? folderName, string? dataBaseName) // creating database file and open
        {
           // _duckdb = DuckDB(false, folderName);
            MessageBox.Show($"ConfirmDatabaseName: {folderName}+{dataBaseName}");
        }
        private void OpenDuckDBFile() // opening database file 
        {
            MessageBox.Show("OpenDuckDBFile");
        }
        private void GoBackToMainWindowFromCreateControl()
        {
            CreateDatabaseControlVisibility = false;
            OnPropertyChange(nameof(CreateDatabaseControlVisibility));
        }
        private void GoBackToMainWindowFromOpenControl()
        {
            OpenDatabaseControlVisibility = false;
            OnPropertyChange(nameof(OpenDatabaseControlVisibility));
        }
        private void BrousePlaceForDataBase()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            dialog.Title = "Confirm place for saving your database";
            if (dialog.ShowDialog() == true)
            {
                SearchPlaceForDataBaseButtonLabel = $"{dialog.FolderName}"; //showing picking folder name for saving database CreateDatabaseControl
                OnPropertyChange(nameof(SearchPlaceForDataBaseButtonLabel));
                ConfirmDatabaseName(dialog.FolderName, DatabaseName);
            }
            else { MessageBox.Show("Error saving file directory"); }
        }
        private void BrouseDataBaseFileToOpen()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Find your database file";
            dialog.Filter = "Database files (*.duckdb) | *.duckdb";
            if (dialog.ShowDialog() == true)
            {
                SearchDatabaseFileToOpenButtonLabel = $"{dialog.FileName}"; //showing picking database file in OpenDatabaseControl
                OnPropertyChange(nameof(SearchDatabaseFileToOpenButtonLabel));
            }
            else { MessageBox.Show("Error picking database file"); }
        }
        protected void OnPropertyChange(string TrueOrFalse)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(TrueOrFalse));
        }

    }
}   