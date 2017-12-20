using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWallpaper.Wallpapers
{
    [JsonObject(MemberSerialization.OptOut)]
    public class WallpaperManifestItem
    {
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
    public class WallpaperManifest
    {
        public List<WallpaperManifestItem> list;

        public WallpaperManifest()
        {
            list = new List<WallpaperManifestItem>();
        }

        public static WallpaperManifest GetWallpapersList(string jsonData)
        {
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

            WallpaperManifest wallpaperManifest = new WallpaperManifest();
            wallpaperManifest.list = list;
            return wallpaperManifest;
        }
    }
}
