using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWallpaper.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AppSettings
    {
        // server
        [JsonProperty(PropertyName = "name")]
        [DefaultValue("User")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "autoStart")]
        [DefaultValue(true)]
        public bool AutoStart { get; set; }

        [JsonProperty(PropertyName = "wallpaperManifest")]
        [DefaultValue("wallpaperManifest.json")]
        public string WallpaperManifest { get; set; }

        static AppSettings ReadFromFile(string path)
        {
            string settingsFile = SettingsUtil.ReadSettingsAsString(path);
            AppSettings result;
            result = JsonConvert.DeserializeObject<AppSettings>(settingsFile);

            return result;
        }
    }
}
