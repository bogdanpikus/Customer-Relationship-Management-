using CRM.Services;
using System;

namespace CRM.Models
{
    public class Order : NotifyPropertyChange
    {
        /*
        public bool IsSelected {  get; set; }
        public DateTime OrderDate {  get; set; }
        public string? Articul { get; set; }
        public string? OrderID { get; set; }   
        public Customer Customer { get; set; }
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Item { get; set; }
        public int Amount { get; set; }
        public decimal? Price { get; set; } 
        public decimal? PrimeCost { get; set; }
        public string? PaymentWay { get; set; }
        public string? DelivarWay { get; set; }
        public string? DeliverAdress { get; set; }
        public string? Status { get; set; }
        public decimal? Spending { get; set; }
        public decimal? Income { get; set; }
        public string? Organization { get; set; }
        public string? Comment { get; set; }
        */

        public bool IsSelected { get; set; }
        private DateTime _orderDate;
        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                if (_orderDate != value) 
                {
                _orderDate = value;
                OnPropertyChange(nameof(OrderDate));
                }
            }
        }

        private string? _articul;
        public string? Articul
        {
            get => _articul;
            set
            {
                if (_articul != value) {
                _articul = value;
                OnPropertyChange(nameof(Articul));
                }
            }
        }

        private string? _orderID;
        public string? OrderID
        {
            get => _orderID;
            set
            {
                if(_orderID != value)
                {
                    _orderID = value;
                    OnPropertyChange(nameof(OrderID));
                }
            }
        }

        public Customer Customer { get; set; }

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

        private string? _secondName;
        public string? SecondName 
        {
            get => _secondName; 
            set 
            {
                if (_secondName != value) 
                { 
                    _secondName = value;
                    OnPropertyChange(nameof(SecondName)); 
                } 
            } 
        }

        private string? _surname;
        public string? Surname
        {
            get => _surname;
            set
            {
                if(_surname != value)
                {
                    _surname = value;
                    OnPropertyChange(nameof(Surname));
                }
            }
        }

        private string? _phone;
        public string? Phone
        {
            get => _phone;
            set
            {
                if(_phone != value)
                {
                    _phone = value;
                    OnPropertyChange(nameof(Phone));
                }
            }
        }

        private string? _item;
        public string? Item
        {
            get => _item;
            set
            {
                if(_item != value)
                {
                    _item = value;
                    OnPropertyChange(nameof(Item));
                }
            }
        }

        private int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChange(nameof(Amount));
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

        private decimal? _primeCost;
        public decimal? PrimeCost
        {
            get => _primeCost;
            set
            {
                if(_primeCost != value)
                {
                    _primeCost = value;
                    OnPropertyChange(nameof(PrimeCost));
                }
            }
        }

        private string? _paymentWay;
        public string? PaymentWay
        {
            get => _paymentWay;
            set
            {
                if(_paymentWay != value)
                {
                    _paymentWay = value;
                    OnPropertyChange(nameof(PaymentWay));
                }
            }
        }

        private string? _delivarWay;
        public string? DelivarWay
        {
            get => _delivarWay;
            set
            {
                if (_delivarWay != value)
                {
                    _delivarWay = value;
                    OnPropertyChange(nameof(DelivarWay));
                }
            }
        }

        private string? _deliverAdress;
        public string? DeliverAdress
        {
            get => _deliverAdress;
            set
            {
                if(_deliverAdress != value)
                {
                    _deliverAdress = value;
                    OnPropertyChange(nameof(DeliverAdress));
                }
            }
        }

        private string? _status;
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

        private decimal? _spending;
        public decimal? Spending
        {
            get => _spending;
            set
            {
                if(_spending != value)
                {
                    _spending = value;
                    OnPropertyChange(nameof(Spending));
                }
            }
        }

        private decimal? _income;
        public decimal? Income
        {
            get => _income;
            set
            {
                if(_income != value)
                {
                    _income = value;
                    OnPropertyChange(nameof(Income));
                }
            }
        }

        private string? _organization;
        public string? Organization
        {
            get => _organization;
            set
            {
                if(_organization != value)
                {
                    _organization = value;
                    OnPropertyChange(nameof(Organization));
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

    public class Customer
    {
        public string Id { get; set; }
        public string? SecondName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }

        // its variable
        public int AmountOrders { get; set; }
    }
}
