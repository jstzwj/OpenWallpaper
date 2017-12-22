using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenWallpaper.Settings;
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
        public string WallpaperType { get; set; }

        [JsonProperty(PropertyName = "isInPlaylist")]
        [DefaultValue("false")]
        public bool IsInPlaylist { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class WallpapersManifest
    {
        public int nextID;

        public List<WallpaperManifestItem> list;

        public WallpapersManifest()
        {
            list = new List<WallpaperManifestItem>();
        }

        public WallpaperManifestItem RandomWallpaperInPlaylist()
        {
            int count = 0;
            foreach (WallpaperManifestItem each in list)
            {
                if (each.IsInPlaylist)
                    ++count;
            }

            Random random = new Random();
            int index = random.Next(count);

            count = 0;
            foreach (WallpaperManifestItem each in list)
            {
                if (count == index)
                {
                    return each;
                }

                if (each.IsInPlaylist)
                    ++count;
            }

            return null;
        }

        public static WallpapersManifest GetWallpapersList(string path)
        {
            string jsonData = SettingsUtil.ReadSettingsAsString(path);
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
            wallpaperManifest.nextID = (int)obj["nextID"];
            return wallpaperManifest;
        }

        public static void WriteWallpapersList(string path, WallpapersManifest manifest)
        {
            JObject jsonData = new JObject();
            jsonData["nextID"] = manifest.nextID;
            JArray wallpapers = new JArray();
            foreach (var each in manifest.list)
            {
                string wallpaper = JsonConvert.SerializeObject(each);
                wallpapers.Add(JObject.Parse(wallpaper));
            }
            jsonData["wallpapers"] = wallpapers;

            string data = JsonConvert.SerializeObject(jsonData);
            SettingsUtil.WriteSettingsAsString(path, data);
        }
    }
}
