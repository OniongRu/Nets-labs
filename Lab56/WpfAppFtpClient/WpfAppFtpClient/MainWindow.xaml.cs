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

namespace WpfAppFtpClient
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

        private void buttonDefault_Click(object sender, RoutedEventArgs e)
        {
            textBoxUrl.Text = "127.0.0.1/";
            textBoxFile.Text = "testPicture.jpg";
            textBoxResult.Text = "\n#Used default settings#\n";

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            

            if (String.IsNullOrEmpty(textBoxFile.Text) || String.IsNullOrEmpty(textBoxUrl.Text))
            {
                textBoxResult.Text += "\n#_Error, enter all fields_#\n";
                return;
            }

            try
            {
                textBoxResult.Text += "\nLoad: "+textBoxRead.Text+textBoxUrl+textBoxFile+"\n";
                textBoxResult.Text += "[" + DateTime.Now.ToString("HH:mm:ss.ffff") + "] Start_load\n";
                textBoxResult.Text += "[" +  DateTime.Now.ToString("HH:mm:ss.ffff") + "] Connection\n";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(textBoxRead.Text + textBoxUrl.Text + textBoxFile.Text);
                
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                textBoxResult.Text += "[" + DateTime.Now.ToString("HH:mm:ss.ffff") + "] Response_from_server\n";
                Stream responseStream = response.GetResponseStream();
                textBoxResult.Text += "[" + DateTime.Now.ToString("HH:mm:ss.ffff") + "] Data_from_server\n";
               

                FileStream fs = new FileStream("C:\\ftpDownload" + "\\" + textBoxFile.Text, FileMode.Create);
                textBoxResult.Text += "[" + DateTime.Now.ToString("HH:mm:ss.ffff") + "] FSream_ceated\n";
                byte[] buffer = new byte[64];
                int size = 0;
                while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    fs.Write(buffer, 0, size);
                fs.Close();
                response.Close();
                textBoxResult.Text += "[" + DateTime.Now.ToString("HH:mm:ss.ffff") + "] End_load\n";
            }
            catch (Exception ex)
            {
                textBoxResult.Text += "\n#Error#\n" + ex.Message+"\n#---#\n";
            }

        }
    }
}
