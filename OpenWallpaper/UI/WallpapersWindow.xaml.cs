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
                item.WallpaperCheckbox.Checked += new RoutedEventHandler(CheckBoxChecked);
                item.WallpaperCheckbox.Unchecked += new RoutedEventHandler(CheckBoxUnchecked);
                this.MainWrapPanel.Children.Add(item);
            }
        }

        private void CheckBoxChecked(object sender, EventArgs e)
        {
            CheckBox obj = (CheckBox)sender;
            WallpaperItem parent = (WallpaperItem)((Grid)obj.Parent).Parent;
            WallpaperItem newChildren = new WallpaperItem();
            newChildren.SetValue(WallpaperItem.WallpaperNameProperty, parent.WallpaperName.Content);
            newChildren.Margin = new Thickness(10, 10, 10, 10);
            newChildren.SetValue(WallpaperItem.ImagePathProperty, parent.GetValue(WallpaperItem.ImagePathProperty));
            newChildren.WallpaperCheckbox.Visibility = Visibility.Hidden;
            this.Playlist.Children.Add(newChildren);
        }
        private void CheckBoxUnchecked(object sender, EventArgs e)
        {

        }

    }
}
