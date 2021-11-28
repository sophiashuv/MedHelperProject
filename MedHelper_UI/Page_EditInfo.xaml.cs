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
    /// Interaction logic for Page_EditInfo.xaml
    /// </summary>
    public partial class Page_EditInfo : Page
    {
        private HttpClient client = new HttpClient();
        public Page_Doctor MainWindow;
        public Page_EditInfo(Page_Doctor mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            setInformation();
        }

        private void setInformation()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainWindow.mainWindow.token);
            var response = client.GetAsync("https://localhost:44374/api/v1/auth/getInfo");
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<dynamic>(response.Result.Content.ReadAsStringAsync().Result);
                firstname.Text = res.result.firstName;
                email.Text = res.result.email;
                lastname.Text = res.result.lastName;
            }
            
        }

        private void BtmClickEdit(object sender, RoutedEventArgs e)
        {
            //TODO Дописати запит для апдейту
            MainWindow.DoctorFrame.Content = new Page_DoctorInfo(MainWindow);
        }
    }
}
