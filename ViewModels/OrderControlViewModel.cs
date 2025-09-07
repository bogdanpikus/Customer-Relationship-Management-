using System;
using CRM.Services;
using CRM.Commands;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class OrderControlViewModel : NotifyPropertyChange 
    {
        public ICommand Exit {  get; set; }

        public OrderControlViewModel()
        {
            Exit = new RelayCommand(Click => System.Environment.Exit(0));
        }
    }
}
