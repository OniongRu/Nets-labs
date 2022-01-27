using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace WpfAppHttpClient
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


        /// <summary>
        /// Проверка соедиения с сетью
        /// </summary>
        private bool CheckConnetction()
        {
            int timeoutMs = 10000;
            string url = "https://www.google.com";
            try
            {
                url ??= CultureInfo.InstalledUICulture switch
                {
                    { Name: var n } when n.StartsWith("ru") =>
                    "https://www.google.com",
                    _ =>
                        "http://www.gstatic.com/generate_204",
                };

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    buttonConnectionStatus.Background = Brushes.Green;
                    return true;
                }
            }
            catch
            {
                buttonConnectionStatus.Background = Brushes.Red;
                return false;
            }
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckConnetction())
            {
                textBoxResult.Text = "no internet connection";
                return;
            }
            string loadedAddPath = textBoxUrl.Text;
            if (string.IsNullOrEmpty(loadedAddPath))
            {
                textBoxResult.Text = "Enter page URL.";
                return;
            }
            var request = (HttpWebRequest)WebRequest.Create(loadedAddPath);
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = "utf-8";

                string ct = response.Headers["Content-Type"];

                if (ct != null)
                    encoding = ct.Substring(ct.IndexOf("charset=") + 8);

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                if (textBoxResult.Text.Length > 1000)
                    textBoxResult.Text = "#### Page data ####\n" + reader.ReadToEnd();
                else
                    textBoxResult.Text += "\n\n#### Page data ####\n" + reader.ReadToEnd();

                response.Close();
                reader.Close();
            }
            catch
            {
                textBoxResult.Text += "\n# Error, address is not correct #\n";
                return;
            }

        }

        private void buttonConnectionStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckConnetction();
            textBoxResult.Text += "\n# You have connection to net #\n";
        }
    }
}
