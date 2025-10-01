using System;
using CRM.Services;
using CRM.Commands;
using System.Windows.Input;
using CRM.Models;
using System.Collections.ObjectModel;

namespace CRM.ViewModels
{
    public class OrderControlViewModel : NotifyPropertyChange
    {
        private readonly DuckDatabase _db = DatabaseFactory.Instance;

        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();
        public object? CurrentView { get; set; }
        public bool ControlsVisiability { get; set; }
        public bool IsSelected { get; set; }

        public ICommand Exit {  get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand EditingCommand { get; set; }
        public ICommand DeleteOrder {  get; set; }
        public ICommand OrderPage {  get; set; }
        public ICommand AnalizePage { get ; set; }

        public int AllSpendings { get; set; } = 0;
        public int AllChashFlow { get; set; } = 0;
        public int AllIncome { get; set; } = 0;

        public OrderControlViewModel()
        {   
            Exit = new RelayCommand(Click => System.Environment.Exit(0));
            AddOrderCommand = new RelayCommand(Click => OpenOrderDialog());
            EditingCommand = new RelayCommand(Click => OpenEditingDialog());
            DeleteOrder = new RelayCommand(Click => DeleteOrderFromTableAndDatabase());
            OrderPage = new RelayCommand(Click => OpenOrderPage());
            AnalizePage = new RelayCommand(Click => OpenAnalizePage());
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
        private void OpenOrderPage()
        {
            ControlsVisiability = false;
            OnPropertyChange(nameof(ControlsVisiability));
        }
        private void OpenAnalizePage()
        {
            CurrentView = new AnalizeViewModel();
            ControlsVisiability = true;
            OnPropertyChange(nameof(ControlsVisiability));
            OnPropertyChange(nameof(CurrentView));
        }

    }
}
