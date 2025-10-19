using CRM.Services;
using System;

namespace CRM.Models
{
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
}
