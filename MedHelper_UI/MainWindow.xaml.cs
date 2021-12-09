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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedHelper_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string token;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            cabinet.IsEnabled = false;
            logout.IsEnabled = false;
            cabinet.Visibility = Visibility.Collapsed;
            logout.Visibility = Visibility.Collapsed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page_Welcome();
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
    }
}
