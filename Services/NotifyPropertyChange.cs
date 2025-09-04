using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class NotifyPropertyChange : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChange(string TrueOrFalse)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(TrueOrFalse));
        }
    }
}
