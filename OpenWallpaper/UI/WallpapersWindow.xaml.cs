using OpenWallpaper.Wallpapers;
using System;
using System.Collections.Generic;
using System.IO;
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
        private MainWindow.ShowPageEventHandler _showPage;

        public WallpapersWindow(MainWindow.ShowPageEventHandler showPage)
        {
            InitializeComponent();
            AppData._wallpapersManifest = WallpapersManifest.GetWallpapersList("wallpaperManifest.json");
            foreach (var each in AppData._wallpapersManifest.list)
            {
                WallpaperItem item = AddWallpaper(each);

                if (each.IsInPlaylist)
                    AddWallpaperItemInPlaylist(item);

                if (each == AppData._wallpapersManifest.list[0])
                    showDetailsFromItem(item);
            }

            _showPage = showPage;

            ButtonAdd.Click += new RoutedEventHandler(AddButtonClicked);
            ButtonDelete.Click += new RoutedEventHandler(DeleteButtonClicked);
            ButtonPlay.Click += new RoutedEventHandler(PlayButtonClicked);
        }

        protected override void OnClosed(EventArgs e)
        {
            WallpapersManifest.WriteWallpapersList("wallpaperManifest.json", AppData._wallpapersManifest);
        }

        public void showDetailsFromItem(WallpaperItem item)
        {
            DetailImage.Source = item.WallpaperImage.Source;
            DetailName.Content = item.WallpaperName.Content;
            DetailAuthor.Content = "";
            DetailBrief.Content = "";
            DetailType.Content = item.data.WallpaperType;
        }

        public WallpaperItem AddWallpaper(WallpaperManifestItem manifestItem)
        {
            WallpaperItem item = new WallpaperItem();
            item.data = manifestItem;
            item.SetValue(WallpaperItem.WallpaperNameProperty, manifestItem.WallpaperName);
            item.Margin = new Thickness(10, 10, 10, 10);


            string imagePath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(manifestItem.WallpaperPath),
                WallpaperManifest.GetWallpaper(manifestItem.WallpaperPath).WallpaperThumbnail
                );
            item.SetValue(WallpaperItem.ImagePathProperty, imagePath);
            item.WallpaperCheckbox.IsChecked = manifestItem.IsInPlaylist;

            item.WallpaperCheckbox.Checked += new RoutedEventHandler(CheckBoxChecked);
            item.WallpaperCheckbox.Unchecked += new RoutedEventHandler(CheckBoxUnchecked);
            item.ItemMask.MouseDown += new MouseButtonEventHandler(ItemMaskClicked);
            MainWrapPanel.Children.Add(item);

            return item;
        }

        public void AddWallpaperItemInPlaylist(WallpaperItem item)
        {
            WallpaperItem newChildren = new WallpaperItem();
            newChildren.data = item.data;
            newChildren.SetValue(WallpaperItem.WallpaperNameProperty, item.WallpaperName.Content);
            newChildren.Margin = new Thickness(10, 10, 10, 10);
            newChildren.SetValue(WallpaperItem.ImagePathProperty, item.GetValue(WallpaperItem.ImagePathProperty));
            newChildren.WallpaperCheckbox.Visibility = Visibility.Hidden;
            this.Playlist.Children.Add(newChildren);
        }

        public void RemoveWallpaperItemInPlaylist(WallpaperItem item)
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
            foreach (var each in AppData._wallpapersManifest.list)
            {
                if (each.WallpaperID == item.data.WallpaperID)
                {
                    removedItem = each;
                    break;
                }
            }

            return removedItem;
        }

        public void UpdatePlaylist()
        {
            Playlist.Children.Clear();
            foreach (WallpaperItem each in MainWrapPanel.Children)
            {
                WallpaperManifestItem wallpaperManifestItem = FindInManifest(each);
                if (each.WallpaperCheckbox.IsChecked == true)
                {
                    AddWallpaperItemInPlaylist(each);
                    if (wallpaperManifestItem != null)
                        wallpaperManifestItem.IsInPlaylist = true;
                }
                else
                {
                    if (wallpaperManifestItem != null)
                        wallpaperManifestItem.IsInPlaylist = false;
                }
            }
        }

        private void CheckBoxChecked(object sender, EventArgs e)
        {
            CheckBox obj = (CheckBox)sender;
            WallpaperItem parent = (WallpaperItem)((Grid)obj.Parent).Parent;

            showDetailsFromItem(parent);
        }

        private void CheckBoxUnchecked(object sender, EventArgs e)
        {
            CheckBox obj = (CheckBox)sender;
            WallpaperItem parent = (WallpaperItem)((Grid)obj.Parent).Parent;

            showDetailsFromItem(parent);
        }

        private void PlayButtonClicked(object sender, EventArgs e)
        {
            UpdatePlaylist();
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog =
                new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "zip压缩文件|*.zip";
            if (dialog.ShowDialog() == true)
            {
                string file = dialog.FileName;
                string targetDirectory = System.IO.Path.Combine(System.IO.Path.GetFullPath("wallpaper"),
                    System.IO.Path.GetFileNameWithoutExtension(file));
                Package.PackageInstall.Install(file, System.IO.Path.GetFullPath("wallpaper"));

                WallpaperManifest manifest = WallpaperManifest.GetWallpaper(System.IO.Path.Combine(targetDirectory, "manifest.json"));

                WallpaperManifestItem manifestItem = new WallpaperManifestItem();
                manifestItem.WallpaperID = AppData._wallpapersManifest.nextID;
                AppData._wallpapersManifest.nextID++;
                manifestItem.WallpaperPath = System.IO.Path.Combine(targetDirectory,"manifest.json");
                manifestItem.WallpaperName = manifest.WallpaperName;
                manifestItem.WallpaperType = manifest.WallpaperType;

                AppData._wallpapersManifest.list.Add(manifestItem);

                WallpaperItem item = AddWallpaper(manifestItem);

                if (manifestItem.IsInPlaylist)
                    AddWallpaperItemInPlaylist(item);

                if (manifestItem == AppData._wallpapersManifest.list[0])
                    showDetailsFromItem(item);
            }
        }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            List<WallpaperItem> removed = new List<WallpaperItem>();

            foreach (WallpaperItem each in MainWrapPanel.Children)
            {
                WallpaperManifestItem wallpaperManifestItem = FindInManifest(each);
                if (each.WallpaperCheckbox.IsChecked == true)
                {
                    RemoveWallpaperItemInPlaylist(each);
                    if (wallpaperManifestItem != null)
                    {
                        AppData._wallpapersManifest.list.Remove(wallpaperManifestItem);
                    }

                    removed.Add(each);
                }
                else
                {
                    if (wallpaperManifestItem != null)
                        wallpaperManifestItem.IsInPlaylist = false;
                }
            }

            foreach (WallpaperItem each in removed)
            {
                MainWrapPanel.Children.Remove(each);
                Directory.Delete(System.IO.Path.GetDirectoryName(each.data.WallpaperPath), true);
            }
        }

        private void ItemMaskClicked(object sender, EventArgs e)
        {
            Rectangle obj = (Rectangle)sender;
            WallpaperItem parent = (WallpaperItem)((Grid)obj.Parent).Parent;
            parent.WallpaperCheckbox.IsChecked = !parent.WallpaperCheckbox.IsChecked;

            string indexAbsolutePath = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(parent.data.WallpaperPath),
                    WallpaperManifest.GetWallpaper(parent.data.WallpaperPath).WallpaperMainPath
                    );

            _showPage(indexAbsolutePath);
        }
    }
}
