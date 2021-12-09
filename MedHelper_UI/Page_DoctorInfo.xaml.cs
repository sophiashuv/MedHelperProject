using MedHelper_EF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private HttpClient client = new HttpClient();
        public List<Patient> patients = new List<Patient>();
        private List<Patient> found = new List<Patient>();
        public List<Button> buttons;
        public Page_DoctorInfo(Page_Doctor mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            buttons = new List<Button>(patients.Count);
            setInformation();
            Search();
           
        }

        private void Search()
        {
            StackP.Children.Clear();
            buttons.Clear();
            found = patients.FindAll(x => x.UserName.Contains(FindResults.Text));
            var height = 30;
            for (int i = 0; i < found.Count(); i++)
            {
                buttons.Add(new Button());
                buttons[i].Content = found[i];
                buttons[i].Click += patient;
                buttons[i].Height = height;
                buttons[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF56C1CA"));
                buttons[i].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].Click += new RoutedEventHandler(patient_Click);
                StackP.Children.Add(buttons[i]);
            }
            FindResults.Text = "";
        }

        private void patient_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void patient(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var pat = (Patient)button.Content;

            MainWindow.DoctorFrame.Content = new PatientInfo(MainWindow, pat.PatientID);
        }
        private void setInformation()
        {
            firstlastname.Text = MainWindow.firstlastname;
            email.Text = MainWindow.email;
            //username.Text = MainWindow.username;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainWindow.mainWindow.token);
            var response = client.GetAsync("https://localhost:44374/api/v1/patient");
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<dynamic>(response.Result.Content.ReadAsStringAsync().Result);
                var patients_serial = res.result;
                foreach (var item in patients_serial)
                {
                    patients.Add(JsonConvert.DeserializeObject<Patient>(item.ToString()));
                }
            }

        }
        private void BtmEditClick(object sender, RoutedEventArgs e)
        {
            MainWindow.DoctorFrame.Content = new Page_EditInfo(MainWindow);
        }

        private void BtmAddPatientClick(object sender, RoutedEventArgs e)
        {
            MainWindow.DoctorFrame.Content = new Page_AddPatient(MainWindow);
        }

        private void BtmSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
