using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.Logic;
using Client.View;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = this.input.Text.Trim();
            if (url.Length < 5)
            {
                MessageBox.Show("Small url!");
                return;
            }
            if (url[url.Length - 1] == '/') url = url.Substring(0, url.Length - 1);

            

            this.button.IsEnabled = false;
            this.button.Content = "Check...";
            this.flag.Background = Brushes.White;

            if (Requests.checkUrl(url))
            {
                this.flag.Background = Brushes.Green;
                Requests.setUrl(url);
                Close();
            }
            else
                this.flag.Background = Brushes.Red;

            this.button.IsEnabled = true;
            this.button.Content = "Connect";
        }
    }
}
