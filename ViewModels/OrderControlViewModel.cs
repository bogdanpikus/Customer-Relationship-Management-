using System;
using CRM.Services;
using CRM.Commands;
using System.Windows.Input;
using System.Printing;
using CRM.Views.ModalControls;
using CRM.Interfaces;

namespace CRM.ViewModels
{
    public class OrderControlViewModel : NotifyPropertyChange
    {
        private DialogService? service;
        public object? CurrentView { get; set; }

        public ICommand Exit {  get; set; }
        public ICommand AddOrderCommand { get; set; }

        public OrderControlViewModel()
        {   
            Exit = new RelayCommand(Click => System.Environment.Exit(0));
            AddOrderCommand = new RelayCommand(Click => OpenOrderDialog());
        }
        private void OpenOrderDialog()
        {
            service = new DialogService();
            service.ShowDialog();
        }
    }
}
