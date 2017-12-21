using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWallpaper.Wallpapers
{
    [JsonObject(MemberSerialization.OptOut)]
    public class WallpaperManifestItem
    {
        [JsonProperty(PropertyName = "wallpaperID")]
        [DefaultValue(0)]
        public int WallpaperID { get; set; }

        [JsonProperty(PropertyName = "wallpaperName")]
        [DefaultValue("default")]
        public string WallpaperName { get; set; }

        [JsonProperty(PropertyName = "wallpaperPath")]
        [DefaultValue("/")]
        public string WallpaperPath { get; set; }


        [JsonProperty(PropertyName = "wallpaperType")]
        [DefaultValue("web")]
        public string wallpaperType { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class WallpapersManifest
    {
        public List<WallpaperManifestItem> list;

        public WallpapersManifest()
        {
            list = new List<WallpaperManifestItem>();
        }

        public static WallpapersManifest GetWallpapersList(string path)
        {
            string jsonData = ReadSettingsAsString(path);
            List<WallpaperManifestItem> list = new List<WallpaperManifestItem>();
            JObject obj = JObject.Parse(jsonData);
            JArray data = (JArray)obj["wallpapers"];
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<WallpaperManifestItem>(item.ToString()));
                }
            }

            WallpapersManifest wallpaperManifest = new WallpapersManifest();
            wallpaperManifest.list = list;
            return wallpaperManifest;
        }

        private static string ReadSettingsAsString(string path)
        {
            string result = null;

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                using (StreamReader sr = new StreamReader(fs))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                // _logger.LogError(default(EventId), e, e.Message);
            }

            return result;
        }
    }
}
