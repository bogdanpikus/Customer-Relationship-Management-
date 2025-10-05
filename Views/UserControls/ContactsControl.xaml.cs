using System;
using System.Windows.Controls;
using CRM.ViewModels;

namespace CRM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ContactsControl.xaml
    /// </summary>
    public partial class ContactsControl : UserControl
    {
        public ContactsControl()
        {
            InitializeComponent();
            DataContext = new ContactsViewModel();
        }
    }
}
