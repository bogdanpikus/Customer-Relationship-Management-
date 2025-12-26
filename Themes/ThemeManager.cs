using CRM.Properties;
using System.Diagnostics;
using System.Windows;

namespace CRM.Themes
{
    public static class ThemeManager
    {
        public static void ChangeTheme(string themeValue)
        {

            Settings.Default.Theme = themeValue;
            Settings.Default.Save();

            var dictionaries = Application.Current.Resources.MergedDictionaries;

            var dictNew = new ResourceDictionary 
            {
                Source = new Uri(themeValue == "Dark" ? "/Themes/DarkTheme.xaml" : "/Themes/LightTheme.xaml", UriKind.Relative)
            };

            var oldDictionary = dictionaries.FirstOrDefault(d =>
            d.Source.OriginalString.Contains("/Themes/"));

            if (oldDictionary != null)
            {
                dictionaries.Remove(oldDictionary);
                Debug.WriteLine($"{oldDictionary.Source} has been deleted");
            }

            dictionaries.Add(dictNew);
            Debug.WriteLine($"{dictNew.Source} has been added");
        }
    }
}
