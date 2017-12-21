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
        private WallpapersManifest _wallpapersManifest;

        private MainWindow.ShowPageEventHandler _showPage;

        public WallpapersWindow(MainWindow.ShowPageEventHandler showPage)
        {
            InitializeComponent();
            _wallpapersManifest = WallpapersManifest.GetWallpapersList("wallpaperManifest.json");
            foreach (var each in _wallpapersManifest.list)
            {
                WallpaperItem item = new WallpaperItem();
                item.data = each;
                item.SetValue(WallpaperItem.WallpaperNameProperty,each.WallpaperName);
                item.Margin = new Thickness(10, 10, 10, 10);


                string imageAbsolutePath = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(each.WallpaperPath), 
                    WallpaperManifest.GetWallpaper(each.WallpaperPath).WallpaperThumbnail
                    );
                item.SetValue(WallpaperItem.ImagePathProperty, imageAbsolutePath);
                item.WallpaperCheckbox.IsChecked = each.IsInPlaylist;

                item.WallpaperCheckbox.Checked += new RoutedEventHandler(CheckBoxChecked);
                item.WallpaperCheckbox.Unchecked += new RoutedEventHandler(CheckBoxUnchecked);
                item.ItemMask.MouseDown += new MouseButtonEventHandler(ItemMaskClicked);
                this.MainWrapPanel.Children.Add(item);

                if (each.IsInPlaylist)
                    addWallpaperItemInPlaylist(item);

                if (each == _wallpapersManifest.list[0])
                    showDetailsFromItem(item);

                _showPage = showPage;
            } 
        }

        protected override void OnClosed(EventArgs e)
        {
            WallpapersManifest.WriteWallpapersList("wallpaperManifest.json", _wallpapersManifest);
        }

        public void showDetailsFromItem(WallpaperItem item)
        {
            DetailImage.Source = item.WallpaperImage.Source;
            DetailName.Content = item.WallpaperName.Content;
            DetailAuthor.Content = "";
            DetailBrief.Content = "";
            DetailType.Content = item.data.WallpaperType;
        }

        public void addWallpaperItemInPlaylist(WallpaperItem item)
        {
            WallpaperItem newChildren = new WallpaperItem();
            newChildren.data = item.data;
            newChildren.SetValue(WallpaperItem.WallpaperNameProperty, item.WallpaperName.Content);
            newChildren.Margin = new Thickness(10, 10, 10, 10);
            newChildren.SetValue(WallpaperItem.ImagePathProperty, item.GetValue(WallpaperItem.ImagePathProperty));
            newChildren.WallpaperCheckbox.Visibility = Visibility.Hidden;
            this.Playlist.Children.Add(newChildren);
        }

        public void removeWallpaperItemInPlaylist(WallpaperItem item)
        {
            WallpaperItem removedItem = null;
            foreach (WallpaperItem each in Playlist.Children)
            {
                if (each.data.WallpaperID == item.data.WallpaperID)
                {
                    removedItem = each;
                    break;
                }
            }

            if (removedItem != null)
                Playlist.Children.Remove(removedItem);
        }

        public WallpaperManifestItem FindInManifest(WallpaperItem item)
        {
            WallpaperManifestItem removedItem = null;
            foreach (var each in _wallpapersManifest.list)
            {
                if (each.WallpaperID == item.data.WallpaperID)
                {
                    removedItem = each;
                    break;
                }
            }

            return removedItem;
        }

        private void CheckBoxChecked(object sender, EventArgs e)
        {
            CheckBox obj = (CheckBox)sender;
            WallpaperItem parent = (WallpaperItem)((Grid)obj.Parent).Parent;

            addWallpaperItemInPlaylist(parent);

            WallpaperManifestItem wallpaperManifestItem = FindInManifest(parent);
            if(wallpaperManifestItem!=null)
                wallpaperManifestItem.IsInPlaylist = true;

            showDetailsFromItem(parent);

            string indexAbsolutePath = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(parent.data.WallpaperPath),
                    WallpaperManifest.GetWallpaper(parent.data.WallpaperPath).WallpaperMainPath
                    );

            _showPage(indexAbsolutePath);
        }

        private void CheckBoxUnchecked(object sender, EventArgs e)
        {
            CheckBox obj = (CheckBox)sender;
            WallpaperItem parent = (WallpaperItem)((Grid)obj.Parent).Parent;

            removeWallpaperItemInPlaylist(parent);

            WallpaperManifestItem wallpaperManifestItem = FindInManifest(parent);
            if (wallpaperManifestItem != null)
                wallpaperManifestItem.IsInPlaylist = false;

            showDetailsFromItem(parent);
        }

        private void ItemMaskClicked(object sender, EventArgs e)
        {
            Rectangle obj = (Rectangle)sender;
            WallpaperItem parent = (WallpaperItem)((Grid)obj.Parent).Parent;
            parent.WallpaperCheckbox.IsChecked = !parent.WallpaperCheckbox.IsChecked;
        }
    }
}
