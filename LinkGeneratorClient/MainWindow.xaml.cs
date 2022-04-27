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
using LinkGeneratorCommon;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Notify.Client;
using Notify.Exceptions;
using Notify.Models.Responses;
using QMSUK.DigitalHealthCheck.Encryption;
using RestSharp;

namespace LinkGeneratorClient
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadConfiguration();
        }

        static void EncryptConfig()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            AppSettingsSection section = (AppSettingsSection)config.GetSection("appSettings");

            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                config.Save();
            }
        }

        static void DecryptConfig()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            AppSettingsSection section = (AppSettingsSection)config.GetSection("appSettings");

            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
        }

        private void LoadConfiguration()
        {
            EncryptConfig();

            aesHexKey = ConfigurationManager.AppSettings["AesHexKey"];
            linkFormat = ConfigurationManager.AppSettings["LinkFormat"];
            minimumAge = int.Parse(ConfigurationManager.AppSettings["MinimumPatientAge"]);
            useProxy = bool.Parse(ConfigurationManager.AppSettings["UseProxy"]);
            govNotifyApiKey = ConfigurationManager.AppSettings["NotifyAPI"];
            govNotifytemplateId = ConfigurationManager.AppSettings["NotifyTemplateId"];
            bitlyToken = ConfigurationManager.AppSettings["BitlyToken"];
            bitlyGroup = ConfigurationManager.AppSettings["BitlyGroup"];
        }

        string aesHexKey = "";
        string linkFormat = "<digitalHealthCheckURL>/DigitalHealthCheck2_DEMO/?id={0}&hash={1}";
        
        int minimumAge = 30;

        bool useProxy;
        string govNotifyApiKey = "foo";
        string govNotifytemplateId = "bar";
        
        string bitlyToken = "foo";
        string bitlyGroup = "foo";

        class BitlyResponse
        {
            public string link { get; set; }

            public string id { get; set; }

            public string long_url { get; set; }
        }

        public IList<CredentialsOutputFileRecord> Links { get; set; } = new List<CredentialsOutputFileRecord>();

        private void ChooseInputFile_Click(object sender, RoutedEventArgs e)
        {
            InputFileValidationError.Visibility = Visibility.Collapsed;
            InputFileValidationFine.Visibility = Visibility.Collapsed;

            GenerateLinks.Visibility = Visibility.Collapsed;
            GeneratorError.Visibility = Visibility.Collapsed;
            GeneratorFine.Visibility = Visibility.Collapsed;
            GeneratedLinks.Visibility = Visibility.Collapsed;

            SaveGeneratedLinks.Visibility = Visibility.Collapsed;
            SendInvites.Visibility = Visibility.Collapsed;

            SendMessageError.Visibility = Visibility.Collapsed;
            SendMessageFine.Visibility = Visibility.Collapsed;
            

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
                    Links = new CredentialsFileLoader().LoadCredentials(openFileDialog.FileName)
                        .Select(x => new CredentialsOutputFileRecord(x)).ToList();

                    GeneratedLinks.ItemsSource = Links;

                    InputFilePath.Text = openFileDialog.FileName;


                    var errors = Validate(Links);

                    if(errors.Any())
                    {
                        InputFileValidationError.Text = string.Join(". ", errors);
                        InputFileValidationError.Visibility = Visibility.Visible;
                        InputFileValidationFine.Visibility = Visibility.Collapsed;
                        GenerateLinks.Visibility = Visibility.Collapsed;
                        GeneratedLinks.Visibility = Visibility.Collapsed;

                        return;
                    }
                }
                catch (FileNotFoundException)
                {
                    InputFileValidationError.Text = "The file could not be loaded.";
                    InputFileValidationError.Visibility = Visibility.Visible;
                    InputFileValidationFine.Visibility = Visibility.Collapsed;
                    GenerateLinks.Visibility = Visibility.Collapsed;
                    GeneratedLinks.Visibility = Visibility.Collapsed;

                    return;
                }
                catch(FormatException)
                {
                    InputFileValidationError.Text = "There was an error loading the file. Check that it is in the correct format and try again.";
                    InputFileValidationError.Visibility = Visibility.Visible;
                    InputFileValidationFine.Visibility = Visibility.Collapsed;
                    GenerateLinks.Visibility = Visibility.Collapsed;
                    GeneratedLinks.Visibility = Visibility.Collapsed;

                    return;
                }
                catch (HeaderValidationException ex)
                {
                    InputFileValidationError.Text = $"There was an error reading the file, one or more headers was incorrect. Check that it is in the correct format and try again.{Environment.NewLine}{ex.Message}";
                    InputFileValidationError.Visibility = Visibility.Visible;
                    InputFileValidationFine.Visibility = Visibility.Collapsed;
                    GenerateLinks.Visibility = Visibility.Collapsed;
                    GeneratedLinks.Visibility = Visibility.Collapsed;

                    return;
                }
                catch (Exception ex)
                {
                    InputFileValidationError.Text = $"There was an unexpected error loading the file. Check that it is in the correct format and try again.{Environment.NewLine}{ex.Message}";
                    InputFileValidationError.Visibility = Visibility.Visible;
                    InputFileValidationFine.Visibility = Visibility.Collapsed;
                    GenerateLinks.Visibility = Visibility.Collapsed;
                    GeneratedLinks.Visibility = Visibility.Collapsed;

                    return;
                }


                InputFileValidationError.Visibility = Visibility.Collapsed;
                InputFileValidationFine.Visibility = Visibility.Visible;
                GenerateLinks.Visibility = Visibility.Visible;
                GeneratedLinks.Visibility = Visibility.Visible;

                GeneratedLinks.ItemsSource = Links;
            }
        }

        NotificationClient CreateNotifyClient()
        {
            if(useProxy)
            {
                var httpClientWithProxy = new HttpClientWrapper(new HttpClient());
                return new NotificationClient(httpClientWithProxy, govNotifyApiKey);
            }

            return new NotificationClient(govNotifyApiKey);
        }

        async Task SendMessages()
        {
            NotificationClient client;

            try
            {
                client = CreateNotifyClient();
            }
            catch(NotifyAuthException)
            {
                SendMessageError.Text = "Couldn't connect to GOV.Notify. Make sure the API key is set in the .config file and correct.";
                SendMessageError.Visibility = Visibility.Visible;
                return;
            }
            

            async Task SendSMS(string templateId, string smsNumber, Dictionary<string,dynamic> personalisation)
            {
                var response = await client.SendSmsAsync(smsNumber, templateId, personalisation);
            }

            var errors = new Collection<string>();

            int id = 1;

            //foreach(var link in Links)
            //{
            //    try
            //    {
            //        await SendSMS(govNotifytemplateId, link.SmsNumber, new Dictionary<string, dynamic>()
            //        {
            //            {"firstName", link.FirstName },
            //            {"surname", link.Surname },
            //            {"gpSurgery", link.GPSurgery },
            //            {"url", link.Url }
            //        }).ConfigureAwait(false);
            //    }
            //    catch(NotifyClientException ex)
            //    {
            //        errors.Add($"Error sending message {id}: {ex.Message}");
            //    }

            //    id++;
            //}

            if(errors.Any())
            {
                Dispatcher.Invoke(() =>
                {
                    SendMessageError.Text = string.Join(",", errors);
                    SendMessageError.Visibility = Visibility.Visible;
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    SendMessageFine.Visibility = Visibility.Visible;
                });
            }
        }
        IEnumerable<string> Validate(IEnumerable<CredentialsOutputFileRecord> links)
        {
            var i = 1;

            foreach(var link in links)
            {
                if (string.IsNullOrWhiteSpace(link.Surname))
                {
                    yield return $"Patient {i} is missing a surname.";
                }

                if (string.IsNullOrWhiteSpace(link.Postcode))
                {
                    yield return $"Patient {i} ({link.Surname}) is missing a postcode.";
                }

                if (string.IsNullOrWhiteSpace(link.NHSNumber))
                {
                    yield return $"Patient {i} ({link.Surname}) is missing an NHS number.";
                }

                //if (string.IsNullOrWhiteSpace(link.SmsNumber))
                //{
                //    yield return $"Patient {i} ({link.Surname}) is missing an SMS number.";
                //}

                //if(!link.SmsNumber.IsValidPhoneNumber())
                //{
                //    yield return $"Patient {i} ({link.Surname}) has an invalid SMS number.";
                //}

                if(link.DateOfBirth < new DateTime(1900,01,01) || link.DateOfBirth > DateTime.Today)
                {
                    yield return $"Patient {i} ({link.Surname}) has an invalid date of birth.";
                }
                
                if(link.DateOfBirth > DateTime.Today.AddYears(-minimumAge))
                {
                    yield return $"Patient {i} ({link.Surname}) is less than {minimumAge} years old.";
                }

                if (string.IsNullOrWhiteSpace(link.GPEmail))
                {
                    yield return $"Patient {i} ({link.Surname}) is missing a GP email address.";
                }

                if (string.IsNullOrWhiteSpace(link.GPSurgery))
                {
                    yield return $"Patient {i} ({link.Surname}) is missing a GP surgery name.";
                }

                i++;
            }
        }

        

        async Task ShortenLinks()
        {
            var options = new RestClientOptions("https://api-ssl.bitly.com/v4/shorten")
            {
                ThrowOnAnyError = false
            };

            var client = new RestClient(options)
                .AddDefaultHeader("Authorization", $"Bearer {bitlyToken}");

            async Task<string> Shorten(string url)
            {
                var request = new RestRequest()
                    .AddJsonBody(new
                    {
                        long_url = url,
                        domain = "bit.ly",
                        group_guid = bitlyGroup
                    });

                var response = await client.ExecutePostAsync(request);

                if(response.ErrorException is not null)
                {
                    throw response.ErrorException;
                }

                return JObject.Parse(response.Content).Value<string>("link");
            }

            foreach(var link in Links)
            {
                link.Url = await Shorten(link.Url);
            }
        }


        private async void GenerateLinks_Click(object sender, RoutedEventArgs e)
        {
            //GenerateLinks.IsEnabled = false;

            GeneratorError.Visibility = Visibility.Collapsed;
            GeneratorFine.Visibility = Visibility.Collapsed;

            SaveGeneratedLinks.Visibility = Visibility.Collapsed;
            SendInvites.Visibility = Visibility.Collapsed;

            SendMessageError.Visibility = Visibility.Collapsed;
            SendMessageFine.Visibility = Visibility.Collapsed;

            var linkGenerator = new LinkGenerator(
                new HealthCheckCredentialsEncrypter(new UrlOptimisedAesEncrypter(aesHexKey), new NewtonsoftJsonSerializationWrapper<Credentials>()),
                linkFormat
            );


            foreach (var credential in Links)
            {
                //Manually create a new Credentials class here, or JSON.NET will include a bunch of fields we don't want.

                var credentials = new Credentials()
                {
                    DateOfBirth = credential.DateOfBirth,
                    GPEmail = credential.GPEmail,
                    GPSurgery = credential.GPSurgery,
                    NHSNumber = credential.NHSNumber,
                    Postcode = credential.Postcode,
                    Surname = credential.Surname
                };

                credential.Url = linkGenerator.GenerateLink(Guid.NewGuid(), credentials);
            }

            try
            {
                await ShortenLinks();
            }
            catch(Exception ex)
            {
                GeneratorError.Text = $"There was an error shortening links. Double check the Bitly configuration and try again. Full error: {ex.Message}";
                GeneratorError.Visibility = Visibility.Visible;
                GeneratorFine.Visibility = Visibility.Collapsed;
                GenerateLinks.IsEnabled = true;
                return;
            }

            GeneratorError.Visibility = Visibility.Collapsed;
            GeneratorFine.Visibility = Visibility.Visible;

            SaveGeneratedLinks.Visibility = Visibility.Visible;
            SendInvites.Visibility = Visibility.Visible;

            //SendInvites.IsEnabled = true;

            GeneratedLinks.Items.Refresh();

            GenerateLinks.IsEnabled = true;
        }

        private void SaveGeneratedLinks_Click(object sender, RoutedEventArgs e)
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

                csv.Context.RegisterClassMap<CredentialsOutputMap>();

                csv.WriteRecords(Links);
            }
        }

        private async void SendInvites_Click(object sender, RoutedEventArgs e)
        {
            SendMessageError.Visibility = Visibility.Collapsed;
            SendMessageFine.Visibility = Visibility.Collapsed;
            SendInvites.IsEnabled = false;

            await SendMessages();

            //SendInvites.IsEnabled = true;
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            DecryptConfig();

            MessageBox.Show("The configuration file has been successfully decrypted. Once you have made your changes, please restart the application to automatically re-encrypt the configuration file and reload your changes.");
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            EncryptConfig();

            MessageBox.Show("The configuration file has been successfully encrypted.");
        }
    }
}
