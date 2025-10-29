using CRM.Interfaces;
using CRM.ViewModels;
using CRM.ViewModels.ModalWindowViewModels;
using CRM.Views.ModalControls;
using CRM.Views.ModalWindows;
using System.Windows;
using System.Windows.Interop;

namespace CRM.Services
{
    public class DialogService : IDialogInterface
    {
        private static DialogService? _instance;
        private Window? _activeWindow;
        public static DialogService Instance => _instance ?? (_instance = new DialogService());
        private readonly Dictionary<Type, Type> _mapping = new();

        public DialogService()
        {
            _mapping[typeof(OrderModalViewModel)] = typeof(OrderModalWindow);
            _mapping[typeof(EditingViewModal)] = typeof(EditingModalWindow);
            _mapping[typeof(CustomerAddViewModel)] = typeof(CustomerAddWindow);
        }
        public bool ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            if (!_mapping.TryGetValue(typeof(TViewModel), out var viewType))
                throw new InvalidOperationException($"Нет View для {typeof(TViewModel).Name}");

                var window = (Window)Activator.CreateInstance(viewType)!;
                _activeWindow = window;
                window.DataContext = viewModel;

                return window.ShowDialog() ?? false;
        }
        public void CloseDialog()
        {
            if(_activeWindow != null)
            {
                _activeWindow.Close();
                _activeWindow = null;
            }
        }
    }
}
