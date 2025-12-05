using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class StorageProductViewModel : NotifyPropertyChange
    {
        private SQLService _sqlServise = new();
        public ObservableCollection<Products> ProductCollection { get; set; } = new();
        public bool CurrentVisiability { get; set; }

        public ICommand GoBackToGroups { get; }
        public ICommand CreateProduct { get; }
        public ICommand Editing { get; }
        public ICommand Delete { get; }

        public bool ContentControlVisiability { get; set; }
        public object ProductContentControl {  get; set; }
        
        private int _groupId { get; set; }

        public StorageProductViewModel(int id)
        {
            _groupId = id;

            CurrentVisiability = true;
            ContentControlVisiability = false;

            GoBackToGroups = new RelayCommand(Click => GoBackToGroupsAction());
            CreateProduct = new RelayCommand(Click => CreateProductAction(_groupId));
            Editing = new RelayCommand(Click => EditingProductAction());
            Delete = new RelayCommand(Click => DeleteProductAction());
            Load(_groupId);
        }
        private void Load(int id)
        {
            ProductCollection.Clear();
            // Загрузка товаров с FOREIGN KEY (ProductId) REFERENCES productGroup(Id)
            var products = _sqlServise.SelectProducts(id);
            foreach(var product in products)
            {
                ProductCollection.Add(product);
                Debug.WriteLine($"{product.ProductName} | Hash: {product.GetHashCode()} | Selected: {product.IsSelected}");
            }
        }
        private void GoBackToGroupsAction()
        {
            CurrentVisiability = false;
            OnPropertyChange(nameof(CurrentVisiability));
        }
        private void CreateProductAction(int id)
        {
            ContentControlVisiability = true;
            ProductContentControl = new ProductCreateViewModel(ProductCollection, id);
            OnPropertyChange(nameof(ProductContentControl));
            OnPropertyChange(nameof(ContentControlVisiability));
        }
        private void EditingProductAction()
        {
            var isSelected = ProductCollection.Where(o => o.IsSelected).ToList();
            foreach(var selected in isSelected)
            {
                ContentControlVisiability = true;
                ProductContentControl = new ProductEditingViewModel(selected);
                OnPropertyChange(nameof(ProductContentControl));
                OnPropertyChange(nameof(ContentControlVisiability));
            }
        }
        private void DeleteProductAction()
        {
            var isSelected = ProductCollection.Where(o => o.IsSelected).ToList();
            foreach (var selected in isSelected)
            {
               if (_sqlServise.DeleteProductAction(selected.Id))
                {
                    ProductCollection.Remove(selected);
                }
                else
                {
                    MessageBox.Show($"DELETING {selected.ProductName} SQL ERRROR");
                }

            }
        }
    }
}
