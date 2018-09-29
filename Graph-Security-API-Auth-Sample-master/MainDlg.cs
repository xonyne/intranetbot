/* 
 * Copyright (c) Microsoft Corporation. All rights reserved. Licensed under the MIT license.
 * See LICENSE in the project root for license information.
 */
namespace GraphAuthSample
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Microsoft.Identity.Client;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Class for the main UI form
    /// </summary>
    public partial class MainDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainDlg"/> class. 
        /// </summary>
        public MainDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Cache of <see cref="PublicClientApplication"/> objects
        /// </summary>
        private readonly Dictionary<string, PublicClientApplication> MsalClientDict = new Dictionary<string, PublicClientApplication>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Load UI from settings
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.appKeyInput.Text = ConfigurationManager.AppSettings["DefaultAppKey"];
            this.FillInitComboBox(this.appIdInput, "DefaultAppId");
            this.FillInitComboBox(this.urlInput, "DefaultGraphUri");
            this.FillInitComboBox(this.tenantIdInput, "DefaultTenantId");
        }

        /// <summary>
        /// Get access token and send request to Graph. Called when "Send Request" button is clicked
        /// </summary>
        /// <param name="sender">UI sender</param>
        /// <param name="e">UI event</param>
        private async void OnSendRequest(object sender, EventArgs e)
        {
            try
            {
                var url = new Uri(this.urlInput.Text.Trim());
                AuthResult authResult = null;
                if (authMethodTabControl.SelectedIndex == 0)
                {
                    authResult = await this.GetUserDelegatedTokenAsync();
                }
                else if (authMethodTabControl.SelectedIndex == 1)
                {
                    authResult = await this.GetAppOnlyTokenAsync();
                }
                else
                {
                    MessageBox.Show("Please choose User Delegated or Application Only tab to choose authentication method");
                    return;
                }
                if (authResult.Exception != null)
                {
                    throw authResult.Exception;
                }

                this.ShowAccessToken(authResult.AccessToken);
                var result = await this.SendRequestToGraphAsync(url, HttpMethod.Get, authResult.AccessToken);
                this.ShowResponse(result);
            }
            catch(Exception err)
            {
                this.responseContentOutput.Text = err.ToString();
            }
        }

        /// <summary>
        /// Accoding to the doc at https://docs.microsoft.com/en-us/azure/active-directory/develop/guidedsetups/active-directory-windesktop
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<AuthResult> GetUserDelegatedTokenAsync()
        {
            var appId = appIdInput.Text.Trim();
            var scopes = new string[] { "User.Read" };
            AuthResult result = new AuthResult();
            PublicClientApplication client;
            if (!this.MsalClientDict.TryGetValue(appId, out client))
            {
                client = new PublicClientApplication(appId);
                this.MsalClientDict[appId] = client;
            }

            AuthenticationResult authResult = null;
            try
            {
                result.Logs.Add("Calling PublicClientApplication.AcquireTokenSilentAsync()");
                authResult = await client.AcquireTokenSilentAsync(scopes, client.Users.FirstOrDefault());
                result.Logs.Add("PublicClientApplication.AcquireTokenSilentAsync() succeeded");
            }
            catch (MsalUiRequiredException)
            {
                try
                {
                    result.Logs.Add("Calling PublicClientApplication.AcquireTokenAsync()");
                    authResult = await client.AcquireTokenAsync(scopes);
                    result.Logs.Add("PublicClientApplication.AcquireTokenAsync() succeeded");
                }
                catch (MsalException msalex)
                {
                    result.Logs.Add("PublicClientApplication.AcquireTokenAsync() failed");
                    result.Exception = msalex;
                }
            }
            catch (Exception ex)
            {
                result.Logs.Add("PublicClientApplication.AcquireTokenSilentAsync() failed");
                result.Exception = ex;
            }

            if (authResult != null)
            {
                result.AccessToken = authResult.AccessToken;
            }

            return result;
        }

        /// <summary>
        /// Accoding to the doc at https://docs.microsoft.com/en-us/azure/active-directory/develop/guidedsetups/active-directory-windesktop
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<AuthResult> GetAppOnlyTokenAsync()
        {
            var appId = appIdInput.Text.Trim();
            var appKey = appKeyInput.Text.Trim();
            var tenantId = tenantIdInput.Text.Trim();

            AuthResult result = new AuthResult();
            try
            {
                ConfidentialClientApplication clientApp = new ConfidentialClientApplication(
                    appId,
                    $"https://login.microsoftonline.com/{tenantId}/v2.0",
                    "http://localhost",
                    new ClientCredential(appKey),
                    null,
                    new TokenCache());
                AuthenticationResult authResult = await clientApp.AcquireTokenForClientAsync(new string[] { "https://graph.microsoft.com/.default" });
                result.AccessToken = authResult?.AccessToken;
            }
            catch (Exception err)
            {
                result.Exception = err;
            }

            return result;
        }

        /// <summary>
        /// Fill combobox from settings
        /// </summary>
        /// <param name="uiItem">The combobox UI object</param>
        /// <param name="configName">The config name</param>
        private void FillInitComboBox(ComboBox uiItem, string configName)
        {
            uiItem.Items.Clear();
            var rawValue = ConfigurationManager.AppSettings[configName];
            if (!string.IsNullOrEmpty(rawValue))
            {
                var entries = rawValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var entry in entries)
                {
                    uiItem.Items.Add(entry.Trim());
                }
            }

            if (uiItem.Items.Count > 0)
            {
                uiItem.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Show claims in access token
        /// </summary>
        /// <param name="accessToken">The access token</param>
        private void ShowAccessToken(string accessToken)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;
                sb.Append("==== Payload in token ====\r\n");
                sb.Append(JsonConvert.SerializeObject(securityToken.Payload, Newtonsoft.Json.Formatting.Indented));
                sb.AppendLine();

                this.tokenOutput.Text = sb.ToString();
            }
            catch(Exception err)
            {
                this.tokenOutput.Text = err.ToString();
            }
        }

        /// <summary>
        /// Send requests to Graph
        /// </summary>
        /// <param name="url">The URL to send request</param>
        /// <param name="method">HTTP method</param>
        /// <param name="accessToken">Access token to be used</param>
        /// <param name="content">Body content for non GET requests</param>
        /// <returns></returns>
        private async Task<ResponseResult> SendRequestToGraphAsync(Uri url, HttpMethod method, string accessToken, string content = null)
        {
            Stopwatch timer = Stopwatch.StartNew();
            var client = new HttpClient();
            HttpRequestMessage request = null;
            StringContent requestContent = null;
            HttpResponseMessage respMessage = null;
            ResponseResult result = new ResponseResult();

            try
            {
                request = new HttpRequestMessage(method, url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                if (method != HttpMethod.Get && !string.IsNullOrEmpty(content))
                {
                    requestContent = new StringContent(
                        content,
                        Encoding.UTF8,
                        "application/json");
                    request.Content = requestContent;
                    request.Headers.Add("Prefer", "return=representation");
                }

                respMessage = client.SendAsync(request).Result;
                result.StatusCode = respMessage.StatusCode;
                result.Headers = respMessage.Headers;
                result.Content = await respMessage.Content.ReadAsStringAsync();
            }
            finally
            {
                if (requestContent != null)
                {
                    requestContent.Dispose();
                }

                if (request != null)
                {
                    request.Dispose();
                }
            }

            timer.Stop();
            result.Latency = timer.ElapsedMilliseconds;

            return result;
        }

        /// <summary>
        /// Show response from Graph
        /// </summary>
        /// <param name="response">The response result</param>
        private void ShowResponse(ResponseResult response)
        {
            if (response.Exception != null)
            {
                this.responseContentOutput.Text = response.Exception.ToString();
            }
            else
            {
                var cleanContent = response.Content.Replace("\\r\\n", "\r\n").Replace("\\\"", "\"");
                try
                {
                    this.responseContentOutput.Text = JValue.Parse(cleanContent).ToString(Newtonsoft.Json.Formatting.Indented);
                }
                catch
                {
                    this.responseContentOutput.Text = cleanContent;
                }

                StringBuilder headerSb = new StringBuilder();
                foreach (var header in response.Headers)
                {
                    headerSb.Append($"{header.Key}\r\n");
                    foreach (var item in header.Value)
                    {
                        headerSb.Append($"   {item}\r\n");
                    }
                }

                this.responseHeaderOutput.Text = headerSb.ToString();
            }

            statusOutput.Text = $"Status Code {response.StatusCode}      {response.Latency}ms";
        }

        /// <summary>
        /// Called to sign out 
        /// </summary>
        /// <param name="sender">UI sender</param>
        /// <param name="e">UI event</param>
        private void OnSignOut(object sender, EventArgs e)
        {
            var appId = appIdInput.Text.Trim();
            PublicClientApplication client;
            if (this.MsalClientDict.TryGetValue(appId, out client))
            {
                client.Remove(client.Users.FirstOrDefault());
            }
        }
    }
}
