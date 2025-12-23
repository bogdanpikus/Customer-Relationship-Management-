using CRM.Commands;
using CRM.Properties;
using CRM.Services;
using CRM.Themes;
using System.Windows;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class SettingsViewModel : NotifyPropertyChange
    {
        public ICommand ResetSettings {  get; }
        public ICommand SaveSettings { get; }

        public object CurrentContentControl { get; set; }
        public bool ContentControlVisiability { get; set; }

        public List<string> ThemeSourse { get; set; } = new List<string> {"Светлая", "Темная"};
        private string _themeChange;
        public string ThemeChange 
        {
            get => _themeChange;
            set
            {
                if(_themeChange != value)
                {
                    _themeChange = value;
                    OnPropertyChange(nameof(ThemeChange));
                }
            }
        } 

        public SettingsViewModel() 
        {
            ThemeConfiguration();
            ContentControlVisiability = true;

            SaveSettings = new RelayCommand(Click => SaveAppSettings());
            ResetSettings = new RelayCommand(Click => ResetDefaultSettings());
        }

        private void ThemeConfiguration()
        {
            var themeCurrent = Settings.Default.Theme;
            if (themeCurrent == "Light")
            {
                ThemeChange = "Светлая";
            }
            else
            {
                ThemeChange = "Темная";
            }
        }
        private void SaveAppSettings()
        {
            var engNewDarkTheme = "Dark";
            var engNewLightTheme = "Light";

            if (ThemeChange == "Светлая")
            {
                ThemeManager.ChangeTheme(engNewLightTheme);
            }
            else if (ThemeChange == "Темная")
            {
                ThemeManager.ChangeTheme(engNewDarkTheme);
            }
            
            MessageBox.Show("Theme was successfuly updated");
        }
        private void ResetDefaultSettings()
        {
            ResetSettingsService.SettingsToDefault();
            MessageBox.Show("Settings was successfuly droped to DEFAULT, RELOAD PROGRAM PLEASE");
        }
    }
} 
