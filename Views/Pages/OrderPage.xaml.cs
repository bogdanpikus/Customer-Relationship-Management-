using System;
using System.Windows.Controls;

namespace CRM.ViewModels.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
            DataContext = new OrderPageViewModel();
        }
    }
}
