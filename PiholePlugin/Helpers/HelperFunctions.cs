namespace Loupedeck.PiholePlugin.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;

    internal class HelperFunctions
    {
        public void UpdateConnectionSettings()
        {
            var helperFunction = new PiholePlugin();
            helperFunction.TryGetSetting("ApiUrl", out var apiUrl);
            helperFunction.TryGetSetting("ApiToken", out var apiToken);
            var pluginDataDirectory = helperFunction.GetPluginDataDirectory();
            var json = File.ReadAllText(Path.Combine(pluginDataDirectory, "settings.json"));
            var configFileSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
            if (configFileSettings["ApiUrl"].Value != null && configFileSettings["ApiUrl"].Value != apiUrl)
            {
                helperFunction.SetPluginSetting("ApiUrl", configFileSettings["ApiUrl"].Value); 
            }
            if (configFileSettings["ApiToken"] != null && configFileSettings["ApiToken"].Value != apiToken && configFileSettings["ApiToken"].Value != "[securely stored in plugin settings - replace with new token if neccessary]")
            {
                helperFunction.SetPluginSetting("ApiToken", configFileSettings["ApiToken"].Value);
            //    configFileSettings["ApiToken"] = "[securely stored in plugin settings - replace with new token if neccessary]";
            //    String output = Newtonsoft.Json.JsonConvert.SerializeObject(configFileSettings, Newtonsoft.Json.Formatting.Indented);
            //    File.WriteAllText(Path.Combine(pluginDataDirectory, "settings.json"), output);
            }
            return;
        }
    }
}
