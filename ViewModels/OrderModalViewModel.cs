using CRM.Commands;
using CRM.Services;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class OrderModalViewModel : NotifyPropertyChange
    {
        public ICommand Confirm {  get; }

        public string? Date { get; set; }
        public string? Articul {  get; set; }
        public string? OrderID { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? Phone {  get; set; }
        public string? ItemName { get; set; }
        public string? AmountItem { get; set; }
        public decimal? Price { get; set; }
        public decimal? PrimeCost { get; set; }
        public string? PaymentWay { get; set; }
        public string? DeliverWay { get; set; }
        public string? DeliverAdress { get; set; }
        public string? Status { get; set; }
        public decimal? Spending { get; set; }
        public decimal? Income { get; set; }
        public string? Comment { get; set; }

        public OrderModalViewModel()
        {
            Confirm = new RelayCommand(Click => ConfirmOrder());
        }
     
        private void ConfirmOrder()
        {
            MessageBox.Show("Confirm Order Button");
        }
    }
}
