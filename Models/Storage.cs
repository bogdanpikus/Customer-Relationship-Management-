using CRM.Services;

namespace CRM.Models
{
    public class Storages : NotifyPropertyChange
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        private string? _storageName;
        public string? StorageName 
        {  
            get => _storageName;
            set
            {
                if (_storageName != value)
                {
                    _storageName = value;
                    OnPropertyChange(nameof(StorageName));
                }
            }
        }

        private string? _address;
        public string? Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChange(nameof(Address));
                }
            }
        }

        private string? _responsible;
        public string? Responsible
        {
            get => _responsible;
            set
            {
                if(_responsible != value)
                {
                    _responsible = value;
                    OnPropertyChange(nameof(Responsible));
                }
            }
        }

        private string? _phone;
        public string? Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChange(nameof(Phone));
                }
            }
        }

        private int? _amountGoods;
        public int? AmountGoodsInStorage
        {
            get => _amountGoods;
            set
            {
                if(_amountGoods != value)
                {
                    _amountGoods = value;
                    OnPropertyChange(nameof(AmountGoodsInStorage));
                }
            }
        }
    }

    public class ProductGroups : NotifyPropertyChange
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public int StorageId { get; set; }

        private string? _name;
        public string? Name 
        { 
            get => _name;
            set 
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChange(nameof(Name));
                }
            }
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set
            {
                if( _description != value)
                {
                    _description = value;
                    OnPropertyChange(nameof(Description));
                }
            }
        }

        private int? _amountGoodsInGroup;
        public int? AmountGoodsInGroup
        {
            get => _amountGoodsInGroup;
            set
            {
                if(_amountGoodsInGroup != value)
                {
                    _amountGoodsInGroup = value;
                    OnPropertyChange(nameof(AmountGoodsInGroup));
                }
            }
        }
    }

    public class Products : NotifyPropertyChange
    {
        public int Id { get; set; } // собственный id для базы данных
        public bool IsSelected { get; set; }
        public int ProductId { get; set; } // код товара для групп 

        public string? SKU { get; set; } // внутренний код товара

        private string? _articul;
        public string? Articul
        {
            get => _articul;
            set
            {
                if( _articul != value)
                {
                    _articul = value;
                    OnPropertyChange(nameof(Articul));
                }
            }
        }

        private string? _imagePath;
        public string? ImagePath // путь для отображения фото
        {
            get => _imagePath;
            set
            {
                if(_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChange(nameof(ImagePath));
                }
            }
        }

        private string? _name;
        public string? Name
        {
            get => _name;
            set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChange(nameof(Name));
                }
            }
        }

        private string? _color;
        public string? Color
        {
            get => _color;
            set
            {
                if(_color != value)
                {
                    _color = value;
                    OnPropertyChange(nameof(Color));
                }
            }
        }

        private decimal? _price;
        public decimal? Price
        {
            get => _price;
            set
            {
                if(_price != value)
                {
                    _price = value;
                    OnPropertyChange(nameof(Price));
                }
            }
        }

        private decimal? _primaryPrice;
        public decimal? PrimaryPrice
        {
            get => _primaryPrice;
            set
            {
                if(_primaryPrice != value)
                {
                    _primaryPrice = value;
                    OnPropertyChange(nameof(PrimaryPrice));
                }
            }
        }

        private decimal? _incomeFromSelling;
        public decimal? IncomeFromSelling
        {
            get => _incomeFromSelling;
            set
            {
                if(_incomeFromSelling != value)
                {
                    _incomeFromSelling = value;
                    OnPropertyChange(nameof(IncomeFromSelling));
                }
            }
        }

        private string? _measurement; // единица измерения
        public string? Measurement
        {
            get => _measurement;
            set
            {
                if(_measurement != value)
                {
                    _measurement = value;
                    OnPropertyChange(nameof(Measurement));
                }
            }
        }

        private int? _amount;
        public int? Amount
        {
            get => _amount;
            set
            {
                if(_amount != value)
                {
                    _amount = value;
                    OnPropertyChange(nameof(Amount));
                }
            }
        }

        private string? _supplier; // поставщик/производитель товара
        public string? Supplier
        {
            get => _supplier;
            set
            {
                if(_supplier != value)
                {
                    _supplier = value;
                    OnPropertyChange(nameof(Supplier));
                }
            }
        }

        private string? _category;
        public string Category
        {
            get => _category;
            set
            {
                if(_category != value)
                {
                    _category = value;
                    OnPropertyChange(nameof(Category));
                }
            }
        }

        private string? _status; // Статус товара (в продаже, архив, предзаказ)
        public string? Status
        {
            get => _status;
            set
            {
                if(_status != value)
                {
                    _status = value;
                    OnPropertyChange(nameof(Status));
                }
            }
        }

        private string? _comment;
        public string? Comment
        {
            get => _comment;
            set
            {
                if(_comment != value)
                {
                    _comment = value;
                    OnPropertyChange(nameof(Comment));
                }
            }
        }

        public string? Barcode { get; set; } //Штрих-код в формате EAN/UPC.
        public Dictionary<string, string>? Attributes { get; set; } // Дополнительные характеристики
    }
}
