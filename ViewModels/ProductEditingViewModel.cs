using CRM.Commands;
using CRM.Models;
using CRM.Services;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class ProductEditingViewModel : NotifyPropertyChange
    {
        private Products _product;
        private readonly SQLService _sqlService = new();
        public bool GridVisiability { get; set; }

        public ICommand GoBackToProducts { get; }
        public ICommand ConfirmProductUpdate { get; } // подтверждение создания продукта

        public bool PhotoVisisability { get; set; } // видимость загруменных фото

        public string? Articul { get; set; }
        public string? PhotoPath { get; set; } // путь загруженной фото  для товара
        public string? ProductName { get; set; }
        public string? Color { get; set; }
        public decimal? Price { get; set; }
        public decimal? PrimePrice { get; set; } // себестоимость
        public string? Measurement { get; set; } // единица измерения (кг, набор, граммы)
        public int? Amount { get; set; } // колличество на складе
        public string? Supplier { get; set; } // поставщик, произвоитель товара
        public string? Category { get; set; } // категория товара
        public string? Status { get; set; } // статус заказа
        public string? Comment { get; set; } // комментарий к заказу
        public Dictionary<string, string> Attributes { get; set; } //JSON? (не реализован)
        public string? ActivityLabel { get; set; } // Активный-Архив-Неактивный
        public string? StatusLabel { get; set; } // Хит-продаж
        public decimal? Commission { get; set; }
        public string? Description { get; set; } // Описание товара

        public int? PriceForAmount { get; set; } // Price за 1 штук(у) может DICTIONARY?
        public int? PrimeForAmount { get; set; } // PrimePrice за 1 штук(у) может DICTIONARY?
        public int? OptAmount { get; set; }
        public int? PercentOptDiscount { get; set; }

        public ProductEditingViewModel(Products product) 
        {
            GridVisiability = true;
            _product = product;

            Articul = _product.Articul;
            PhotoPath = _product.PhotoPath;
            ProductName = _product.ProductName;
            Color = _product.Color;
            Price = _product.Price;
            PrimePrice = _product.PrimePrice;
            Measurement = _product.Measurement;
            Amount = _product.Amount;
            Supplier = _product.Supplier;
            Category = _product.Category;
            Status = _product.Status;
            Comment = _product.Comment;
            ActivityLabel = _product.ActivityLabel;
            StatusLabel = _product.StatusLabel;
            Commission = _product.Commission;
            Description = _product.Description;

            GoBackToProducts = new RelayCommand(Click => GoBackToProductsAction());
            ConfirmProductUpdate = new RelayCommand(Click => ConfirmProductUpdateAction());
        }
        private void GoBackToProductsAction()
        {
            GridVisiability = false;
            OnPropertyChange(nameof(GridVisiability));
        }
        private void ConfirmProductUpdateAction()
        {
            _product.Articul = Articul;
            _product.PhotoPath = PhotoPath;
            _product.ProductName = ProductName;
            _product.Color = Color;
            _product.Price = Price;
            _product.PrimePrice = PrimePrice;
            _product.Measurement = Measurement;
            _product.Amount = Amount;
            _product.Supplier = Supplier;
            _product.Category = Category;
            _product.Status = Status;
            _product.Comment = Comment;
            _product.ActivityLabel = ActivityLabel;
            _product.StatusLabel = StatusLabel;
            _product.Commission = Commission;
            _product.Description = Description;

            if (_sqlService.UpdateProductAction(_product))
            {
                GridVisiability = false;
                OnPropertyChange(nameof(GridVisiability));  
            }
            else
            {
                MessageBox.Show($"ERROR {_product.ProductName} SQL UPDATE");
            }
        }
    }
}
