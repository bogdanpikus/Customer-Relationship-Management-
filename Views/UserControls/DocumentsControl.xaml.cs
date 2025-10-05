using System;
using System.Windows.Controls;
using CRM.ViewModels;

namespace CRM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для DocumentsControl.xaml
    /// </summary>
    public partial class DocumentsControl : UserControl
    {
        public DocumentsControl()
        {
            InitializeComponent();
            DataContext = new DocumentsViewModel();
        }
    }
}
