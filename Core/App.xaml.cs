using System.Windows;
using CRM.Properties;

namespace CRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            
            string theme = Settings.Default.Theme;
            var dict = new ResourceDictionary
            {
                Source = new Uri(theme == "Dark" ? "/Themes/DarkTheme.xaml" : "/Themes/LightTheme.xaml", UriKind.Relative)
            };

            Resources.MergedDictionaries.Add(dict);
            MigrateSettings();
        }

        private void MigrateSettings()
        {

        }
    }

}