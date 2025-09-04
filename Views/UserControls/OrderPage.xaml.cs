using CRM.ViewModels;
using System;
using System.Windows.Controls;

namespace CRM.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для OrderPageControl.xaml
    /// </summary>
    public partial class OrderPage : UserControl
    {
        public OrderPage()
        {
            InitializeComponent();
            DataContext = new OrderPageViewModel();
        }
    }
}
