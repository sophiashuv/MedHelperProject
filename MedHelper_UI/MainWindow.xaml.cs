using System;
using System.Windows;
using System.Windows.Threading;

namespace MedHelper_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string token;

        DispatcherTimer timer = new DispatcherTimer(); 
        public MainWindow()
        {
            
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Media.Source = new Uri(@"..\..\..\images\LoadingLogo.gif", UriKind.Relative);
            cabinet.IsEnabled = false;
            logout.IsEnabled = false;
            cabinet.Visibility = Visibility.Collapsed;
            logout.Visibility = Visibility.Collapsed;
            Loading();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //MainFrame.Content = new Page_Welcome();
            
        }

        private void BtmClickSignIn(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page_signIn(this);
        }

        private void BtmClickSignUp(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page_signUp(this);
        }

        private void BtmClickMedHelper(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page_Welcome();
        }
        private void Logout()
        {
            token = "";
            cabinet.IsEnabled = false;
            logout.IsEnabled = false;
            cabinet.Visibility = Visibility.Collapsed;
            logout.Visibility = Visibility.Collapsed;
            login.IsEnabled = true;
            signin.IsEnabled = true;
            login.Visibility = Visibility.Visible;
            signin.Visibility = Visibility.Visible;
            MainFrame.Content = new Page_Welcome();

        }
        private void BtmClickLogOut(object sender, RoutedEventArgs e)
        {
            Logout();
        }

        private void BtmClickDoctor(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page_Doctor(this);
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Media.Position = new TimeSpan(0, 0, 1);
            Media.Play();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            Media.Visibility = Visibility.Hidden;
            Header.Visibility = Visibility.Visible;
            login.Visibility = Visibility.Visible;
            signin.Visibility = Visibility.Visible;
            mainWin.Visibility = Visibility.Visible;
            Footer.Visibility = Visibility.Visible;
            MainFrame.Content = new Page_Welcome();
        }

        void Loading()
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();
            Media.Play();
        }
    }
}
