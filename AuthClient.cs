using LocoDotnet.Internal;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LocoDotnet
{
    public class AuthClient
    {
        private HttpClient GetHttpClient(Account account)
        {
            HttpClient client = new HttpClient();
            string userAgent = this.GetUserAgent(Config.VERSION, "Wd", Config.OS_VERSION, Config.LANGUAGE);

            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("A", this.GetA(Config.AGENT, Config.VERSION, Config.LANGUAGE));
            client.DefaultRequestHeaders.Add("X-VC", this.GetXVC(userAgent, account.email, account.deviceUUID));
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("Accept-Language", Config.LANGUAGE);

            return client;
        }

        private FormUrlEncodedContent GetBody(Account account, string passcode = "")
        {
            var body = new Dictionary<string, string>();
            body.Add("email", account.email);
            body.Add("password", account.password);
            body.Add("device_uuid", account.deviceUUID);
            body.Add("os_version", account.osVersion);
            body.Add("device_name", account.deviceName);
            if (String.IsNullOrEmpty(passcode))
                body.Add("passcode", passcode);
            body.Add("permanent", account.permanent ? "true" : "false");
            body.Add("forced", account.permanent ? "true" : "false");

            return new FormUrlEncodedContent(body);
        }

        public async Task<string> Login(Account account)
        {
            HttpClient client = GetHttpClient(account);
            Task<HttpResponseMessage> responseTask = client.PostAsync($"https://{Config.AUTH_HOST}/{Config.AGENT}/account/login.json", GetBody(account));
            HttpResponseMessage response = await responseTask;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RequestPasscode(Account account)
        {
            HttpClient client = GetHttpClient(account);
            Task<HttpResponseMessage> responseTask = client.PostAsync($"https://{Config.AUTH_HOST}/{Config.AGENT}/account/request_passcode.json", GetBody(account));
            HttpResponseMessage response = await responseTask;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RegisterDevice(Account account, string passcode)
        {
            HttpClient client = GetHttpClient(account);
            Task<HttpResponseMessage> responseTask = client.PostAsync($"https://{Config.AUTH_HOST}/{Config.AGENT}/account/register_device.json", GetBody(account, passcode));
            HttpResponseMessage response = await responseTask;

            return await response.Content.ReadAsStringAsync();
        }

        private string GetA(string agent, string version, string language)
        {
            return $"{agent}/{version}/{language}";
        }

        private string GetUserAgent(string version, string platform, string osVersion, string language)
        {
            return $"KT/{version} {platform}/{osVersion} {language}";
        }

        private string GetXVC(string userAgent, string email, string deviceUUID)
        {
            byte[] plainText = Encoding.UTF8.GetBytes($"HEATH|{userAgent}|DEMIAN|{email}|{deviceUUID}");
            byte[] hashedText = new SHA512Managed().ComputeHash(plainText);

            return BitConverter.ToString(hashedText).Replace("-", "").Substring(0, 16);
        }
    }
}