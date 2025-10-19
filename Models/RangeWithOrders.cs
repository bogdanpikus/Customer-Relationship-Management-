using CRM.Services;
using System;

namespace CRM.Models
{
    public class RangeWithOrders : NotifyPropertyChange
    {
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                if(_date != value)
                {
                    _date = value;
                    OnPropertyChange(nameof(Date));
                }
            }
        }
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                if(_count != value)
                {
                    _count = value;
                    OnPropertyChange(nameof(Count));
                }
            }
        }
    }
}
