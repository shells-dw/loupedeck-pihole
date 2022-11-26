namespace Loupedeck.PiholePlugin.Actions
{
    using System;
    using System.CodeDom;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using Loupedeck.PiholePlugin.Helpers;

    internal class AdsBlockedPercentage : PluginDynamicCommand
    {
        // Assign variables

        private PiholePlugin _plugin;
        // build action
        public AdsBlockedPercentage() : base() => this.AddParameter("adsblockedpercentage", "Display Ads Blocked Today %", groupName: "Display");

        protected override Boolean OnLoad()
        {
            this._plugin = base.Plugin as PiholePlugin;
            if (this._plugin is null)
                return false;

            this._plugin.UpdatedStatus += (sender, e) => this.ActionImageChanged("");
            return base.OnLoad();
        }

        // Button is pressed
        protected override void RunCommand(String actionParameter) => this.ActionImageChanged(actionParameter);

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
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.DrawRectangle(0, 0, 80, 80, BitmapColor.Black);
                bitmapBuilder.FillRectangle(0, 0, 80, 80, BitmapColor.Black);
                bitmapBuilder.SetBackgroundImage(currentState ? EmbeddedResources.ReadImage(EmbeddedResources.FindFile("piholeOn.png")) : EmbeddedResources.ReadImage(EmbeddedResources.FindFile("piholeOff.png")));
                bitmapBuilder.DrawRectangle(0, 0, 80, 80, BitmapColor.Transparent);
                bitmapBuilder.FillRectangle(0, 0, 80, 80, color: new BitmapColor(0, 0, 0, 140));
                bitmapBuilder.DrawText($"{Double.Parse(Globals.PiDump.AdsPercentageToday, CultureInfo.InvariantCulture).ToString("0.0")}%", x: 40, y: 5, width: 0, height: 40, fontSize: 15, color: BitmapColor.White);
                bitmapBuilder.DrawText("% blocked\ntoday", x: 5, y: 35, width: 70, height: 40, fontSize: 12, color: BitmapColor.White);

                return bitmapBuilder.ToImage();
            }
        }
    }
}

