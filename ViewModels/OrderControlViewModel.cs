using System;
using CRM.Services;
using CRM.Commands;
using System.Windows.Input;
using CRM.Models;
using System.Windows;
using System.Collections.ObjectModel;

namespace CRM.ViewModels
{
    public class OrderControlViewModel : NotifyPropertyChange
    {
        private readonly DuckDatabase _db = DatabaseFactory.Instance;

        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();
        public object? CurrentView { get; set; }

        public ICommand Exit {  get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand EditingCommand { get; set; }

        public OrderControlViewModel()
        {   
            Exit = new RelayCommand(Click => System.Environment.Exit(0));
            AddOrderCommand = new RelayCommand(Click => OpenOrderDialog());
            EditingCommand = new RelayCommand(Click => OpenEditingDialog());
            LoadOrdersFromDatabase();
        }
        private void LoadOrdersFromDatabase()
        {
             _db.ExtractOrdersFromDatabase(Orders);
        }
        private void OpenOrderDialog()
        {
            DialogService.Instance.ShowDialog(new OrderModalViewModel());
        }
        private void OpenEditingDialog()
        {
            DialogService.Instance.ShowDialog(new EditingViewModal());
        }
    }
}
