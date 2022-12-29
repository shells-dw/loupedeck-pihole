namespace Loupedeck.PiholePlugin
{
    using System;
    using System.IO;
    using System.Reflection;

    public class PiholePluginInstaller
    {
        public static Boolean Install()
        {
            // Here we ensure the plugin data directory is there.
            var _this = new PiholePlugin();
            var pluginDataDirectory = _this.GetPluginDataDirectory();
            if (!IoHelpers.EnsureDirectoryExists(pluginDataDirectory))
            {
                _this.Log.Info($"{DateTime.Now} - Installer: pluginDataDirectory doesn't exist");
                return false;
            }

            // Now we put a template configuration file
            var filePath = System.IO.Path.Combine(pluginDataDirectory, System.IO.Path.Combine(pluginDataDirectory, "settings.json"));
            if (File.Exists(filePath))
            {
                _this.Log.Info($"{DateTime.Now} - Installer: File exists");
                return true;
            }
            else
            {
                ResourceReader.CreateFileFromResource("Loupedeck.PiholePlugin.settings.json", filePath);
                _this.Log.Info($"{DateTime.Now} - Installer: config file does not yet exist, creating default file");
                return true;
            }
        }
    }
    public class ResourceReader
    {
        // to read the file as a Stream
        public static Stream GetResourceStream(String resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
            return resourceStream;
        }

        // to save the resource to a file
        public static void CreateFileFromResource(String resourceName, String path)
        {
            Stream resourceStream = GetResourceStream(resourceName);
            if (resourceStream != null)
            {
                using (Stream input = resourceStream)
                {
                    using (Stream output = File.Create(path))
                    {
                        input.CopyTo(output);
                    }
                }
            }
        }
    }
}
