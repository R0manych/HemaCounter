using HEMACounter.ViewModels;
using HEMACounter.Views;
using System.Windows;

namespace HEMACounter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Startup? _startupView;
        protected override void OnStartup(StartupEventArgs e) 
        {
            base.OnStartup(e);
            _startupView = new Startup(RunTournament);
            _startupView.Show();
        }

        private void RunTournament(TournamentType type)
        { 
            switch (type)
            {
                case TournamentType.RubilnikTeam:
                {
                    var commonContext = new TeamViewModel();
                    MainWindow = new Display();
                    MainWindow.DataContext = commonContext;
                    MainWindow.Show();

                    var control = new TeamControl();
                    control.DataContext = commonContext;
                    control.Show();
                    break;
                }
                case TournamentType.RubilnikIndividual:
                {
                    var commonContext = new IndividualViewModel();
                    MainWindow = new Display();
                    MainWindow.DataContext = commonContext;
                    MainWindow.Show();

                    var control = new IndividualControl();
                    control.DataContext = commonContext;
                    control.Show();
                    break;
                }
                case TournamentType.Stahlkugeln:
                {
                    var commonContext = new EggsViewModel();
                    MainWindow = new DisplayEggs();
                    MainWindow.DataContext = commonContext;
                    MainWindow.Show();

                    var control = new IndividualControl();
                    control.DataContext = commonContext;
                    control.Show();
                    break;
                }
                case TournamentType.Dante:
                {
                    var commonContext = new IndividualViewModel();
                    MainWindow = new Display();
                    MainWindow.DataContext = commonContext;
                    MainWindow.Show();

                    var control = new IndividualControl();
                    control.DataContext = commonContext;
                    control.Show();
                    break;
                }
            }
            _startupView.Close();
        }
    }
}
