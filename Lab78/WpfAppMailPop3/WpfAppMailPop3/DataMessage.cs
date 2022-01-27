using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMailPop3
{
    public class DataMessage
    {
        public string Date { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string MessageBody { get; set; }
    }
}
