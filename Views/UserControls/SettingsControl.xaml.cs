using System;
using System.Windows.Controls;
using CRM.ViewModels;

namespace CRM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}
