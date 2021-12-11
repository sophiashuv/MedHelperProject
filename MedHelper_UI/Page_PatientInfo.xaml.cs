using MedHelper_EF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedHelper_UI
{
    /// <summary>
    /// Interaction logic for PatientInfo.xaml
    /// </summary>
    public partial class PatientInfo : Page
    {
        public Page_Doctor MainWindow;
        private HttpClient client = new HttpClient();
        public List<Medicine> medicines = new List<Medicine>();
        public List<Medicine> found = new List<Medicine>();
        public List<Button> buttons;
        private int UserId;

        public PatientInfo(Page_Doctor mainWindow, int userId)
        {
            UserId = userId;
            MainWindow = mainWindow;
            InitializeComponent();
            buttons = new List<Button>(medicines.Count);
            setInformation();
            FindResults.TextChanged += SearchEvent;
            Search("");
        }

        private void SearchEvent(object sender, TextChangedEventArgs e)
        {
            Search(FindResults.Text);
        }

        private void medicine_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var medicine = (Medicine)button.Content;
            var med = StackPP.FindName("desc2");
            var medicinePanel = FindChild<StackPanel>(StackPP, $"desc{medicine.MedicineID}");
            if (medicinePanel.Visibility == System.Windows.Visibility.Visible)
            {
                medicinePanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                medicinePanel.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public static TextBlock CreateMedicineText(String text, string color, bool bold = true)
        {
            var medicineText = new TextBlock();
            medicineText.Text = text;
            medicineText.Margin = new Thickness(10, 0, 5, 0);
            medicineText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            if (bold) medicineText.FontWeight = FontWeights.Bold;
            return medicineText;
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
                userTitle.Text = res.result.userName;
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
                    var medicineName = CreateMedicineBlock(Convert.ToString(item.title));
                    stackPanel.Children.Add(number);
                    stackPanel.Children.Add(medicineName);
                    DiseaseList.Children.Add(stackPanel);
                    counter++;
                }
            }

            var medicineResponse = client.GetAsync($"https://localhost:44374/api/v1/patient/{UserId}/allmedicines");
            medicineResponse.Wait();
            if (medicineResponse.Result.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(medicineResponse.Result.Content.ReadAsStringAsync().Result);
                foreach (var item in result.result)
                {
                    var medicineComp = new List<MedicineComposition>();
                    foreach (var comp in item.compositions)
                    {
                        medicineComp.Add(new MedicineComposition() { Composition = new Composition() { Description = comp } });
                    }

                    var medicineContr = new List<MedicineContraindication>();
                    foreach (var contr in item.contraindications)
                    {
                        medicineContr.Add(new MedicineContraindication() { Contraindication = new Contraindication() { Description = contr } });
                    }

                    var medicineInter = new List<MedicineInteraction>();
                    foreach (var inter in item.medicineInteractions)
                    {
                        medicineInter.Add(new MedicineInteraction()
                        {
                            Description = inter.description,
                            Composition = new Composition() { Description = inter.compositionDescription }
                        });
                    }

                    medicines.Add(new Medicine()
                    {
                        MedicineID = item.medicineID,
                        Name = item.name,
                        pharmacotherapeuticGroup = item.pharmacotherapeuticGroup,
                        MedicineCompositions = medicineComp,
                        MedicineContraindications = medicineContr,
                        MedicineInteractions = medicineInter
                    });
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

        private static StackPanel createDesc(Medicine m)
        {
            var medicinePanel = new StackPanel();
            medicinePanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            var medicineName = CreateMedicineText("Medicine Name: ", "#FFED635E");
            var medicineName2 = CreateMedicineText(m.Name, "#000000", false);
            var StackPName = new StackPanel();
            StackPName.Margin = new Thickness(0, 3, 0, 0);
            StackPName.Orientation = Orientation.Horizontal;
            StackPName.Children.Add(medicineName);
            StackPName.Children.Add(medicineName2);
            medicinePanel.Children.Add(StackPName);

            var medicineGroup = CreateMedicineText("Medicine Group: ", "#FFED635E");
            var medicineGroup2 = CreateMedicineText(m.pharmacotherapeuticGroup, "#000000", false);
            var StackPGroup = new StackPanel();
            StackPGroup.Orientation = Orientation.Horizontal;
            StackPGroup.Children.Add(medicineGroup);
            StackPGroup.Children.Add(medicineGroup2);
            medicinePanel.Children.Add(StackPGroup);

            var composition = new List<String> ();
            foreach (var item in m.MedicineCompositions)
            {
                composition.Add(item.Composition.Description);
            }
            var compositionString = Regex.Replace(String.Join(", ", composition), ".{27}", "$0\n");
            var medicineComposition = CreateMedicineText("Medicine Composition: ", "#FFED635E");
            var medicineComposition2 = CreateMedicineText(compositionString, "#000000", false);
            var StackPComposition = new StackPanel();
            StackPComposition.Orientation = Orientation.Horizontal;
            StackPComposition.Children.Add(medicineComposition);
            StackPComposition.Children.Add(medicineComposition2);
            medicinePanel.Children.Add(StackPComposition);

            var сontraindication = new List<String>();
            foreach (var item in m.MedicineContraindications)
            {
                сontraindication.Add(item.Contraindication.Description);
            }
            var сontraindicationString = Regex.Replace(String.Join(", ", сontraindication), ".{25}", "$0\n");
            var medicineContraindication = CreateMedicineText("Medicine Contraindication: ", "#FFED635E");
            var medicineContraindication2 = CreateMedicineText(сontraindicationString, "#000000", false);
            var StackPContraindication = new StackPanel();
            StackPContraindication.Orientation = Orientation.Horizontal;
            StackPContraindication.Children.Add(medicineContraindication);
            StackPContraindication.Children.Add(medicineContraindication2);
            medicinePanel.Children.Add(StackPContraindication);

            var interaction = new Dictionary<String, String>();
            foreach (var item in m.MedicineInteractions)
            {
                interaction.Add(item.Composition.Description, item.Description);
            }
            var medicineInteraction = CreateMedicineText("Medicine Interaction: ", "#FFED635E");
            var StackPInteraction = new StackPanel();
            StackPInteraction.Orientation = Orientation.Horizontal;
            StackPInteraction.Children.Add(medicineInteraction);
            medicinePanel.Children.Add(StackPInteraction);

            var ind = 1;
            var StackPInteractionAll = new StackPanel();
            StackPInteractionAll.Margin = new Thickness(0, 0, 0, 5);
            foreach (var (key, value) in interaction)
            {
                var medicineInteractionMedicine = CreateMedicineText($"{ind}. {Regex.Replace(key, ".{25}", "$0\n")}:", "#000000");
                var medicineInteractionInteraction = CreateMedicineText(Regex.Replace(value, ".{33}", "$0\n"), "#000000", false);
                StackPInteraction = new StackPanel();
                StackPInteraction.Orientation = Orientation.Horizontal;
                StackPInteraction.Children.Add(medicineInteractionMedicine);
                StackPInteraction.Children.Add(medicineInteractionInteraction);
                StackPInteractionAll.Children.Add(StackPInteraction);
                ind++;
            }
            medicinePanel.Children.Add(StackPInteractionAll);
            medicinePanel.Visibility = Visibility.Collapsed;
            medicinePanel.Name = $"desc{m.MedicineID}";
            return medicinePanel;
        }
        private void Search(string text)
        {
            found.Clear();
            found = medicines.Where(x => x.Name.ToLower().Contains(text.ToLower()) ||
            x.pharmacotherapeuticGroup.ToLower().Contains(text.ToLower())).ToList();

            var height = 30;
            buttons.Clear();
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
                StackPP.Children.Add(createDesc(found[i]));
            }
        }
        private void BtmEditPatientClick(object sender, RoutedEventArgs e)
        {
            MainWindow.DoctorFrame.Content = new Page_EditPatient(MainWindow, UserId);
        }


        private static T FindChild<T>(DependencyObject parent, string childName)
   where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);

                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
