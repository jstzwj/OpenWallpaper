using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace OpenWallpaper.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IntPtr _handle;
        private WindowState ws;
        private WindowState wsl;
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            icon();
        }

        private void icon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Icon = new System.Drawing.Icon("AppIcon.ico");
            this.notifyIcon.Visible = true;

            //打开菜单项
            System.Windows.Forms.MenuItem settings = new System.Windows.Forms.MenuItem("Settings");
            settings.Click += new EventHandler(ShowSettings);

            //打开菜单项
            System.Windows.Forms.MenuItem wallPapers = new System.Windows.Forms.MenuItem("Wallpapers");
            wallPapers.Click += new EventHandler(ShowWallpapers);

            //打开菜单项
            System.Windows.Forms.MenuItem shop = new System.Windows.Forms.MenuItem("Shop");
            shop.Click += new EventHandler(ShowShop);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(Close);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { settings, wallPapers, shop, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left) this.ShowSettings(o, e);
            });
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void ShowWallpapers(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            WallpapersWindow wallpapersWindow = new WallpapersWindow();
            wallpapersWindow.Show();
        }

        private void ShowShop(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            ShopWindow shopWindow = new ShopWindow();
            shopWindow.Show();
        }

        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Close(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            _handle = (new WindowInteropHelper(this)).Handle;
            WindowDesktopBind.SetDesktopAsParent(_handle);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            double x1 = SystemParameters.PrimaryScreenWidth;//得到屏幕宽度
            double y1 = SystemParameters.PrimaryScreenHeight;//得到屏幕高度
            this.Left = 0;
            this.Top = 0;
            this.Height = y1;
            this.Width = x1;
            base.OnRender(drawingContext);
        }
    }
}
