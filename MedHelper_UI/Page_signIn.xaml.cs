using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedHelper_UI
{
    /// <summary>
    /// Interaction logic for Page_signUp.xaml
    /// </summary>
    public partial class Page_signIn : Page
    {
        private HttpClient client = new HttpClient();
        public MainWindow MainWindow;
        public Page_signIn(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            email.GotFocus += RemoveText;
            email.LostFocus += AddText;
        }
        private void Login()
        {

            var str = "{\n" + $"\"Email\": \"{email.Text}\",\n" +
                      $"\"Pass\": \"{password.Password}\"\n" + "}";
            var httpContent = new StringContent(str, Encoding.UTF8,
                                    "application/json");
            httpContent.Headers.ContentType.MediaType = "application/json";
            var response = client.PostAsync("https://localhost:44374/api/v1/auth/login", httpContent);
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
                MessageBox.Show("Invalid credentials. Try again", "Invalid credentials", MessageBoxButton.OK, MessageBoxImage.Hand);
            }


        }
        private void BtmClickSignInPage(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void BtmClickRegisterSignInPage(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.Content = new Page_signUp(MainWindow);
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (email.Text == "example@email.com")
            {
                email.Text = "";
                email.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(email.Text))
            {
                email.Text = "example@email.com";
                email.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF716E6E"));
            }
        }
    }
}
