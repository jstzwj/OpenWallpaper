﻿using Newtonsoft.Json;
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
    public class WallpaperManifest
    {
        [JsonProperty(PropertyName = "wallpaperName")]
        [DefaultValue("default")]
        public string WallpaperName { get; set; }

        [JsonProperty(PropertyName = "wallpaperMainPath")]
        [DefaultValue("index.html")]
        public string WallpaperMainPath { get; set; }


        [JsonProperty(PropertyName = "wallpaperType")]
        [DefaultValue("web")]
        public string WallpaperType { get; set; }

        [JsonProperty(PropertyName = "wallpaperThumbnail")]
        [DefaultValue("")]
        public string WallpaperThumbnail { get; set; }


        public static WallpaperManifest GetWallpaper(string path)
        {
            string settingsFile = SettingsUtil.ReadSettingsAsString(path);
            WallpaperManifest result;
            result = JsonConvert.DeserializeObject<WallpaperManifest>(settingsFile);

            return result;
        }
    }

}
