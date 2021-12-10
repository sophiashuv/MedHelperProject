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
    /// Interaction logic for Page_EditPatient.xaml
    /// </summary>
    public partial class Page_EditPatient : Page
    {
        private HttpClient client = new HttpClient();
        public Page_Doctor MainWindow;
        private List<int> medecine = new List<int>();
        private List<int> diseases = new List<int>();
        public List<TextBlock> textBoxesMedicine = new List<TextBlock>();
        public List<TextBlock> textBoxesDisasters = new List<TextBlock>();
        private int UserId;
        public Page_EditPatient(Page_Doctor mainWindow, int userId)
        {
            UserId = userId;
            MainWindow = mainWindow;
            InitializeComponent();
            setInformation();
            setMedicine();
            setDiseases();
        }

        private void setInformation()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainWindow.mainWindow.token);
            var response = client.GetAsync($"https://localhost:44374/api/v1/patient/{UserId}");
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<dynamic>(response.Result.Content.ReadAsStringAsync().Result);
                username.Text = res.result.userName;
                date.SelectedDate = res.result.birthdate;
                if (res.result.gender == "one")
                {
                    male.IsChecked = true;
                }
                else
                {
                    female.IsChecked = true;
                }
                var medecineList = new List<Medicine>();
                foreach (var item in res.result.medicines)
                {
                    var el = new Medicine() { MedicineID = item.medicineID, Name = item.name };
                        
                    var textBlock = CreateTextBlock(el.ToString());
                    medecine.Add(el.MedicineID);
                    StackP1.Children.Add(textBlock);
                    textBoxesMedicine.Add(textBlock);
                }
                foreach (var item in res.result.diseases)
                {
                    var el= new Disease() { DiseaseID=item.DiseaseID, Title=item.title };
                    var textBlock = CreateTextBlock(el.ToString());
                    diseases.Add(el.DiseaseID);
                    StackP2.Children.Add(textBlock);
                    textBoxesDisasters.Add(textBlock);
                }
            }

        }

        private void EditPatient()
        {
            var sex = "";
            if ((bool)male.IsChecked)
            {
                sex = "one";
            }
            else if ((bool)female.IsChecked)
            {
                sex = "two";
            }
            else
            {
                MessageBox.Show("Please, choose gender", "Gender", MessageBoxButton.OK, MessageBoxImage.Hand);
            }

            if (sex!="")
            {
                var str = "{\n" + $"\"UserName\": \"{username.Text}\",\n" +
                          $"\"Gender\": \"{sex}\",\n" +
                          $"\"MedicineIds\": {JsonConvert.SerializeObject(medecine)},\n" +
                          $"\"DiseasesIds\": {JsonConvert.SerializeObject(diseases)},\n" +
                          $"\"Birthdate\": {JsonConvert.SerializeObject(date.SelectedDate.Value)}\n" + "}";
                var httpContent = new StringContent(str, Encoding.UTF8,
                                        "application/json");
                httpContent.Headers.ContentType.MediaType = "application/json";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainWindow.mainWindow.token);
                var response = client.PutAsync($"https://localhost:44374/api/v1/patient/{UserId}", httpContent);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    MainWindow.DoctorFrame.Content = new Page_DoctorInfo(MainWindow);
                }
                else
                {
                    MessageBox.Show("Something went wrong. Try again", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            }
        
        private void setMedicine()
        {
            var responseMedicine = client.GetAsync("https://localhost:44374/api/v1/medicines");
            responseMedicine.Wait();
            if (responseMedicine.Result.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<dynamic>(responseMedicine.Result.Content.ReadAsStringAsync().Result);
                var medicines = res.result;
                List<Medicine> medicinesdeserialized = new List<Medicine>();
                foreach (var item in medicines)
                {
                    var el = (Medicine)JsonConvert.DeserializeObject<Medicine>(item.ToString());
                    cb.Items.Add(el);
                    medicinesdeserialized.Add(el);
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
                List <Disease> diseasesdeserialized = new List<Disease>();
                foreach (var item in deseases)
                {
                    var desease = (Disease)JsonConvert.DeserializeObject<Disease>(item.ToString());
                    cbd.Items.Add(JsonConvert.DeserializeObject<Disease>(item.ToString()));
                    diseasesdeserialized.Add(desease);
                    
                }
            }
        }

        private void BtmEditPatient(object sender, RoutedEventArgs e)
        {
            EditPatient();
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
    }
}
