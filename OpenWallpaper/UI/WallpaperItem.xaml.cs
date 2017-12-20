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
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(DateTime), typeof(WallpaperItem),
            new FrameworkPropertyMetadata(DateTime.Now, new PropertyChangedCallback(ImagePathPropertyChangedCallback)));

        public static readonly DependencyProperty WallpaperNameProperty =
            DependencyProperty.Register("WallpaperName", typeof(DateTime), typeof(WallpaperItem),
            new FrameworkPropertyMetadata(DateTime.Now, new PropertyChangedCallback(WallpaperNamePropertyChangedCallback)));

        public static readonly DependencyProperty BriefProperty =
            DependencyProperty.Register("Brief", typeof(DateTime), typeof(WallpaperItem),
            new FrameworkPropertyMetadata(DateTime.Now, new PropertyChangedCallback(BriefPropertyChangedCallback)));

        public WallpaperItem()
        {
            InitializeComponent();
        }

        private static void ImagePathPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
        }

        private static void WallpaperNamePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
        }

        private static void BriefPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
        }

    }
}
