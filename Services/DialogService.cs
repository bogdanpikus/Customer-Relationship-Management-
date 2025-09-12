using CRM.Interfaces;
using CRM.Views.ModalControls;
using System;
using System.Windows;
using System.Windows.Automation.Peers;

namespace CRM.Services
{
    public class DialogService : IDialogInterface
    {
        public bool? ShowDialog()
        {
            OrderModalWindow window = new();
            return window.ShowDialog();
        }
    }
}
