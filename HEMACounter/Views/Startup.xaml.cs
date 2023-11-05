using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HEMACounter.Views
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window
    {
        private Action<TournamentType> _callback;
        public Startup(Action<TournamentType> callback)
        {
            InitializeComponent();
            _callback = callback;
        }

        private void Button_Click_RubTeam(object sender, RoutedEventArgs e)
        {
            _callback(TournamentType.RubilnikTeam);
        }

        private void Button_Click_RubInd(object sender, RoutedEventArgs e)
        {
            _callback(TournamentType.RubilnikIndividual);
        }

        private void Button_Click_Stahlkugeln(object sender, RoutedEventArgs e)
        {
            _callback(TournamentType.Stahlkugeln);
        }

        private void Button_Click_Dante(object sender, RoutedEventArgs e)
        {
            _callback(TournamentType.Dante);
        }
    }
}
