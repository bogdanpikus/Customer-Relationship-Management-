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
        private List<string> EngThemeNames { get; set; } = new List<string> { "Light", "Dark" };
        public List<string> PagesSourse { get; set; } = new List<string> {"Заказы","Аналитика","Контакты", "Документы", "Склад","Эспорт/Импорт","Настройки" };
        private List<string> EngPageNames { get; set; } = new List<string> { "Order", "Analize", "Contacts", "Documents", "Storage", "Import", "Settings" };


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

        private string _startPage;
        public string StartPage
        {
            get => _startPage;
            set
            {
                if(_startPage != value)
                {
                    _startPage = value;
                    OnPropertyChange(nameof(StartPage));
                }
            }
        }

        public SettingsViewModel() 
        {
            ThemeConfiguration();
            StartPageConfiguration();
            ContentControlVisiability = true;

            SaveSettings = new RelayCommand(Click => SaveAppSettings());
            ResetSettings = new RelayCommand(Click => ResetDefaultSettings());
        }
        private void StartPageConfiguration()
        {
            string currentStartPage = Settings.Default.StartPage;
            switch (currentStartPage)
            {
                case "Order": StartPage = PagesSourse[0];  break;
                case "Analize": StartPage = PagesSourse[1]; break;
                case "Contacts": StartPage = PagesSourse[2]; break;
                case "Documents": StartPage = PagesSourse[3]; break;
                case "Storage": StartPage = PagesSourse[4]; break;
                case "Import": StartPage = PagesSourse[5]; break;
                case "Settings": StartPage = PagesSourse[6]; break;
            }
        }
        private void ThemeConfiguration()
        {
            string themeCurrent = Settings.Default.Theme;
            if (themeCurrent == EngThemeNames[0])
            {
                ThemeChange = ThemeSourse[0];
            }
            else
            {
                ThemeChange = ThemeSourse[1];
            }
        }
        private void SaveAppSettings()
        {
            SaveTheme();
            SaveStartPage();
        }
        private void SaveTheme()
        {

            if (ThemeChange == ThemeSourse[0])
            {
                ThemeManager.ChangeTheme(EngThemeNames[0]);
            }
            else if (ThemeChange == ThemeSourse[1])
            {
                ThemeManager.ChangeTheme(EngThemeNames[1]);
            }

            MessageBox.Show("Theme was successfuly updated");
        }
        private void SaveStartPage()
        {
            // Сохранение-изменение стартовой страницы
        }
        private void ResetDefaultSettings()
        {
            SettingsService.SettingsToDefault();
            MessageBox.Show("Settings was successfuly droped to DEFAULT, RELOAD PROGRAM PLEASE");
        }
    }
} 
