using CRM.Commands;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class DocumentsViewModel
    {
        public ICommand AbcAnalizeCreate { get; }
        public ICommand SellsAnalizeInPeriod { get; }
        public ICommand CurrentYearAnalize { get; }
        public ICommand IncomeAnalizeInPeriod { get; }
        public ICommand CashflowAnalizeInPeriod { get; }
        public ICommand SpendingsAnalizeInPeriod { get; }
        public ICommand ProductsAnalize {  get; }
        public ICommand LastProductAnalize { get; }

        public DocumentsViewModel() 
        {
            AbcAnalizeCreate = new RelayCommand(Click => AbcAnalizeCreating());
            SellsAnalizeInPeriod = new RelayCommand(Click => SellsAnalizeInPeriodAction());
            CurrentYearAnalize = new RelayCommand(Click => CurrentYearAnalizing());
            IncomeAnalizeInPeriod = new RelayCommand(Click => IncomeAnalizeInPeriodAction());
            CashflowAnalizeInPeriod = new RelayCommand(Click => CashflowAnalizeInPeriodAction());
            SpendingsAnalizeInPeriod = new RelayCommand(Click => SpendingsAnalizeInPeriodAction());
            ProductsAnalize = new RelayCommand(Click => ProductsAnalizing());
            LastProductAnalize = new RelayCommand(Click => LastProductAnalizing());
        }
        private void AbcAnalizeCreating()
        {
            MessageBox.Show("AbcAnalizeCreating");
        }
        private void SellsAnalizeInPeriodAction()
        {
            MessageBox.Show("SellsAnalizeInPeriodAction");
        }
        private void CurrentYearAnalizing() 
        {
            MessageBox.Show("CurrentYearAnalizing");
        }
        private void IncomeAnalizeInPeriodAction()
        {
            MessageBox.Show("IncomeAnalizeInPeriodAction");
        }
        private void CashflowAnalizeInPeriodAction()
        {
            MessageBox.Show("CashflowAnalizeInPeriodAction");
        }
        private void SpendingsAnalizeInPeriodAction()
        {
            MessageBox.Show("SpendingsAnalizeInPeriodAction");
        }
        private void ProductsAnalizing()
        {
            MessageBox.Show("ProductsAnalizing");
        }
        private void LastProductAnalizing()
        {
            MessageBox.Show("LastProductAnalizing");
        }
    }
}
