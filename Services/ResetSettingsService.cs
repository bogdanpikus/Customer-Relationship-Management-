using CRM.Properties;

namespace CRM.Services
{
    public static class ResetSettingsService
    {
       public static void SettingsToDefault()
        {
            Settings.Default.Theme = "Light";
            Settings.Default.StartPage = "";
            Settings.Default.WindowScreenSize = "";
            Settings.Default.Company = "";
            Settings.Default.FontSize = "";
            Settings.Default.Currency = "UAN";
            Settings.Default.Languaguе = "ru-US";
            Settings.Default.DateFormat = "";
            Settings.Default.CurrencyFormat = "";
            Settings.Default.Tips = "On";
            Settings.Default.Mode = "Simple";
            Settings.Default.Sidebar = "Left";
            Settings.Default.Save();
        }
    }
}
