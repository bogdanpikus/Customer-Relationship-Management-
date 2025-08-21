using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Commands;
using CRM.Templates;

namespace CRM.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand CreateDatabase { get; }
        public ICommand ExitSystem { get; }
        public ICommand OpenDatabase { get; }
        public ICommand OpenDocumentation { get; }
        public ICommand ConfirmDatabase { get; }
        public ICommand GoToMainWindowFromCreateControl { get; }
        public ICommand GoToMainWindowFromOpenControl { get; }
        public string? DatabaseName { get; set; }

        public bool CreateDatabaseControlVisibility { get; set; }
        public bool OpenDatabaseControlVisibility { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            CreateDatabase = new RelayCommand(Click => CreateDB());
            OpenDatabase = new RelayCommand(Click => OpenDB());
            OpenDocumentation = new RelayCommand(Click => Documentation());
            ExitSystem = new RelayCommand(Click => System.Environment.Exit(0));
            ConfirmDatabase = new RelayCommand(Click => ConfirmDatabaseName());
            GoToMainWindowFromCreateControl = new RelayCommand(Click => GoBackToMainWindowFromCreateControl());
            GoToMainWindowFromOpenControl = new RelayCommand(Click => GoBackToMainWindowFromOpenControl());

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
        private void ConfirmDatabaseName()
        {
            if (!string.IsNullOrEmpty(DatabaseName))
            {
                MessageBox.Show("Yee");
            }

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

        protected void OnPropertyChange(string TrueOrFalse)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(TrueOrFalse));
        }

    }
}   