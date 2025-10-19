using CRM.Services;
using System;

namespace CRM.Models
{
    public class PriceByMonth : NotifyPropertyChange
    {
        private string _month;
        public string Month
        {
            get => _month;
            set
            {
                if(_month != value)
                {
                    _month = value;
                    OnPropertyChange(nameof(Month));
                }
            }
        }
        private double _sumOfPriceByMonth;
        public double SumOfPriceByMonth
        {
            get => _sumOfPriceByMonth;
            set
            {
                if(_sumOfPriceByMonth != value)
                {
                    _sumOfPriceByMonth = value;
                    OnPropertyChange(nameof(SumOfPriceByMonth));
                }
            }
        }
    }
}
