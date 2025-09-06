using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Commands;
using CRM.Services;

namespace CRM.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChange
    {
        public object? CurrentViewModel { get; set; }

        public ICommand CreateDatabase { get; }
        public ICommand OpenDatabase { get; }
        public ICommand ExitSystem { get; }
        public ICommand OpenDocumentation { get; }

        public MainWindowViewModel()
        {
            CreateDatabase = new RelayCommand(Click => CreateDB());
            OpenDatabase = new RelayCommand(Click => OpenDB());
            OpenDocumentation = new RelayCommand(Click => Documentation());
            ExitSystem = new RelayCommand(Click => System.Environment.Exit(0));
        }
        private void CreateDB()
        {
            CurrentViewModel = new CreateDatabaseViewModel();
            OnPropertyChange(nameof(CurrentViewModel));
        }
        private void OpenDB()
        {
            CurrentViewModel = new OpenDatabaseViewModel();
            OnPropertyChange(nameof(CurrentViewModel));
        }
        private void Documentation()
        {
            Process.Start(new ProcessStartInfo {
            FileName = ".\\Documentation.pdf",
            UseShellExecute = true });
        }
    }
}   