using CRM.Services;

namespace CRM.Models
{
    public class Storages : NotifyPropertyChange
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }

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
        public bool isSelected { get; set; }
        public int StorageId { get; set; }

        private string _name;
        public string Name 
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
    }

    public class Products : NotifyPropertyChange
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public int ProductId { get; set; }

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

        private string? _photoPath;
        public string? PhotoPath // путь для отображения фото
        {
            get => _photoPath;
            set
            {
                if(_photoPath != value)
                {
                    _photoPath = value;
                    OnPropertyChange(nameof(PhotoPath));
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
    }
}
