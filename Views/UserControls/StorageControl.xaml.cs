using CRM.ViewModels;
using System;
using System.Windows.Controls;

namespace CRM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StorageControl.xaml
    /// </summary>
    public partial class StorageControl : UserControl
    {
        public StorageControl()
        {
            InitializeComponent();
            DataContext = new StorageViewModel();
        }
    }
}
