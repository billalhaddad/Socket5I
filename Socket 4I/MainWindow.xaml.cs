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
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Rubrica rubrica;
        Socket socket = null;
        private DispatcherTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
            rubrica = new Rubrica();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var random = new Random();
            var ip=$"{random.Next(127,127)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
            Random random1 = new Random();
            int r = random1.Next(11000, 65000);
            IPAddress local_address = IPAddress.Parse(ip);
            IPEndPoint local_endpoint = new IPEndPoint(local_address,r);
            IP.Content = "Ip: "+local_address;
            PORTA.Content = "Porta: "+r;
            socket.Bind(local_endpoint);
            MyViewModel();
            Task.Run(RicevimentoMessaggio);
            socket.EnableBroadcast = true;
        }
        

        public void MyViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += OnTimerTick;
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (contatti.SelectedIndex==-1)
            {
                return;
            }
            lstMessaggi.Items.Clear();
            foreach(var item in (contatti.SelectedItem as Contatto).Messaggi)
            {
                lstMessaggi.Items.Add(item);
            }
        }

        private Task RicevimentoMessaggio()
        {//chiudilo
            
            while (true)
            {
                bool nuovoContatto = true;
                int nBytes = 1024;
                byte[] buffer = new byte[nBytes];
                EndPoint remoreEndPoint = new IPEndPoint(IPAddress.Any, 0);
                nBytes = socket.ReceiveFrom(buffer, ref remoreEndPoint);
                string from = ((IPEndPoint)remoreEndPoint).Address.ToString();
                string messaggio = Encoding.UTF8.GetString(buffer, 0, nBytes);
                List<Contatto> temp= new List<Contatto>();
                foreach (Contatto c in rubrica._contatti)
                {
                    if (c.IndirizzoIp.Address==((IPEndPoint)remoreEndPoint).Address.Address && c.IndirizzoIPEndPoint.Port==((IPEndPoint)remoreEndPoint).Port)
                    {
                        c.AggiungiMessaggio(false, messaggio);
                        nuovoContatto=false;
                    }
                }
                if (nuovoContatto)
                {
                    AggiuntaContatto(remoreEndPoint as IPEndPoint, messaggio, temp);
                }
                rubrica._contatti.AddRange(temp);
                
            }
        }
        private void AggiuntaContatto(IPEndPoint temp,string messaggio,List<Contatto> contatti)
        {
            MessageBoxResult result= MessageBox.Show("una persona che non hai nei contatti vuole mandarti un messaggio, vuoi aggiungerlo ?", "Errore", MessageBoxButton.YesNo);
            if (result==MessageBoxResult.Yes)
            {
                Contatto c = new Contatto(temp.Address.ToString(), temp.Address, temp);
                contatti.Add(c);
                c.AggiungiMessaggio(false, messaggio);
            }

        }
        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            string temp;
            Contatto c = rubrica._contatti[contatti.SelectedIndex];
            IPEndPoint remote_endpoint = c.IndirizzoIPEndPoint;
            temp=txtMessaggio.Text;
            byte[] messaggio = Encoding.UTF8.GetBytes(txtMessaggio.Text);
            socket.SendTo(messaggio, remote_endpoint);
            c.AggiungiMessaggio(true, temp);
            txtMessaggio.Text="";

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(username.Text!=""&&ipContatto.Text!=""&&portaContatto.Text!="")
            {
                AggiungiContatto();
                contatti.ItemsSource = null;
                contatti.ItemsSource = rubrica._contatti;
                username.Text="";
                ipContatto.Text="";
                portaContatto.Text="";
            }
            else
            {
                MessageBox.Show("Errore inserimento dati");
            }
            
        }

        private void AggiungiContatto()
        {
            IPAddress remote_address= IPAddress.Parse(ipContatto.Text);
            IPEndPoint remote_endpoint = new IPEndPoint(remote_address, int.Parse(portaContatto.Text));
            Contatto c = new Contatto(username.Text, remote_address, remote_endpoint);
            rubrica._contatti.Add(c);
        }

        private void contatti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contatti.SelectedItems!=null)
            {
                txtMessaggio.IsEnabled = true;
                btnInvia.IsEnabled=true;
            }
            else
            {
                btnInvia.IsEnabled = false;
                txtMessaggio.IsEnabled = false;

            }
        }

    }
}
