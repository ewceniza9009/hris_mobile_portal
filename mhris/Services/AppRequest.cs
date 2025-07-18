using mhris.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace mhris.Services
{
    internal class AppRequest
    {
        public static async Task<(bool, string)> IsLoginValid(string Code)
        {
            var result = false;
            var message = "Success";
            var requestUri = $@"{Global.UriBase}/MobileRepPayroll/IsLoginValid?Code={Code}";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                try
                {
                    var response = await client.GetStringAsync(requestUri);
                    result = JsonConvert.DeserializeObject<bool>(response);
                }
                catch (Exception ex)
                {
                    result = false;
                    message = ex.Message;
                }
            }

            return (result, message);
        }

        public static List<PayrollRecord> GetPayrollNumbers(string Code)
        {
            var result = new List<PayrollRecord>();
            var requestUri = $@"{Global.UriBase}/MobileRepPayroll/GetPayrollNumbers?Code={Code}";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                try
                {
                    var response = client.GetStringAsync(requestUri).Result;
                    result = JsonConvert.DeserializeObject<List<PayrollRecord>>(response);
                }
                catch
                {
                    result = new List<PayrollRecord>();
                }
            }

            return result;
        }

        public static async Task<List<PaySlipRecord>> GetPayslipByPayrollIdAndEmployeeCode(int PayrollId, string Code)
        {
            var result = new List<PaySlipRecord>();
            var requestUri = $@"{Global.UriBase}/MobileRepPayroll/GetPaySlip?PayrollId={PayrollId}&Code={Code}";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                try
                {
                    var response = await client.GetStringAsync(requestUri);
                    result = JsonConvert.DeserializeObject<List<PaySlipRecord>>(response);
                }
                catch
                {
                    result = new List<PaySlipRecord>();
                }
            }

            return result;
        }

        public static SerializeSettings GetDTRs(string Code)
        {
            var result = new SerializeSettings();
            var requestUri = $@"{Global.UriBase}/MobileRepPayroll/GetDTRs?Code={Code}";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                try
                {
                    var response = client.GetStringAsync(requestUri).Result;
                    result = JsonConvert.DeserializeObject<SerializeSettings>(response);
                }
                catch
                {
                    result = new SerializeSettings();
                }
            }

            return result;
        }

        public static async Task<List<DTRSlipRecord>> GetDTRSlipByDTRIdAndEmployeeCode(int DTRId, string Code) 
        {
            var result = new List<DTRSlipRecord>();
            var requestUri = $@"{Global.UriBase}/MobileRepPayroll/GetDTRSlip?DTRId={DTRId}&Code={Code}";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                try
                {
                    var response = await client.GetStringAsync(requestUri);
                    result = JsonConvert.DeserializeObject<List<DTRSlipRecord>>(response);
                }
                catch
                {
                    result = new List<DTRSlipRecord>();
                }
            }

            return result;
        }
    }
}
