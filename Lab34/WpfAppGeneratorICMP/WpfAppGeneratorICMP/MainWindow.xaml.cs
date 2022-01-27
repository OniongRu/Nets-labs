using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace WpfAppGeneratorICMP
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
            textBoxReceiver.Text = "127.0.0.1";
            textBoxType.Text = "8";
            textBoxCode.Text = "0";
            textBoxMessage.Text = "TestMessage";
            textBoxLogs.Text += "\n#Used default settings#\nReciever: 127.0.0.1\nType = 8(Echo)\nCode = 0 (No Code)\nMessage: TestMessageDefault\n#----#\n";
        }

        private void buttonGenerate_Click(object sender, RoutedEventArgs e)
        {
            string senderStr, receiverStr, typeStr, codeStr, messageStr;
            senderStr = textBoxSender.Text;
            receiverStr = textBoxReceiver.Text;
            typeStr = textBoxType.Text;
            codeStr = textBoxCode.Text;
            messageStr = textBoxMessage.Text;
            if (String.IsNullOrEmpty(receiverStr) || String.IsNullOrEmpty(typeStr) || String.IsNullOrEmpty(codeStr) || String.IsNullOrEmpty(messageStr))
            {
                textBoxLogs.Text += "\n\n# Fill all fields #\n";
                return;
            }
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);

                IPAddress ipSender = IPAddress.Parse(senderStr); 
                IPAddress ipReceiver = IPAddress.Parse(receiverStr); 
                IPEndPoint ip = new IPEndPoint(ipReceiver, 6666);

                IcmpPacket icmp = new IcmpPacket();
                icmp.Type = (byte)Convert.ToInt32(typeStr);
                icmp.Code = (byte)Convert.ToInt32(codeStr);
                icmp.CheckSum = 0;

                byte[] icmpData = new byte[1024]; 
                int bytes;

                //  SendMessage
                icmpData = Encoding.ASCII.GetBytes(textBoxMessage.Text); 
                Buffer.BlockCopy(icmpData, 0, icmp.Message, 4, icmpData.Length); 
                icmp.Size = icmpData.Length + 4;
                icmp.CheckSum = icmp.getChecksum(); 
                socket.SendTo(icmp.getData(), icmp.Size + 4, SocketFlags.None, ip);
                
                
                icmpData = new byte[1024];
                var ipEndPoint = (EndPoint)ip;

                //Get message
                bytes = socket.ReceiveFrom(icmpData,  ref ipEndPoint); 
                IcmpPacket response = new IcmpPacket(icmpData, bytes);
                string result = Encoding.ASCII.GetString(response.Message, 4, response.Size - 4);
                socket.Close();


                textBoxLogs.Text += "\n#---#\nReplyFrom: " + ipEndPoint.ToString() +  "\nMessage: " + result + "\n#---#\n";

                

            }
            catch (Exception exception)
            {
                textBoxLogs.Text += "\n" + exception.Message + "\n";
            }
        }
    }
}
