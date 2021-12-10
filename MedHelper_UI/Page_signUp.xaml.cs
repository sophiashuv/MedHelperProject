using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
        private HttpClient client = new HttpClient();
        public MainWindow MainWindow;
        public Page_signUp(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }
        private void Register()
        {
            if (password.Password != confirm.Password)
            {
                MessageBox.Show("Passwords are different", "Wrong password", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                var str = "{\n" + $"\"FirstName\": \"{firstname.Text}\",\n" +
                          $"\"LastName\": \"{lastname.Text}\",\n" +
                          $"\"Email\": \"{email.Text}\",\n" +
                          $"\"Pass\": \"{password.Password}\"\n" + "}";
                var httpContent = new StringContent(str, Encoding.UTF8,
                                        "application/json");
                httpContent.Headers.ContentType.MediaType = "application/json";
                var response = client.PostAsync("https://localhost:44374/api/v1/auth/registration", httpContent);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var res = JsonConvert.DeserializeObject<dynamic>(response.Result.Content.ReadAsStringAsync().Result);
                    MainWindow.token = res.result.accessToken;
                    MainWindow.cabinet.IsEnabled = true;
                    MainWindow.logout.IsEnabled = true;
                    MainWindow.cabinet.Visibility = Visibility.Visible;
                    MainWindow.logout.Visibility = Visibility.Visible;
                    MainWindow.login.IsEnabled = false;
                    MainWindow.signin.IsEnabled = false;
                    MainWindow.login.Visibility = Visibility.Collapsed;
                    MainWindow.signin.Visibility = Visibility.Collapsed;
                    MainWindow.MainFrame.Content = new Page_Doctor(MainWindow);
                }
                else
                {
                    MessageBox.Show("Something went wrong. Try again", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            
            }
        }
        private void BtmClickRegister(object sender, RoutedEventArgs e)
        {
            Register();
        }
    }
}
