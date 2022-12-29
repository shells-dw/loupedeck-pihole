namespace Loupedeck.PiholePlugin.Actions
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Loupedeck.PiholePlugin.Helpers;

    internal class Enable : PluginDynamicCommand
    {
        // Assign variables

        private PiholePlugin _plugin;
        // build action
        public Enable() : base() => this.AddParameter("enable", "Enable Ad-blocking", groupName: "Enable");

        protected override Boolean OnLoad()
        {
            this._plugin = base.Plugin as PiholePlugin;
            if (this._plugin is null)
            {
                return false;
            }

            this._plugin.UpdatedStatus += (sender, e) => this.ActionImageChanged("");
            return base.OnLoad();
        }

        // Button is pressed
        protected override void RunCommand(String actionParameter)
        {
            this._plugin.TryGetPluginSetting("ApiUrl", out var apiUrl);
            this._plugin.TryGetPluginSetting("ApiToken", out var ApiToken);
            var httpClient = new HttpClient();
            var apiClient = new PiHoleApiClient(httpClient, apiUrl, ApiToken);
            Task.Run(() => apiClient.Enable()).Wait();
            this.Log.Info($"{DateTime.Now} - Enabled PiHole");
            // notify Loupedeck about the change
            this.ActionImageChanged(actionParameter);
        }

        // update command image (nothing to update here per se, but that's called to draw whatever is shown on the Loupedeck)
        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            Boolean currentState;
            if (this.Plugin.PluginStatus.Status.ToString() != "Normal")
            {
                using (var bitmapBuilder = new BitmapBuilder(imageSize))
                {
                    //drawing a black full-size rectangle to overlay the default graphic (TODO: figure out if that's maybe something that is done nicer than this)
                    bitmapBuilder.DrawRectangle(0, 0, 80, 80, new BitmapColor(0, 0, 0, 255));
                    bitmapBuilder.FillRectangle(0, 0, 80, 80, new BitmapColor(0, 0, 0, 255));
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(EmbeddedResources.FindFile("piholeErr.png")));

                    // draw icons for different cases

                    bitmapBuilder.DrawText("Error", x: 5, y: 35, width: 70, height: 40, fontSize: 20, color: new BitmapColor(255, 255, 255, 255));
                    return bitmapBuilder.ToImage();
                }
            }
            currentState = Globals.PiDump.Status == "enabled";
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.DrawRectangle(0, 0, 80, 80, BitmapColor.Black);
                bitmapBuilder.FillRectangle(0, 0, 80, 80, BitmapColor.Black);
                bitmapBuilder.SetBackgroundImage(currentState ? EmbeddedResources.ReadImage(EmbeddedResources.FindFile("piholeOn.png")) : EmbeddedResources.ReadImage(EmbeddedResources.FindFile("piholeOff.png")));
                bitmapBuilder.DrawRectangle(0, 40, 80, 32, BitmapColor.Transparent);
                bitmapBuilder.FillRectangle(0, 40, 80, 32, color: currentState ? new BitmapColor(0, 0, 0, 150) : new BitmapColor(0, 100, 0, 200));
                bitmapBuilder.DrawText("Enable\nAd-blocking", x: 5, y: 35, width: 70, height: 40, fontSize: 15, color: BitmapColor.White);

                return bitmapBuilder.ToImage();
            }
        }
    }
}

