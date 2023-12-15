using DeviceId;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace WApp1
{
    internal class RegistrationViewModel : IDataErrorInfo
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string MacID { get; set; } = "";
        public string LicenseKey { get; set; } = "";
        public string StaticIP { get; set; } = "";
        public string OSID { get; set; } = "";

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Email":
                        if (string.IsNullOrEmpty(Email))
                            return "Please enter Email.";
                        break;
                    case "Password":
                        if (string.IsNullOrEmpty(Password))
                            return "Please enter Password.";
                        break;
                    case "FirstName":
                        if (string.IsNullOrEmpty(FirstName))
                            return "Please enter FirstName.";
                        break;
                    case "LastName":
                        if (string.IsNullOrEmpty(LastName))
                            return "Please enter LastName.";
                        break;
                    case "CompanyName":
                        if (string.IsNullOrEmpty(CompanyName))
                            return "Please enter CompanyName.";
                        break;
                    case "PhoneNumber":
                        if (string.IsNullOrEmpty(PhoneNumber))
                            return "Please enter PhoneNumber.";
                        break;
                    case "MacID":
                        if (string.IsNullOrEmpty(MacID))
                            return "Please enter MacID.";
                        break;
                    case "LicenseKey":
                        if (string.IsNullOrEmpty(LicenseKey))
                            return "Please enter LicenseKey.";
                        break;
                    case "StaticIP":
                        if (string.IsNullOrEmpty(StaticIP))
                            return "Please enter StaticIP.";
                        break;
                    case "OSID":
                        if (string.IsNullOrEmpty(OSID))
                            return "Please enter OSID.";
                        break;
                }
                return string.Empty;
            }
        }

        internal bool IsValid()
        {
            bool result = true;
            if (string.IsNullOrEmpty(Email)
                || string.IsNullOrEmpty(Password)
                || string.IsNullOrEmpty(FirstName)
                || string.IsNullOrEmpty(LastName)
                || string.IsNullOrEmpty(CompanyName)
                || string.IsNullOrEmpty(PhoneNumber)
                || string.IsNullOrEmpty(MacID)
                || string.IsNullOrEmpty(LicenseKey)
                || string.IsNullOrEmpty(StaticIP)
                || string.IsNullOrEmpty(OSID)
                )
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> PostAsync()
        {
            bool result = false;
            try
            {
                if (!IsValid())
                {
                    return result;
                }

                HttpClient httpClient = new HttpClient();

                using StringContent jsonContent = new(
                    JsonSerializer.Serialize(new
                    {
                        email = Email,
                        password = Password,
                        first_name = FirstName,
                        last_name = LastName,
                        company_name = CompanyName,
                        phone_number = PhoneNumber,
                        mac_id = MacID,
                        license_key = LicenseKey,
                        static_ip = StaticIP,
                        os_id = OSID
                    }),
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage response = await httpClient.PostAsync(
                    "https://api.tailortrix.com/rest/api/user/register/",
                    jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(errorResponse);
                }
                else
                {
                    //response.EnsureSuccessStatusCode().WriteRequestToConsole();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"{jsonResponse}\n");
                    MessageBox.Show(jsonResponse);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }


    }
}
