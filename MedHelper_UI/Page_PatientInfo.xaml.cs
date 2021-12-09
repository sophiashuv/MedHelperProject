using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<String> medicines = new List<string> { "Medicine1", "Medicine2", "Medicine3", "Medicine4", "Medicine5", "Medicine6", "Medicine7", "Medicine8", "Medicine9"};
        public List<Button> buttons;
        public StackPanel medicinePanel = new StackPanel();
        private int UserId;

        public static TextBlock CreateMedicineText(String text, string color, bool bold=true)
        {
            var medicineText = new TextBlock();
            medicineText.Text = text;
            medicineText.Margin = new Thickness(10, 0, 5, 0);
            medicineText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            if (bold) medicineText.FontWeight = FontWeights.Bold;
            return medicineText;
        }
        public PatientInfo(Page_Doctor mainWindow, int userId)
        {
            MainWindow = mainWindoe;
            InitializeComponent();
            buttons = new List<Button>(medicines.Count);

            var height = 30;
            for (int i = 0; i < medicines.Count(); i++)
            {
                buttons.Add(new Button());
                buttons[i].Content = medicines[i];
                buttons[i].Height = height;
                buttons[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF56C1CA"));
                buttons[i].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                buttons[i].Click += new RoutedEventHandler(medicine_Click);
                StackPP.Children.Add(buttons[i]);
            }
        }

        private void medicine_Click(object sender, RoutedEventArgs e)
        {
            StackPP.Children.Clear();
            var button_num = 0;
            var medicinePanel = new StackPanel();
            medicinePanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            var medicineName = CreateMedicineText("Medicine Name: ", "#FFED635E");
            var medicineName2 = CreateMedicineText("Medicine ", "#000000", false);
            var StackPName = new StackPanel();
            StackPName.Orientation = Orientation.Horizontal;
            StackPName.Children.Add(medicineName);
            StackPName.Children.Add(medicineName2);
            medicinePanel.Children.Add(StackPName);

            var medicineGroup = CreateMedicineText("Medicine Group: ", "#FFED635E");
            var medicineGroup2 = CreateMedicineText("Group ", "#000000", false);
            var StackPGroup = new StackPanel();
            StackPGroup.Orientation = Orientation.Horizontal;
            StackPGroup.Children.Add(medicineGroup);
            StackPGroup.Children.Add(medicineGroup2);
            medicinePanel.Children.Add(StackPGroup);

            var composition = new List<String> { "Composition1", "Composition2", "Composition3" };
            var compositionString = Regex.Replace(String.Join(", ", composition), ".{27}", "$0\n");
            var medicineComposition = CreateMedicineText("Medicine Composition: ", "#FFED635E");
            var medicineComposition2 = CreateMedicineText(compositionString, "#000000", false);
            var StackPComposition = new StackPanel();
            StackPComposition.Orientation = Orientation.Horizontal;
            StackPComposition.Children.Add(medicineComposition);
            StackPComposition.Children.Add(medicineComposition2);
            medicinePanel.Children.Add(StackPComposition);

            var interaction = new List<String> { "Interaction1", "Interaction2", "Interaction3" };
            var interactionString = Regex.Replace(String.Join(", ", interaction), ".{33}", "$0\n");
            var medicineInteraction = CreateMedicineText("Medicine Interaction: ", "#FFED635E");
            var medicineInteraction2 = CreateMedicineText(interactionString, "#000000", false);
            var StackPInteraction = new StackPanel();
            StackPInteraction.Orientation = Orientation.Horizontal;
            StackPInteraction.Children.Add(medicineInteraction);
            StackPInteraction.Children.Add(medicineInteraction2);
            medicinePanel.Children.Add(StackPInteraction);

            var сontraindication = new List<String> { "Contraindication1", "Contraindication2", "Contraindication3" };
            var сontraindicationString = Regex.Replace(String.Join(", ", сontraindication), ".{25}", "$0\n");
            var medicineContraindication = CreateMedicineText("Medicine Contraindication: ", "#FFED635E");
            var medicineContraindication2 = CreateMedicineText(сontraindicationString, "#000000", false);
            var StackPContraindication = new StackPanel();
            StackPContraindication.Orientation = Orientation.Horizontal;
            StackPContraindication.Children.Add(medicineContraindication);
            StackPContraindication.Children.Add(medicineContraindication2);
            medicinePanel.Children.Add(StackPContraindication);

            for (int i = 0; i < button_num + 1; i++)
            {
                StackPP.Children.Add(buttons[i]);
            }

            StackPP.Children.Add(medicinePanel);

            for (int i = button_num + 1; i < medicines.Count(); i++)
            {
                StackPP.Children.Add(buttons[i]);
            }

        }

        private void BtmEditPatientClick(object sender, RoutedEventArgs e)
        {
            MainWindow.DoctorFrame.Content = new Page_EditPatient(MainWindow);
        }
    }
}
