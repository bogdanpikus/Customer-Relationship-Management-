using CRM.Services;
using CRM.ViewModels;
using System;
using System.Windows.Controls;

namespace CRM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для OrderPageControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        public OrderControl()
        {
            InitializeComponent();
            DataContext = new OrderControlViewModel();
        }
    }
}
