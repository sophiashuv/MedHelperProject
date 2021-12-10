using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MedHelper_UI
{
    /// <summary>
    /// Interaction logic for Page_EditInfo.xaml
    /// </summary>
    public partial class Page_EditInfo : Page
    {
        private HttpClient client = new HttpClient();
        public Page_Doctor MainWindow;
        private string hashedpass;
        public Page_EditInfo(Page_Doctor mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            setInformation();
        }
        private void Edit()
        {
            if (!BCrypt.Net.BCrypt.Verify(password.Password, hashedpass))
            {
                MessageBox.Show("Wrong password. Try again", "Wrong password", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                var pass = password.Password; ;
                if (newpass.Password != "")
                {
                    pass = newpass.Password;
                }
                if (newpass.Password != confirm.Password)
                {
                    MessageBox.Show("Passwords are different", "Wrong password", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {


                    var str = "{\n" + $"\"FirstName\": \"{firstname.Text}\",\n" +
                              $"\"LastName\": \"{lastname.Text}\",\n" +
                              $"\"Email\": \"{email.Text}\",\n" +
                              $"\"Pass\": \"{pass}\"\n" + "}";
                    var httpContent = new StringContent(str, Encoding.UTF8,
                                            "application/json");
                    httpContent.Headers.ContentType.MediaType = "application/json";
                    var response = client.PutAsync("https://localhost:44374/api/v1/doctor", httpContent);
                    response.Wait();
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var res = JsonConvert.DeserializeObject<dynamic>(response.Result.Content.ReadAsStringAsync().Result);
                        MainWindow.mainWindow.MainFrame.Content = new Page_Doctor(MainWindow.mainWindow);
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong. Try again", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);

                    }
                }
            }
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
                this.hashedpass = res.result.pass;
            }

        }

        private void BtmClickEdit(object sender, RoutedEventArgs e)
        {
            Edit();

        }
    }
}
