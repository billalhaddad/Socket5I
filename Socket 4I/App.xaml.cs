using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Socket_4I
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            // Otteniamo la finestra principale
            MainWindow mainWindow = new MainWindow();

            // Impostiamo la larghezza e l'altezza della finestra principale
            mainWindow.Width = 437;
            mainWindow.Height = 360;

            // Mostriamo la finestra principale
            mainWindow.Show();
        }
    }
}
