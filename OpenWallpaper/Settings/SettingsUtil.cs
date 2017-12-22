using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OpenWallpaper.Settings
{
    public class SettingsUtil
    {
        public static string ReadSettingsAsString(string path)
        {
            string result = null;

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                using (StreamReader sr = new StreamReader(fs))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                // _logger.LogError(default(EventId), e, e.Message);
            }

            return result;
        }

        public static void WriteSettingsAsString(string path, string data)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.Write(data);
                }
            }
            catch (IOException e)
            {
                // _logger.LogError(default(EventId), e, e.Message);
            }
        }

        public static BitmapImage ReadImage(string path)
        {
            BitmapImage bitmapImage;
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                FileInfo fi = new FileInfo(path);
                byte[] bytes = reader.ReadBytes((int)fi.Length);
                reader.Close();

                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(bytes);
                bitmapImage.EndInit();

                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            }
            return bitmapImage;
        }
    }
}
