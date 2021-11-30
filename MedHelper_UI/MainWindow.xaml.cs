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
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
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

        private void BtmClickDoctor(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = new Page_Doctor(this);
        }

        private void BtmClickAddPatient(object sender, RoutedEventArgs e)
        {
            var k = new Page_Doctor(this);
            var v = k.DoctorFrame;
            v.Content = new Page_AddPatient(k);
            this.MainFrame.Content = v;
        }

        private void BtmClickEditDoctor(object sender, RoutedEventArgs e)
        {

        }

        private void BtmClickPatient(object sender, RoutedEventArgs e)
        {

        }
    }
}
