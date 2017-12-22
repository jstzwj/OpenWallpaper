﻿using CefSharp;
using CefSharp.Wpf;
using Microsoft.Win32;
using OpenWallpaper.Settings;
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
        public delegate void ShowPageEventHandler(string path);

        private IntPtr _handle;

        private NotifyIcon _notifyIcon;

        private ChromiumWebBrowser _chromiumWebBrowser;

        public MainWindow()
        {
            InitializeComponent();
            SetIcon();

            // cef init
            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("disable-gpu", "0");
            Cef.Initialize(settings);

            // create browser
            _chromiumWebBrowser = new ChromiumWebBrowser();
            _chromiumWebBrowser.SetValue(ChromiumWebBrowser.AddressProperty,
                "about:blank");
            MainGrid.Children.Add(_chromiumWebBrowser);

            // random wallpaper
            RandomWallpaper();

            // listen system event
             SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
        }

        ~MainWindow()
        {
            Microsoft.Win32.SystemEvents.SessionSwitch -= new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
        }

        public void ShowPage(string path)
        {
            _chromiumWebBrowser.SetValue(ChromiumWebBrowser.AddressProperty,"file:///" + path);
        }

        private void RandomWallpaper()
        {
            // random wallpaper
            AppData._wallpapersManifest = WallpapersManifest.GetWallpapersList("wallpaperManifest.json");
            Wallpapers.WallpaperManifestItem randomWallpaper = AppData._wallpapersManifest.RandomWallpaperInPlaylist();
            WallpaperManifest manifest = WallpaperManifest.GetWallpaper(randomWallpaper.WallpaperPath);

            // get index.html path
            string wallpaperDirectory = System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(randomWallpaper.WallpaperPath));
            string indexPath = System.IO.Path.Combine(wallpaperDirectory, manifest.WallpaperMainPath);

            // create browser
            _chromiumWebBrowser.SetValue(ChromiumWebBrowser.AddressProperty,
                "file:///" + indexPath);
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLogon:
                    // Console.WriteLine("用户登录");
                    RandomWallpaper();
                    break;

                case SessionSwitchReason.SessionUnlock:
                    // Console.WriteLine("解锁");
                    RandomWallpaper();
                    break;

                case SessionSwitchReason.SessionLock:
                    // Console.WriteLine("锁屏");
                    break;
                case SessionSwitchReason.SessionLogoff:
                    // Console.WriteLine("注销");
                    break;
            }
        }

        private void SetIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon("AppIcon.ico");
            _notifyIcon.Visible = true;

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
            _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            _notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
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
            WallpapersWindow wallpapersWindow = new WallpapersWindow(new ShowPageEventHandler(ShowPage));
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
