using MedHelper_EF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for Page_AddPatient.xaml
    /// </summary>
    public partial class Page_AddPatient : Page, INotifyPropertyChanged
    {
        private HttpClient client = new HttpClient();
        public Page_Doctor MainWindow;
        private List<int> medecine = new List<int>();
        private List<int> diseases = new List<int>();
        public List<TextBlock> textBoxesMedicine = new List<TextBlock>();
        public List<TextBlock> textBoxesDisasters = new List<TextBlock>();

        public event PropertyChangedEventHandler PropertyChanged;

        public Page_AddPatient(Page_Doctor mainWindow)
        {
            
            InitializeComponent();
            MainWindow = mainWindow;
            setMedicine();
            setDiseases();
            username.GotFocus += RemoveText;
            username.LostFocus += AddText;
            //for (var i = 0; i < 1000; i++)
            //{
            //    cb.Items.Add($"Medicine {i}");
            //}

            //for (var i = 0; i < 1000; i++)
            //{
            //    cbd.Items.Add($"Disaster {i}");
            //}
        }


        private void Cb_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cb.IsDropDownOpen = true;
        }

        private void setMedicine()
        {
            var responseMedicine = client.GetAsync("https://localhost:44374/api/v1/medicines");
            responseMedicine.Wait();
            if (responseMedicine.Result.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<dynamic>(responseMedicine.Result.Content.ReadAsStringAsync().Result);
                var medicines= res.result;
                foreach (var item in medicines)
                {
                    cb.Items.Add(JsonConvert.DeserializeObject<Medicine>(item.ToString()));
                }
            }
        }
        private void setDiseases()
        {
            var responseMedicine = client.GetAsync("https://localhost:44374/api/v1/diseases");
            responseMedicine.Wait();
            if (responseMedicine.Result.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<dynamic>(responseMedicine.Result.Content.ReadAsStringAsync().Result);
                var deseases = res.result;
                foreach (var item in deseases)
                {
                    cbd.Items.Add(JsonConvert.DeserializeObject<Disease>(item.ToString()));
                }
            }
        }
        public static TextBlock CreateTextBlock(String text)
        {
            TextBlock textBolock = new TextBlock();
            textBolock.Text = text;
            textBolock.Width = 222;
            textBolock.Height = 25;
            textBolock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            return textBolock;
        }

        private void BtnDelMedicine(object sender, RoutedEventArgs e)
        {
            if (textBoxesMedicine.Count != 0)
            {
                medecine.RemoveAt(textBoxesMedicine.Count - 1);
                textBoxesMedicine.Remove(textBoxesMedicine[textBoxesMedicine.Count - 1]);
                StackP1.Children.Remove(StackP1.Children[textBoxesMedicine.Count]);
            }
        }

        private void BtnDelDisaster(object sender, RoutedEventArgs e)
        {
            if (textBoxesDisasters.Count != 0)
            {
                diseases.RemoveAt(textBoxesDisasters.Count - 1);
                textBoxesDisasters.Remove(textBoxesDisasters[textBoxesDisasters.Count - 1]);
                StackP2.Children.Remove(StackP2.Children[textBoxesDisasters.Count]);
            }
        }

        private void AddPatient()
        {
            var sex = "";
            if((bool)male.IsChecked)
            {
                sex = "one";
            }
            else if((bool)female.IsChecked)
            {
                sex = "two";
            }
            else
            {
                //показати шо треба вибрати
            }

            var str = "{\n" + $"\"UserName\": \"{username.Text}\",\n" +
                      $"\"Gender\": \"{sex}\",\n" +
                      $"\"MedicineIds\": {JsonConvert.SerializeObject(medecine)},\n" +
                      $"\"DiseasesIds\": {JsonConvert.SerializeObject(diseases)},\n" +
                      $"\"Birthdate\": {JsonConvert.SerializeObject(date.SelectedDate.Value)}\n" + "}";
            var httpContent = new StringContent(str, Encoding.UTF8,
                                    "application/json");
            httpContent.Headers.ContentType.MediaType = "application/json";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainWindow.mainWindow.token);
            var response = client.PostAsync("https://localhost:44374/api/v1/patient", httpContent);
            response.Wait();
            if(response.Result.IsSuccessStatusCode)
            {
                MainWindow.DoctorFrame.Content = new Page_DoctorInfo(MainWindow);
            }
        }
        private void BtmAddPatientToDb(object sender, RoutedEventArgs e)
        {
            AddPatient();

        }

        private void ComboboxMedicine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb.SelectedItem != null)
            {
                var selectedTMedicine = (Medicine)cb.SelectedItem;
                var textBlock = CreateTextBlock(selectedTMedicine.ToString());
                medecine.Add(selectedTMedicine.MedicineID);
                StackP1.Children.Add(textBlock);
                textBoxesMedicine.Add(textBlock);
            }
        }

        private void ComboboxDisaster_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbd.SelectedItem != null)
            {
                var selectedDisease = (Disease)cbd.SelectedItem;
                var textBlock = CreateTextBlock(selectedDisease.ToString());
                diseases.Add(selectedDisease.DiseaseID);
                StackP2.Children.Add(textBlock);
                textBoxesDisasters.Add(textBlock);
            }
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (username.Text == "Name")
            {
                username.Text = "";
                username.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username.Text))
            {
                username.Text = "Name";
                username.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF716E6E"));
            }
        }

    }
}
