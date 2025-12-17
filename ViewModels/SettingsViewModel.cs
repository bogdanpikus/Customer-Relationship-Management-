using CRM.Properties;
using CRM.Services;
using CRM.Themes;
using System.Windows.Input;

namespace CRM.ViewModels
{
    public class SettingsViewModel : NotifyPropertyChange
    {
        public ICommand ResetSettings {  get; }
        public ICommand Otmenit {  get; }
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
                    ThemeChangeAction(ThemeChange);
                }
            }
        } 

        public SettingsViewModel() 
        {
            ContentControlVisiability = true;
            ThemeConfiguration();
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
        private void ThemeChangeAction(string newTheme)
        {
            var engNewDarkTheme = "Dark";
            var engNewLightTheme = "Light";

            if(newTheme == "Светлая")
            {
                Settings.Default.Theme = engNewLightTheme;
                Settings.Default.Save();
                ThemeManager.ChangeTheme(engNewLightTheme);
            }
            else if(newTheme == "Темная")
            {
                Settings.Default.Theme = engNewDarkTheme;
                Settings.Default.Save();
                ThemeManager.ChangeTheme(engNewDarkTheme);
            }
        }
    }
} 
