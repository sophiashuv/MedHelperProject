using MedHelper_EF.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedHelper_UI
{
    /// <summary>
    /// Interaction logic for Page_Doctor.xaml
    /// </summary>
    public partial class Page_Doctor : Page
    {
        private HttpClient client = new HttpClient();
        public MainWindow mainWindow;
        public List<Patient> patients = new List<Patient>();
        public List<Button> buttons;
        private Dictionary<int, int> patientDict = new Dictionary<int, int>();
        public string username;
        public string email;
        public string firstlastname;
        public Page_Doctor(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            Loaded += DoctorWindow_Loaded;
            setInformation();
          
            buttons = new List<Button>(patients.Count);


            var height = 30;
            for (int i = 0; i < patients.Count(); i++)
            {
                patientDict.Add(i + 1, patients[i].PatientID);
                buttons.Add(new Button());
                buttons[i].Content = i+1;
                buttons[i].Click += patient;
                buttons[i].Height = height;
                buttons[i].Background = new  SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFED635E"));
                buttons[i].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                StackP.Children.Add(buttons[i]);
            }
        }

        private void patient(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var num = (int)button.Content;
            DoctorFrame.Content = new PatientInfo(this, patientDict[num]);
        }

        private void setInformation()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mainWindow.token);
            var response = client.GetAsync("https://localhost:44374/api/v1/auth/getInfo");
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<dynamic>(response.Result.Content.ReadAsStringAsync().Result);
                firstlastname = res.result.lastName + " " + res.result.firstName;
                email = res.result.email;
                username = "А в модельці цього поля немаа";
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mainWindow.token);
            var responsePatient = client.GetAsync("https://localhost:44374/api/v1/patient");
            responsePatient.Wait();
            if (responsePatient.Result.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<dynamic>(responsePatient.Result.Content.ReadAsStringAsync().Result);
                var patients_serial = res.result;
                foreach (var item in patients_serial)
                {
                    patients.Add(JsonConvert.DeserializeObject<Patient>(item.ToString()));
                }
            }
        }
        private void DoctorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorFrame.Content = new Page_DoctorInfo(this);
        }

    }
}
