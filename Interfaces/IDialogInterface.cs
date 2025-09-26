using System;
using System.Windows;

namespace CRM.Interfaces
{
    public interface IDialogInterface
    {
        bool ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class;
    }
}
