using System;
using System.Windows.Controls;
using CRM.ViewModels;

namespace CRM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ExportImportControl.xaml
    /// </summary>
    public partial class ExportImportControl : UserControl
    {
        public ExportImportControl()
        {
            InitializeComponent();
            DataContext = new ExportImportViewModel();
        }
    }
}
