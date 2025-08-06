using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace CRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Create_Database_Click(object sender, RoutedEventArgs e)
        {
            Create_Database_Window create_Database_Window = new Create_Database_Window();
            create_Database_Window.Show();
            this.Close();
        }

        private void Open_Database_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Documentation_Click(object sender, RoutedEventArgs e)
        {
            try {
                Process.Start(new ProcessStartInfo { FileName = ".\\Documentation.pdf", UseShellExecute = true});
            }
            catch { MessageBox.Show("Throw new error while opening Documentation.pdf"); }
        }

        private void Close_MainWindow_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = Process.GetCurrentProcess();
                MessageBox.Show($"id:{process.Id}, name:{process.ProcessName}, VirtualMemory: {process.VirtualMemorySize64}");
            }
            catch { MessageBox.Show("Test Error"); }
        }
    }
}