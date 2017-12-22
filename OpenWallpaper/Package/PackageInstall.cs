using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWallpaper.Package
{
    public class PackageInstall
    {
        public static void Install(string zippedFilePath, string directoryPath)
        {
            (new FastZip()).ExtractZip(zippedFilePath, directoryPath, "");
        }
    }
}
