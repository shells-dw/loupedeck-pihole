namespace Loupedeck.PiholePlugin
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Runtime;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    using Loupedeck.PiholePlugin.Helpers;
    using Loupedeck.PiholePlugin.Models;

    // This class contains the plugin-level logic of the Loupedeck plugin.

    public class PiholePlugin : Plugin
    {
        // assign variables
        public event EventHandler<Summary> UpdatedStatus;
        private readonly Thread _queryThread;
        private String apiUrl;
        private String ApiToken;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        // Gets a value indicating whether this is an Universal plugin or an Application plugin.
        public override Boolean UsesApplicationApiOnly => true;

        // Gets a value indicating whether this is an API-only plugin.
        public override Boolean HasNoApplication => true;

        //background updater
        public PiholePlugin() => this._queryThread = new Thread(this.QueryThread) { IsBackground = true };
        private async void QueryThread()
        {
            var httpClient = new HttpClient();
            var apiClient = new PiHoleApiClient(httpClient, this.apiUrl, this.ApiToken);
            var _this = new PiholePlugin();
            _this.Log.Info($"{DateTime.Now} - Starting query thread");

            while (true && !this._cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    Globals.PiDump = await apiClient.GetSummaryRawAsync();
                    UpdatedStatus?.Invoke(this, Globals.PiDump);
                }
                catch (Exception ex)
                {
                    _this.Log.Error($"{DateTime.Now} - QueryThread: Exception caught {ex.Message}");
                }
                await Task.Delay(1000);
            }
        }
        // This method is called when the plugin is loaded during the Loupedeck service start-up.
        public override void Load()
        {
            // call installer that puts the settings.json file in place if it shouldn't exist
            PiholePluginInstaller.Install();

            // update plugin settings for PiHole url and token from settings.json if necessary
            this.UpdateConnectionSettings();

            // get plugin settings and fill variables
            this.FetchSettings();

            // verify pihole is reachable under given url and the token is correct, set plugin status accordingly to display help if need be

            if (!this.VerifyConnectivity())
            { 
                this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Can't connect to PiHole! Verify and set URL in settings.json accordingly.", "https://github.com/shells-dw/loupedeck-pihole#settings.json", "GitHub Readme");
                this.Log.Error($"{DateTime.Now} - Starting... !this.VerifyConnectivity()");
            }
            else
            {
                if (!this.VerifyToken())
                {
                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "It appears the API token isn't set correctly. Verify and set ApiToken in settings.json accordingly.", "https://github.com/shells-dw/loupedeck-pihole#settings.json", "GitHub Readme");
                    this.Log.Error($"{DateTime.Now} - Starting... !this.VerifyToken()");
                }
                else
                {
                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Normal, "Connected", null, null);
                    this.Log.Info($"{DateTime.Now} - Starting... plugin in nominal status");
                }
            }

            // start background updater
            this._queryThread.Start();
        }

        // This method is called when the plugin is unloaded during the Loupedeck service shutdown.
        public override void Unload() => this._cancellationTokenSource.Cancel();

        private Boolean VerifyConnectivity()
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    return tcpClient.ConnectAsync(Regex.Match(this.apiUrl, @"^(?:https?:\/\/)?(?:[^@\/\n]+@)?(?:www\.)?([^:\/\n]+)", RegexOptions.Singleline).Groups[1].Value, 80).Wait(5000);
                }
                catch (Exception ex)
                {
                    this.Log.Error($"{DateTime.Now} - VerifyConnectivity: Exception {ex.Message}");
                    return false;
                }
            }
        }
        private Boolean VerifyToken()
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(2);
            var apiClient = new PiHoleApiClient(httpClient, this.apiUrl, this.ApiToken);

            // get status
            var getSettings = Task.Run(() => apiClient.GetSummaryRawAsync()).Result;
            Boolean apiAccessGranted;
            if (getSettings.Status == null)
            {
                this.Log.Error($"{DateTime.Now} - VerifyToken: getSettings.Status == null");
                return false;
            }
            apiAccessGranted = getSettings.Status == "enabled"
                ? Task.Run(() => apiClient.VerifyConnectivity("enable")).Result
                : Task.Run(() => apiClient.VerifyConnectivity("disable")).Result;
            return apiAccessGranted == true;
        }

        public void UpdateConnectionSettings()
        {
            this.Log.Info($"{DateTime.Now} - UpdateConnectionSettings()");
            this.TryGetSetting("ApiUrl", out var apiUrl);
            this.TryGetSetting("ApiToken", out var apiToken);
            var pluginDataDirectory = this.GetPluginDataDirectory();
            var json = File.ReadAllText(Path.Combine(pluginDataDirectory, "settings.json"));
            var configFileSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
            if (configFileSettings["ApiUrl"].Value != null && configFileSettings["ApiUrl"].Value != apiUrl)
            {
                this.SetPluginSetting("ApiUrl", configFileSettings["ApiUrl"].Value);
            }
            if (configFileSettings["ApiToken"] != null && configFileSettings["ApiToken"].Value != apiToken && configFileSettings["ApiToken"].Value != "[securely stored in plugin settings - replace with new token if neccessary]")
            {
                this.SetPluginSetting("ApiToken", configFileSettings["ApiToken"].Value);
            }
            if (configFileSettings["ApiToken"].Value == apiToken)
            {
                configFileSettings["ApiToken"] = "[securely stored in plugin settings - replace with new token if neccessary]";
                String output = Newtonsoft.Json.JsonConvert.SerializeObject(configFileSettings, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(Path.Combine(pluginDataDirectory, "settings.json"), output);
            }
            return;
        }

        private void FetchSettings()
        {
            this.TryGetSetting("ApiUrl", out this.apiUrl);
            this.TryGetSetting("ApiToken", out this.ApiToken);
        }

        public Boolean TryGetSetting(String settingName, out String settingValue) =>
            this.TryGetPluginSetting(settingName, out settingValue);

        public void SetSetting(String settingName, String settingValue, Boolean backupOnline = false) =>
            this.SetPluginSetting(settingName, settingValue, backupOnline);

    }
}
