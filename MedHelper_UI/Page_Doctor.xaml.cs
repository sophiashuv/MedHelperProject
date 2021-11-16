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
    /// Interaction logic for Page_Doctor.xaml
    /// </summary>
    public partial class Page_Doctor : Page
    {
        public List<String> patients = new List<string> { "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3" };
        public List<Button> buttons;
        public Page_Doctor()
        {
            InitializeComponent();
            Loaded += DoctorWindow_Loaded;

          
            buttons = new List<Button>(patients.Count);


            var height = 30;
            for (int i = 0; i < patients.Count(); i++)
            {
                buttons.Add(new Button());
                buttons[i].Content = i;
                buttons[i].Height = height;
                buttons[i].Background = new  SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFED635E"));
                buttons[i].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                StackP.Children.Add(buttons[i]);
            }
        }

        private void DoctorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorFrame.Content = new Page_DoctorInfo(this);
          
            
        }

    }
}
