using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
using CsvHelper;
using CsvHelper.Configuration;
using DigitalHealthCheckCommon;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace QRiskEstimator
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        int minimumAge = 30;

        public IList<OutputFileRecord> Patients { get; set; } = new List<OutputFileRecord>();

        private void ChooseInputFile_Click(object sender, RoutedEventArgs e)
        {
            InputFileValidationError.Visibility = Visibility.Collapsed;
            InputFileValidationFine.Visibility = Visibility.Collapsed;

            GenerateEstimates.Visibility = Visibility.Collapsed;
            GeneratorError.Visibility = Visibility.Collapsed;
            GeneratorFine.Visibility = Visibility.Collapsed;
            GeneratedEstimates.Visibility = Visibility.Collapsed;

            SaveGeneratedEstimates.Visibility = Visibility.Collapsed;           

            var openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = "csv";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {

                try
                {
                    Patients = new PatientsFileLoader().LoadPatients(openFileDialog.FileName).ToList();

                    GeneratedEstimates.ItemsSource = Patients;

                    InputFilePath.Text = openFileDialog.FileName;

                    var errors = Validate(Patients);

                    if(errors.Any())
                    {
                        InputFileValidationError.Text = string.Join(". ", errors);
                        InputFileValidationError.Visibility = Visibility.Visible;
                        InputFileValidationFine.Visibility = Visibility.Collapsed;
                        GenerateEstimates.Visibility = Visibility.Collapsed;
                        GeneratedEstimates.Visibility = Visibility.Collapsed;

                        return;
                    }
                }
                catch (FileNotFoundException)
                {
                    InputFileValidationError.Text = "The file could not be loaded check it exists and you have access to it and try again.";
                    InputFileValidationError.Visibility = Visibility.Visible;
                    InputFileValidationFine.Visibility = Visibility.Collapsed;
                    GenerateEstimates.Visibility = Visibility.Collapsed;
                    GeneratedEstimates.Visibility = Visibility.Collapsed;

                    return;
                }
                catch(FormatException ex)
                {
                    InputFileValidationError.Text = "There was an error loading the file. Check that it is in the correct format and try again.";
                    InputFileValidationError.Visibility = Visibility.Visible;
                    InputFileValidationFine.Visibility = Visibility.Collapsed;
                    GenerateEstimates.Visibility = Visibility.Collapsed;
                    GeneratedEstimates.Visibility = Visibility.Collapsed;

                    return;
                }
                catch (HeaderValidationException ex)
                {
                    InputFileValidationError.Text = $"There was an error reading the file, one or more headers was incorrect. Check that it is in the correct format and try again.{Environment.NewLine}{ex.Message}";
                    InputFileValidationError.Visibility = Visibility.Visible;
                    InputFileValidationFine.Visibility = Visibility.Collapsed;
                    GenerateEstimates.Visibility = Visibility.Collapsed;
                    GeneratedEstimates.Visibility = Visibility.Collapsed;

                    return;
                }
                catch (Exception ex)
                {
                    InputFileValidationError.Text = $"There was an unexpected error loading the file. Check that it is in the correct format and try again.{Environment.NewLine}{ex.Message}";
                    InputFileValidationError.Visibility = Visibility.Visible;
                    InputFileValidationFine.Visibility = Visibility.Collapsed;
                    GenerateEstimates.Visibility = Visibility.Collapsed;
                    GeneratedEstimates.Visibility = Visibility.Collapsed;

                    return;
                }


                InputFileValidationError.Visibility = Visibility.Collapsed;
                InputFileValidationFine.Visibility = Visibility.Visible;
                GenerateEstimates.Visibility = Visibility.Visible;
                GeneratedEstimates.Visibility = Visibility.Visible;

                GeneratedEstimates.ItemsSource = Patients;
            }
        }

        IEnumerable<string> Validate(IEnumerable<OutputFileRecord> patients)
        {
            var i = 1;

            foreach(var patient in patients)
            {
                if (string.IsNullOrWhiteSpace(patient.NHSNumber))
                {
                    yield return $"Patient {i} is missing an NHS number.";
                }

                if (string.IsNullOrWhiteSpace(patient.Postcode))
                {
                    yield return $"Patient {i} ({patient.NHSNumber}) is missing a postcode.";
                }
                
                if(patient.Age < 30)
                {
                    yield return $"Patient {i} ({patient.NHSNumber}) is younger than 30.";
                }

                if(string.IsNullOrEmpty(patient.UniqueLink))
                {
                    yield return $"Patient {i} ({patient.NHSNumber}) is missing a unique link.";
                }

                i++;
            }
        }

        
        IDictionary<string, double> townsendValues = new Dictionary<string,double>(StringComparer.OrdinalIgnoreCase);

        public class TownsendRecord
        {
            public string Postcode {get;set;}
            public double Score {get;set;}
        }

        void LoadTownsend()
        {
            using (var reader = new StreamReader("townsend.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                townsendValues = csv.GetRecords<TownsendRecord>().ToDictionary(x=> x.Postcode, x=> x.Score);
            }
        }

        double TownsendLookup(string postcode)
        {
            if(townsendValues.TryGetValue(postcode.Replace(" ", ""),out var score))
            {
                return score;
            }

            return 0d;
        }

        private void GenerateEstimates_Click(object sender, RoutedEventArgs e)
        {
            GenerateEstimates.IsEnabled = false;

            try
            {
                GeneratorError.Visibility = Visibility.Collapsed;
                GeneratorFine.Visibility = Visibility.Collapsed;

                SaveGeneratedEstimates.Visibility = Visibility.Collapsed;

                LoadTownsend();

                var riskScoreCalculator = new QMSRiskCalculator(new BodyMassIndexCalculator());

                foreach (var patient in Patients)
                {
                    var townsend = TownsendLookup(patient.Postcode);

                    var qrisk = riskScoreCalculator.Calculate10YearCVDRiskScore(patient,townsend);

                    patient.QRISK = qrisk;
                }

                GeneratorError.Visibility = Visibility.Collapsed;
                GeneratorFine.Visibility = Visibility.Visible;

                SaveGeneratedEstimates.Visibility = Visibility.Visible;

                GeneratedEstimates.Items.Refresh();
            }
            finally
            {
                GenerateEstimates.IsEnabled = true;
            }
        }

        private void SaveGeneratedEstimates_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = args => args.Header.ToLower(),
                };

                using var writer = new StreamWriter(saveFileDialog.FileName);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                csv.WriteRecords(Patients);
            }
        }
    }
}
