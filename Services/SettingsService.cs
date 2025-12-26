using CRM.Properties;
using System.Net.NetworkInformation;

namespace CRM.Services
{
    public static class SettingsService
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
       
        public static void DBPathSave(string newdbConnection) // TIP: когда выбираешь путь для запуска базы данных
        {
            string originaldbPath = Settings.Default.DBConnection;
            if(newdbConnection != originaldbPath)
            {
                Settings.Default.DBConnection = newdbConnection;
                Settings.Default.Save();
            }
        }

    }
}
