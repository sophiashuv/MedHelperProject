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

            firstname.GotFocus += RemoveTextName;
            firstname.LostFocus += AddTextName;
            lastname.GotFocus += RemoveTextSurName;
            lastname.LostFocus += AddTextSurName;
            email.GotFocus += RemoveTextEmail;
            email.LostFocus += AddTextEmail;
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
            }
        }
        private void BtmClickRegister(object sender, RoutedEventArgs e)
        {
            Register();
        }

        public void RemoveTextName(object sender, EventArgs e)
        {
            if (firstname.Text == "First Name")
            {
                firstname.Text = "";
                firstname.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
        }

        public void AddTextName(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstname.Text))
            {
                firstname.Text = "First Name";
                firstname.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF716E6E"));
            }
        }

        public void RemoveTextSurName(object sender, EventArgs e)
        {
            if (lastname.Text == "Last Name")
            {
                lastname.Text = "";
                lastname.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
        }

        public void AddTextSurName(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lastname.Text))
            {
                lastname.Text = "Last Name";
                lastname.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF716E6E"));
            }
        }

        public void RemoveTextEmail(object sender, EventArgs e)
        {
            if (email.Text == "Email address")
            {
                email.Text = "";
                email.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
        }

        public void AddTextEmail(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(email.Text))
            {
                email.Text = "Email address";
                email.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF716E6E"));
            }
        }
    }
}
