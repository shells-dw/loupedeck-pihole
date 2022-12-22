namespace Loupedeck.PiholePlugin.Actions
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Loupedeck.PiholePlugin.Helpers;

    internal class Disable : PluginDynamicCommand
    {
        // Assign variables

        private PiholePlugin _plugin;
        // build action
        public Disable() : base()
        {
            this.AddParameter("10", "Disable 10s", groupName: "Disable");
            this.AddParameter("30", "Disable 30s", groupName: "Disable");
            this.AddParameter("300", "Disable 5m", groupName: "Disable");
            this.AddParameter("0", "Disable Indefinitely", groupName: "Disable");
        }
        protected override Boolean OnLoad()
        {
            this._plugin = base.Plugin as PiholePlugin;
            if (this._plugin is null)
            {
                return false;
            }

            this._plugin.UpdatedStatus += (sender, e) => this.Update();
            return base.OnLoad();
        }

        private void Update()
        {
            this.ActionImageChanged("10");
            this.ActionImageChanged("30");
            this.ActionImageChanged("300");
            this.ActionImageChanged("0");
        }

        // Button is pressed
        protected override void RunCommand(String actionParameter)
        {
            this._plugin.TryGetPluginSetting("ApiUrl", out var apiUrl);
            this._plugin.TryGetPluginSetting("ApiToken", out var ApiToken);
            var httpClient = new HttpClient();
            var apiClient = new PiHoleApiClient(httpClient, apiUrl, ApiToken);
            var data = Task.Run(() => apiClient.Disable(Int32.Parse(actionParameter))).GetAwaiter().GetResult();
            // notify Loupedeck about the change
            this.ActionImageChanged(actionParameter);
        }

        // update command image (nothing to update here per se, but that's called to draw whatever is shown on the Loupedeck)
        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
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

            var currentState = Globals.PiDump.Status == "enabled";
            var disabled = actionParameter != "0" ? $"Disable for\n{actionParameter}s" : $"Disable\nindefinitely";

            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.DrawRectangle(0, 0, 80, 80, BitmapColor.Black);
                bitmapBuilder.FillRectangle(0, 0, 80, 80, BitmapColor.Black);
                bitmapBuilder.SetBackgroundImage(currentState ? EmbeddedResources.ReadImage(EmbeddedResources.FindFile("piholeOn.png")) : EmbeddedResources.ReadImage(EmbeddedResources.FindFile("piholeOff.png")));
                bitmapBuilder.DrawRectangle(0, 40, 80, 32, BitmapColor.Transparent);
                bitmapBuilder.FillRectangle(0, 40, 80, 32, color: currentState ? new BitmapColor(150, 0, 0, 200) : new BitmapColor(0, 0, 0, 150));
                bitmapBuilder.DrawText(disabled, x: 5, y: 35, width: 70, height: 40, fontSize: 15, color: BitmapColor.White);

                return bitmapBuilder.ToImage();
            }
        }
    }
}

