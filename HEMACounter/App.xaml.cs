using HEMACounter.ViewModels;
using System.Windows;

namespace HEMACounter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) //you need to add this.
        {
            base.OnStartup(e);

            switch (MessageBox.Show("Для проведения командных соревнований нажмите Yes, для индивидуальных - No, Stahlkugeln - Cancel", "Внимание", MessageBoxButton.YesNoCancel, MessageBoxImage.Information))
            {
                case MessageBoxResult.Yes:
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
                case MessageBoxResult.No:
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
                case MessageBoxResult.Cancel:
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
            }
        }
    }
}
