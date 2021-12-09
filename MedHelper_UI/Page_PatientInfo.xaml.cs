using MedHelper_EF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for PatientInfo.xaml
    /// </summary>
    public partial class PatientInfo : Page
    {
        public Page_Doctor MainWindow;
        private HttpClient client = new HttpClient();
        public List<String> medicines = new List<string> { "Medecine1", "Medecine2", "Medekine3", "Medekine4", "Medekine5", "Medicine6", "Medicine7", "Medicine8", "Medicine9"};
        public List<String> found = new List<string>();
        public List<Button> buttons;
        public StackPanel medicinePanel = new StackPanel();
        private int UserId;
        public PatientInfo(Page_Doctor mainWindow, int userId)
        {
            UserId = userId;
            MainWindow = mainWindow;
            InitializeComponent();
            buttons = new List<Button>(medicines.Count);
            setInformation();
            FindResults.TextChanged += SearchEvent;

            //for (int i = 0; i < medicines.Count(); i++)
            //{
            //    buttons.Add(new Button());
            //    buttons[i].Content = medicines[i];
            //    buttons[i].Height = height;
            //    buttons[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF56C1CA"));
            //    buttons[i].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            //    buttons[i].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            //    buttons[i].Click += new RoutedEventHandler(medicine_Click);
            //}
            //StackPP.Children.Clear();
            //var button_num = 0;

            //medicinePanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            //var medicineText = new TextBlock();
            //var medicineDescription = "Не слід порушувати рекомендації щодо застосування лікарського засобу ‒ це може зашкодити здоров’ю. " +
            //    "Не слід перевищувати рекомендовані дози лікарського засобу. " +
            //    "При застосуванні препарату слід утримуватися від вживання алкоголю. " +
            //    "Препарат містить етанол.Не рекомендується застосування лікарського засобу протягом тривалого часу." +
            //    " Валеріана може виявляти помірний депресивний ефект, тому не рекомендується сумісний прийом лікарського " +
            //    "засобу із синтетичними седативними засобами через потенціювання ефекту. " +
            //    "При вираженому атеросклерозі мозкових судин препарат можна застосовувати тільки під контролем лікаря. " +
            //    "У процесі зберігання допускається випадання осаду. Перед застосуванням препарат слід збовтувати. " +
            //    "Валеріанка - Вішфа є традиційним лікарським засобом для використання відповідно до показань, підтверджених тривалим застосуванням. " +
            //    "Застосування у період вагітності або годування груддю " +
            //    "Препарат не застосовувати у період вагітності або годування груддю через вміст етанолу 70 %. " +
            //    "Здатність впливати на швидкість реакції при керуванні автотранспортом або іншими механізмами. " +
            //    "На період лікування препаратом слід утримуватися від керування транспортними засобами та роботи з потенційно ‒ небезпечними механізмами через вміст етанолу 70 % та можливість розвитку побічних ефектів з боку центральної нервової системи.";

            //medicineDescription = Regex.Replace(medicineDescription, ".{50}", "$0\n");
            //medicineText.Text = medicineDescription;
            //medicineText.Margin = new Thickness(10, 0, 5, 0);
            //medicineText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFED635E"));
            //medicinePanel.Children.Add(medicineText);
            //for (int i = 0; i < medicines.Count(); i++)
            //{
            //    StackPP.Children.Add(buttons[i]);
            //}

            //StackPP.Children.Add(medicinePanel);

            //for (int i = button_num + 1; i < medicines.Count(); i++)
            //{
            //    StackPP.Children.Add(buttons[i]);
            //}
        }

        private void SearchEvent(object sender, TextChangedEventArgs e)
        {
            Search(FindResults.Text);
        }

        private void medicine_Click(object sender, RoutedEventArgs e)
        {
            
            if(medicinePanel.Visibility==System.Windows.Visibility.Visible)
            {
                medicinePanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                medicinePanel.Visibility = System.Windows.Visibility.Visible;
            }
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
                Age.Text = (new DateTime((DateTime.Now - Convert.ToDateTime(res.result.birthdate)).Ticks).Year - 1).ToString();
                if (res.result.gender == "one")
                {
                    Gender.Text = "Male";
                }
                else
                {
                    Gender.Text = "Female";
                }
                var counter = 1;
                foreach (var item in res.result.medicines)
                {
                    var stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    var number = CreateNumberBlock($"{counter}. ");
                    var medicineName = CreateMedicineBlock(Convert.ToString(item.name));
                    stackPanel.Children.Add(number);
                    stackPanel.Children.Add(medicineName);
                    MedicineList.Children.Add(stackPanel);
                    counter++;
                }
                counter = 1;
                foreach (var item in res.result.diseases)
                {
                    var stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    var number = CreateNumberBlock($"{counter}. ");
                    var medicineName = CreateMedicineBlock(Convert.ToString(item.name));
                    stackPanel.Children.Add(number);
                    stackPanel.Children.Add(medicineName);
                    DiseaseList.Children.Add(stackPanel);
                    counter++;
                }
            }

        }
        public static TextBlock CreateMedicineBlock(String text)
        {
            TextBlock textBolock = new TextBlock();
            textBolock.Text = text;
            textBolock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            return textBolock;
        }
        public static TextBlock CreateNumberBlock(String text)
        {
            TextBlock textBolock = new TextBlock();
            textBolock.Text = text;
            textBolock.FontWeight = FontWeight.FromOpenTypeWeight(700);
            textBolock.Margin = new Thickness(10, 0, 10, 0);
            textBolock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            return textBolock;
        }

        private void Search(string text)
        {
            found.Clear();
            foreach (var item in medicines)
            {
                if (item.Contains(text))
                {
                    found.Add(item);
                }
            }

            var height = 30;
            for (int i = 0; i < found.Count(); i++)
            {
                buttons.Add(new Button());
                buttons[i].Content = found[i];
                buttons[i].Height = height;
                buttons[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF56C1CA"));
                buttons[i].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].Click += new RoutedEventHandler(medicine_Click);
            }
            StackPP.Children.Clear();

            for (int i = 0; i < found.Count(); i++)
            {
                StackPP.Children.Add(buttons[i]);
            }
        }
        private void BtmEditPatientClick(object sender, RoutedEventArgs e)
        {
            MainWindow.DoctorFrame.Content = new Page_EditPatient(MainWindow,UserId);
        }
    }
}
