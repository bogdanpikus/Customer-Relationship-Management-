using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Commands;

namespace CRM.ViewModels
{
    public class MainWindowViewModel
    {
        public ICommand CreateDatabase { get; }
        public ICommand ExitSystem { get; }
        public ICommand OpenDatabase { get; }
        public ICommand OpenDocumentation { get; }
        public ICommand ConfirmDatabase { get; }
        public string? DatabaseName { get; set; }

        public MainWindowViewModel()
        {
            CreateDatabase = new RelayCommand(Click => CreateDB());
            ExitSystem = new RelayCommand(Click => System.Environment.Exit(0));
            OpenDatabase = new RelayCommand(Click => OpenDB());
            OpenDocumentation = new RelayCommand(Click => Documentation());
            ConfirmDatabase = new RelayCommand(Click => ConfirmDatabaseName());
        }
        private void CreateDB()
        {
            MessageBox.Show("RelayCommand MessageBox");
        }
        private void OpenDB()
        {
            MessageBox.Show("OpenDatabase MessageBox");
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
    }
}   