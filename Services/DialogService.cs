using CRM.Interfaces;
using CRM.Views.ModalControls;
using System;
using System.Printing;
using System.Windows;
using System.Windows.Automation.Peers;

namespace CRM.Services
{
    public class DialogService : IDialogInterface
    {
        public bool? ShowDialog(bool True)
        {
            if (True)
            {
                OrderModalWindow window = new();
                return window.ShowDialog();
            }
            else
            {
                return false;
            }
        }
    }
}
