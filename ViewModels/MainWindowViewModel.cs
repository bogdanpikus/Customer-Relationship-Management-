using System;
using System.Collections.Generic;
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
    public class MainWindowViewModel
    {
        public ICommand CreateDatabase { get; }
        public ICommand ExitSystem { get; }
        public ICommand OpenDatabase { get; }
        public ICommand OpenDocumentation { get; }
        public ICommand ConfirmDatabase { get; }
        public string? DatabaseName { get; }

        public bool CreateDatabaseControlVisibility { get; set; }
        public bool OpenDatabaseControlVisibility { get; set; }

        public MainWindowViewModel()
        {
            CreateDatabase = new RelayCommand(Click => CreateDB());
            OpenDatabase = new RelayCommand(Click => OpenDB());
            OpenDocumentation = new RelayCommand(Click => Documentation());
            ExitSystem = new RelayCommand(Click => System.Environment.Exit(0));
            ConfirmDatabase = new RelayCommand(Click => ConfirmDatabaseName());

        }
        private void CreateDB()
        {
        CreateDatabaseControlVisibility = true;
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