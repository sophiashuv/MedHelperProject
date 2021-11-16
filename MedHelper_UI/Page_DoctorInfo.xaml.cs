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
    /// Interaction logic for Page_DoctorInfo.xaml
    /// </summary>
    public partial class Page_DoctorInfo : Page
    {

        public Page_Doctor MainWindow;
        public List<String> patients = new List<string> { "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3" };
        public List<Button> buttons;
        public Page_DoctorInfo(Page_Doctor mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            buttons = new List<Button>(patients.Count);


            var height = 30;
            for (int i = 0; i < patients.Count(); i++)
            {
                buttons.Add(new Button());
                buttons[i].Content = patients[i];
                buttons[i].Height = height;
                buttons[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF56C1CA"));
                buttons[i].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                StackP.Children.Add(buttons[i]);
            }
        }

        private void BtmEditClick(object sender, RoutedEventArgs e)
        {
            MainWindow.DoctorFrame.Content = new Page_EditInfo(MainWindow);
        }
    }
}
