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
    /// Interaction logic for Page_signIn.xaml
    /// </summary>
    public partial class Page_signUp : Page
    {
        public MainWindow MainWindow;
        public Page_signUp(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }

        private void BtmClickRegister(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.Content = new Page_Doctor();
        }
    }
}
