using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfTiles.Common;
using WpfTiles.Model;
using WpfTiles.ViewModels;

namespace WpfTiles
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.DataContext = new ViewModelMainWindow(ModelMainController.Instance.GameController);
            main.Show();
        }
    }
}
