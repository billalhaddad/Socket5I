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
using System.Windows.Threading;

namespace Socket_4I
{
    internal class Contatto
    {
        public string NomeUtente { get; private set; }
        public IPAddress IndirizzoIp { get; private set; }
        public IPEndPoint IndirizzoIPEndPoint {  get; private set; }
        public List<string> Messaggi { get; private set; }

        public Contatto(string nomeutente,IPAddress indirizzoIp, IPEndPoint indirizzoIPEndPoint)
        {
            NomeUtente = nomeutente;
            IndirizzoIp = indirizzoIp;
            IndirizzoIPEndPoint = indirizzoIPEndPoint;
            Messaggi = new List<string>();
        }
        public override string ToString()
        {
            return $"{NomeUtente}";
        }
        public void AggiungiMessaggio(bool myMessage,string messaggio)
        {
            if (myMessage)
                Messaggi.Add("io: "+messaggio);
            else
                Messaggi.Add(NomeUtente+": "+messaggio);
        }
    }
}
