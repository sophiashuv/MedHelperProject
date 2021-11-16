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
    /// Interaction logic for Page_EditInfo.xaml
    /// </summary>
    public partial class Page_EditInfo : Page
    {
        public Page_Doctor MainWindow;
        public Page_EditInfo(Page_Doctor mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }

        private void BtmClickEdit(object sender, RoutedEventArgs e)
        {
            MainWindow.DoctorFrame.Content = new Page_DoctorInfo(MainWindow);
        }
    }
}
