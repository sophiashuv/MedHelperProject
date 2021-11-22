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
    /// Interaction logic for Page_AddPatient.xaml
    /// </summary>
    public partial class Page_AddPatient : Page
    {
        public Page_Doctor MainWindow;
        public List<TextBox> textBoxesMedicine;
        public List<TextBox> textBoxesDisasters;
        public Page_AddPatient(Page_Doctor mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;

            var textBox = CreateTextBox();
            textBoxesMedicine = new List<TextBox> { textBox };
            StackP1.Children.Add(textBox);

            var textBox2 = CreateTextBox("Disaster");
            textBoxesDisasters = new List<TextBox> { textBox2 };
            StackP2.Children.Add(textBox2);
        }

        public static TextBox CreateTextBox(String text= "Medicine")
        {
            TextBox textBox = new TextBox();
            textBox.Text = text;
            textBox.Width = 222;
            textBox.Height = 25;
            textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF716E6E"));
            return textBox;
        }

        private void BtnAddMedicine(object sender, RoutedEventArgs e)
        {
            var textBox = CreateTextBox();
            textBoxesMedicine.Add(textBox);
            StackP1.Children.Add(textBox);
        }

        private void BtnDelMedicine(object sender, RoutedEventArgs e)
        {
            if (textBoxesMedicine.Count > 1)
            {
                textBoxesMedicine.Remove(textBoxesMedicine[textBoxesMedicine.Count - 1]);
                StackP1.Children.Remove(StackP1.Children[textBoxesMedicine.Count]);
            }
        }

        private void BtnAddDisaster(object sender, RoutedEventArgs e)
        {
            var textBox = CreateTextBox("Disaster");
            textBoxesDisasters.Add(textBox);
            StackP2.Children.Add(textBox);
        }

        private void BtnDelDisaster(object sender, RoutedEventArgs e)
        {
            if (textBoxesDisasters.Count > 1)
            {
                textBoxesDisasters.Remove(textBoxesDisasters[textBoxesDisasters.Count - 1]);
                StackP2.Children.Remove(StackP2.Children[textBoxesDisasters.Count]);
            }
        }

        private void BtmAddPatientToDb(object sender, RoutedEventArgs e)
        {
            MainWindow.DoctorFrame.Content = new Page_DoctorInfo(MainWindow);
        }
    }
}
