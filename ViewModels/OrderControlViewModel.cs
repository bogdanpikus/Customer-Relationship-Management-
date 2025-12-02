using System;
using CRM.Services;
using CRM.Commands;
using System.Windows.Input;
using CRM.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics;

namespace CRM.ViewModels
{
    public class OrderControlViewModel : NotifyPropertyChange
    {
        private readonly SQLService _sqlService = new SQLService();

        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();
        public object? CurrentView { get; set; }
        public bool ControlsVisiability { get; set; }
        public bool IsSelected { get; set; }

        public ICommand AddOrderCommand { get; set; }
        public ICommand EditingCommand { get; set; }
        public ICommand DeleteOrder {  get; set; }
       
        public ICommand OrderPage {  get; set; }
        public ICommand AnalizePage { get ; set; }
        public ICommand Contacts {  get; set; }
        public ICommand Storage { get; set; }
        public ICommand Documents { get; set; }
        public ICommand ExportImport { get; set; }
        public ICommand Settings { get; set; }
        public ICommand Exit { get; set; }

        //---------------------ToolBar------------------
        public ICommand CreateNewDatabase {  get; set; }
        public ICommand OpenExistDatabase {  get; set; }
        public ICommand MakeACopyOfDatabase {  get; set; }
        //----------------------------------------------

        private decimal? _allSpendings;
        public decimal? AllSpendings
        {
            get => _allSpendings;
            set
            {
                if(_allSpendings != value)
                {
                    _allSpendings = value;
                    OnPropertyChange(nameof(AllSpendings));
                }
            }
        }
        private decimal? _allChachFlow;
        public decimal? AllChashFlow
        {
            get => _allChachFlow;
            set
            {
                if(_allChachFlow != value)
                {
                    _allChachFlow = value;
                    OnPropertyChange(nameof(AllChashFlow));
                }
            }
        }
        private decimal? _allIncome;
        public decimal? AllIncome
        {
            get => _allIncome;
            set
            {
                if(_allIncome != value)
                {
                    _allIncome = value;
                    OnPropertyChange(nameof(AllIncome));
                }
            }
        }

        public OrderControlViewModel()
        {
            //---------------------ToolBar------------------
            CreateNewDatabase = new RelayCommand(Click => CreateNewDatabaseAction());
            OpenExistDatabase = new RelayCommand(Click => OpenExistDatabaseAction());
            MakeACopyOfDatabase = new RelayCommand(Click => MakeACopyOfDatabaseAction());
            //----------------------------------------------
            AddOrderCommand = new RelayCommand(Click => OpenOrderDialog());
            EditingCommand = new RelayCommand(Click => OpenEditingDialog());
            DeleteOrder = new RelayCommand(Click => DeleteOrderFromTableAndDatabase());

            Exit = new RelayCommand(Click => System.Environment.Exit(0));
            OrderPage = new RelayCommand(Click => OpenOrderPage());
            AnalizePage = new RelayCommand(Click => OpenAnalizePage());
            Contacts = new RelayCommand(Click => OpenContacsPage());
            Storage = new RelayCommand(Click => OpenStoragePage());
            Documents = new RelayCommand(Click => OpenDocumentPage());
            ExportImport = new RelayCommand(Click => OpenExportImportPage());
            Settings = new RelayCommand(Click => OpenSettingsPage());
            LoadOrdersFromDatabase();
            NumberOfOrders();
            Sum();
        }
        private void NumberOfOrders()
        {
            // считаються номера заказов
            var count = Orders.Count;
            foreach (var order in Orders)
            {
                order.PersonalNumber = count--;
            }
        }
        private void Sum()
        {
            AllIncome = Orders.Sum(o => o.Income);
            AllChashFlow = Orders.Sum(o => o.Price);
            AllSpendings = Orders.Sum(o => o.Spending);

            var canceled = Orders.Where(c => c.Status == "Отмененный");
            foreach (var order in canceled)
            {
                AllIncome -= order.Income;
                AllChashFlow -= order.Price;
                AllSpendings -= order.Spending;
            }

        }
        private void LoadOrdersFromDatabase()
        {
            Orders.Clear();
            var ordersFromDB = _sqlService.LoadOrdersFromDatabase();
            foreach(var order in ordersFromDB)
            {
                Orders.Insert(0,order);
            }
        }
        private void OpenOrderDialog()
        {
            DialogService.Instance.ShowDialog(new OrderModalViewModel(Orders));
            Sum();
        }
        private void OpenEditingDialog()
        {
            var selected = Orders.Where(order => order.IsSelected);
            foreach (var order in selected)
            {
                DialogService.Instance.ShowDialog(new EditingViewModal(order));
            }

            Sum();
        }
        private void DeleteOrderFromTableAndDatabase()
        {
            var selectedOrder = Orders.Where(o => o.IsSelected).ToList();
            foreach (var order in selectedOrder)
            {
                _sqlService.DeleteOrderInDatabase(order.Id); // возвращает true, если удалено
                Orders.Remove(order);
            }

            NumberOfOrders();
            Sum();
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
        private void OpenContacsPage()
        {
            CurrentView = new ContactsViewModel();
            ControlsVisiability = true;
            OnPropertyChange(nameof(ControlsVisiability));
            OnPropertyChange(nameof(CurrentView));
        }
        private void OpenStoragePage()
        {
            CurrentView = new StorageViewModel();
            ControlsVisiability = true;
            OnPropertyChange(nameof(ControlsVisiability));
            OnPropertyChange(nameof(CurrentView));
        }
        private void OpenDocumentPage()
        {
            CurrentView = new DocumentsViewModel();
            ControlsVisiability = true;
            OnPropertyChange(nameof(ControlsVisiability));
            OnPropertyChange(nameof(CurrentView));
        }
        private void OpenExportImportPage()
        {
            CurrentView = new ExportImportViewModel();
            ControlsVisiability = true;
            OnPropertyChange(nameof(ControlsVisiability));
            OnPropertyChange(nameof(CurrentView));
        }
        private void OpenSettingsPage()
        {
            CurrentView = new SettingsViewModel();
            ControlsVisiability = true;
            OnPropertyChange(nameof(ControlsVisiability));
            OnPropertyChange(nameof(CurrentView));
        }
        private void CreateNewDatabaseAction()
        {
            MessageBox.Show("CreateNewDatabaseAction");
        }
        private void OpenExistDatabaseAction()
        {
            MessageBox.Show("OpenExistDatabaseAction");
        }
        private void MakeACopyOfDatabaseAction()
        {
            MessageBox.Show("MakeACopyOfDatabaseAction");
        }
    }
}
