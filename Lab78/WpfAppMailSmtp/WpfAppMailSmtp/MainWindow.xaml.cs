using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

namespace WpfAppMailSmtp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Email = "xxx"; //Your default email
        private const string Password = "xxx"; //Your default Pwd
        private const string UserName = "xxx"; //Your default name

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            
            if(String.IsNullOrEmpty(textBoxMessage.Text)|| String.IsNullOrEmpty(textBoxTopic.Text) || String.IsNullOrEmpty(textBoxEmail.Text)  )
            {
                textBoxLogs.Text += "\n#Error sending#\nFill all textboxes\n#---#\n";
            }


            try
            {
                MailMessage message = new MailMessage(new MailAddress(textBoxMyEmail.Text, textBoxMyName.Text), new MailAddress(textBoxEmail.Text));

                message.Subject = textBoxTopic.Text;
                message.Body = textBoxMessage.Text;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                smtp.Credentials = new NetworkCredential(textBoxMyEmail.Text, textBoxMyPwd.Password);
                smtp.EnableSsl = true;

                smtp.SendMailAsync(message);
                textBoxLogs.Text += "\n#Email sent#\nFrom:"+ textBoxMyEmail.Text +"\nTo:"+ textBoxEmail.Text +"\nTopic:"+textBoxTopic.Text+"Size:"+textBoxMessage.Text.Length+"[symb]\n#---#\n";
            }
            catch (Exception ex)
            {
                textBoxLogs.Text += "\n#Error sending#\nError with email\n#---#\n";
            }
            textBoxEmail.Text = "";
            textBoxMessage.Text = "";
            textBoxTopic.Text = "";


        }

        private void buttonDefault_Click(object sender, RoutedEventArgs e)
        {
            textBoxMyEmail.Text = Email;
            textBoxMyPwd.Password = Password;
            textBoxMyName.Text = UserName;
        }
    }
}
