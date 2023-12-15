using DeviceId;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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
using System.Windows.Shapes;

namespace WApp1
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        readonly string SALT = "4yJmEsPGObWvXEbcLL7qS0Lb5qe6J4";

        public Registration()
        {
            InitializeComponent();
            //PostAsync();

            //var externalIpTask = GetExternalIpAddress();
            //GetExternalIpAddress().Wait();
            //var externalIpString = externalIpTask.Result ?? IPAddress.Loopback;
            DataContext = new RegistrationViewModel();




        }



        private void register_Click(object sender, RoutedEventArgs e)
        {
            ((RegistrationViewModel)DataContext).PostAsync();
        }

        private void license_key_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private string GetMacAddress()
        {
            return new DeviceIdBuilder()
                .AddMacAddress().ToString();
        }

        private string GetOSID()
        {
            return Environment.OSVersion.ToString();
        }

        static string GetIPAddress()
        {
            String address = "";
            HttpClient httpClient = new HttpClient();
            address = httpClient.GetStringAsync("http://checkip.dyndns.org/").Result;

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);
            return address;
        }

        private string GenerateLicenseKey()
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(macid.Text) && !string.IsNullOrEmpty(os_id.Text))
            {
                string data = $"{macid.Text + phone_number.Text + SALT + os_id.Text}";
                SHA512 sha = SHA512.Create("SHA512");
                byte[] hashData = sha.ComputeHash(Encoding.Default.GetBytes(data));
                StringBuilder returnValue = new StringBuilder();
                for (int i = 0; i < hashData.Length; i++)
                {
                    returnValue.Append(hashData[i].ToString("x2"));
                }

                result = returnValue.ToString();
            }

            return result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            macid.Text = GetMacAddress();
            static_ip.Text = GetIPAddress();
            os_id.Text = GetOSID();
            license_key.Text = GenerateLicenseKey();
        }
    }
}
