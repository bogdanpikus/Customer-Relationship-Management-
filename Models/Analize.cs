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
                if (_month != value)
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
                if (_sumOfPriceByMonth != value)
                {
                    _sumOfPriceByMonth = value;
                    OnPropertyChange(nameof(SumOfPriceByMonth));
                }
            }
        }
    }

    public class RangeWithOrders :NotifyPropertyChange
    {
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
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
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChange(nameof(Count));
                }
            }
        }
    }

    public class SourseCount : NotifyPropertyChange
    {
        private string? _sourse;
        public string? Sourse
        {
            get => _sourse;
            set
            {
                if (_sourse != value)
                {
                    _sourse = value;
                    OnPropertyChange(nameof(Sourse));
                }
            }
        }
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChange(nameof(Count));
                }
            }
        }
    }

    public class TodayAnalizeData : NotifyPropertyChange
    {
        private int? _todayOrdersCount;
        public int? TodayOrdersCount
        {
            get => _todayOrdersCount;
            set
            {
                if (_todayOrdersCount != value)
                {
                    _todayOrdersCount = value;
                    OnPropertyChange(nameof(TodayOrdersCount));
                }
            }
        }
        private decimal? _todayIncome;
        public decimal? TodayIncome
        {
            get => _todayIncome;
            set
            {
                if (_todayIncome != value)
                {
                    _todayIncome = value;
                    OnPropertyChange(nameof(TodayIncome));
                }
            }
        }
    }

    public class MonthAnalizeData : NotifyPropertyChange
    {
        private int? _monthOrdersCount;
        public int? MonthOrderCount
        {
            get => _monthOrdersCount;
            set
            {
                if(_monthOrdersCount != value)
                {
                    _monthOrdersCount = value;
                    OnPropertyChange(nameof(MonthOrderCount));
                }
            }
        }
        private int? _monthOrderRejections;
        public int? MonthOrderRejections
        {
            get => _monthOrderRejections;
            set
            {
                if(_monthOrderRejections != value)
                {
                    _monthOrderRejections = value;
                    OnPropertyChange(nameof(MonthOrderRejections));
                }
            }
        }
        private decimal? _monthIncome;
        public decimal? MonthIncome
        {
            get => _monthIncome;
            set
            {
                if(_monthIncome != value)
                {
                    _monthIncome = value;
                    OnPropertyChange(nameof(MonthIncome));
                }
            }
        }
    }
}
