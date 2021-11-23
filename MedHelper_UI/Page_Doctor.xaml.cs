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
    /// Interaction logic for Page_Doctor.xaml
    /// </summary>
    public partial class Page_Doctor : Page
    {
        private HttpClient client = new HttpClient();
        public MainWindow mainWindow;
        public List<String> patients = new List<string> { "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3", "Patient1", "Patient2", "Patient3" };
        public List<Button> buttons;
        public string username;
        public string email;
        public string firstlastname;
        public Page_Doctor(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
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
        }
        private void DoctorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            setInformation();
            DoctorFrame.Content = new Page_DoctorInfo(this);
        }

    }
}
