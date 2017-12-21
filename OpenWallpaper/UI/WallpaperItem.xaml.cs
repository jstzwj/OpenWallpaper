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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenWallpaper.UI
{
    /// <summary>
    /// WallpaperItem.xaml 的交互逻辑
    /// </summary>
    public partial class WallpaperItem : UserControl
    {
        public WallpaperManifestItem data { get; set; }

        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(WallpaperItem),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(ImagePathPropertyChangedCallback)));

        public static readonly DependencyProperty WallpaperNameProperty =
            DependencyProperty.Register("WallpaperName", typeof(string), typeof(WallpaperItem),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(WallpaperNamePropertyChangedCallback)));

        public static readonly DependencyProperty BriefProperty =
            DependencyProperty.Register("Brief", typeof(string), typeof(WallpaperItem),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(BriefPropertyChangedCallback)));

        public WallpaperItem()
        {
            InitializeComponent();
        }

        private static void ImagePathPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null)
            {
                WallpaperItem obj = (WallpaperItem)sender;
                obj.WallpaperImage.Source = new BitmapImage(new Uri((string)arg.NewValue, UriKind.RelativeOrAbsolute));
            }
        }

        private static void WallpaperNamePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null)
            {
                WallpaperItem obj= (WallpaperItem)sender;
                obj.WallpaperName.Content = (string)arg.NewValue;
            }
        }

        private static void BriefPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
        }

    }
}
