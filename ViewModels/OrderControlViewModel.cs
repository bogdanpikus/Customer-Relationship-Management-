using System;
using CRM.Services;
using CRM.Commands;
using System.Windows.Input;
using CRM.Models;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace CRM.ViewModels
{
    public class OrderControlViewModel : NotifyPropertyChange
    {
        private readonly DuckDatabase _db = DatabaseFactory.Instance;

        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();
        public object? CurrentView { get; set; }
        public bool IsSelected { get; set; }

        public ICommand Exit {  get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand EditingCommand { get; set; }
        public ICommand DeleteOrder {  get; set; }

        public OrderControlViewModel()
        {   
            Exit = new RelayCommand(Click => System.Environment.Exit(0));
            AddOrderCommand = new RelayCommand(Click => OpenOrderDialog());
            EditingCommand = new RelayCommand(Click => OpenEditingDialog());
            DeleteOrder = new RelayCommand(Click => DeleteOrderFromTableAndDatabase());
            LoadOrdersFromDatabase();
        }
        private void LoadOrdersFromDatabase()
        {
             _db.ExtractOrdersFromDatabase(Orders);
        }
        private void OpenOrderDialog()
        {
            DialogService.Instance.ShowDialog(new OrderModalViewModel(Orders));
        }
        private void OpenEditingDialog()
        {
            var selected = Orders.Where(order => order.IsSelected).ToList();
            foreach (var order in selected)
            {
                DialogService.Instance.ShowDialog(new EditingViewModal(order));
            }
        }
        private void DeleteOrderFromTableAndDatabase()
        {
            _db.DeleteOrderInDatabase(Orders);
        }
    }
}
