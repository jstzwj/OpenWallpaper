using OpenWallpaper.Wallpapers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenWallpaper.UI
{
    /// <summary>
    /// WallpapersWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WallpapersWindow : Window
    {
        private WallpapersManifest wallpapersManifest;
        public WallpapersWindow()
        {
            InitializeComponent();
            wallpapersManifest = WallpapersManifest.GetWallpapersList("wallpaperManifest.json");
            foreach (var each in wallpapersManifest.list)
            {
                WallpaperItem item = new WallpaperItem();
                item.SetValue(WallpaperItem.WallpaperNameProperty,each.WallpaperName);
                item.Margin = new Thickness(10, 10, 10, 10);


                string imageAbsolutePath = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(each.WallpaperPath), 
                    WallpaperManifest.GetWallpaper(each.WallpaperPath).WallpaperThumbnail
                    );
                item.SetValue(WallpaperItem.ImagePathProperty, imageAbsolutePath);
                this.MainWrapPanel.Children.Add(item);
            }
        }
    }
}
