using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWallpaper
{
    public class WindowDesktopBind
    {
        static void PrintVisibleWindowHandles(int maxLevel = -1)
        {
            // Enumerates all existing top window handles. This includes open and visible windows, as well as invisible windows.
            Win32Wrapper.EnumWindows(new Win32Wrapper.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                PrintVisibleWindowHandles(tophandle, maxLevel);
                return true;
            }), IntPtr.Zero);
        }

        static void PrintVisibleWindowHandles(IntPtr hwnd, int maxLevel = -1, int level = 0)
        {
            bool isVisible = Win32Wrapper.IsWindowVisible(hwnd);

            if (isVisible && (maxLevel == -1 || level <= maxLevel))
            {
                StringBuilder className = new StringBuilder(256);
                Win32Wrapper.GetClassName(hwnd, className, className.Capacity);

                StringBuilder windowTitle = new StringBuilder(256);
                Win32Wrapper.GetWindowText(hwnd, windowTitle, className.Capacity);

                Console.WriteLine("".PadLeft(level * 2) + "0x{0:X8} \"{1}\" {2}", hwnd.ToInt64(), windowTitle, className);

                level++;

                // Enumerates all child windows of the current window
                Win32Wrapper.EnumChildWindows(hwnd, new Win32Wrapper.EnumWindowsProc((childhandle, childparamhandle) =>
                {
                    PrintVisibleWindowHandles(childhandle, maxLevel, level);
                    return true;
                }), IntPtr.Zero);
            }
        }

        public static void SetDesktopAsParent(IntPtr handle)
        {
            PrintVisibleWindowHandles(2);
            // The output will look something like this. 
            // .....
            // 0x00010190 "" WorkerW
            //   ...
            //   0x000100EE "" SHELLDLL_DefView
            //     0x000100F0 "FolderView" SysListVieWin32Wrapper
            // 0x000100EC "Program Manager" Progman



            // Fetch the Progman window
            IntPtr progman = Win32Wrapper.FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;

            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            Win32Wrapper.SendMessageTimeout(progman,
                                   0x052C,
                                   new IntPtr(0),
                                   IntPtr.Zero,
                                   Win32Wrapper.SendMessageTimeoutFlags.SMTO_NORMAL,
                                   1000,
                                   out result);


            PrintVisibleWindowHandles(2);
            // The output will look something like this
            // .....
            // 0x00010190 "" WorkerW
            //   ...
            //   0x000100EE "" SHELLDLL_DefView
            //     0x000100F0 "FolderView" SysListVieWin32Wrapper
            // 0x00100B8A "" WorkerW                                   <--- This is the WorkerW instance we are after!
            // 0x000100EC "Program Manager" Progman

            IntPtr workerw = IntPtr.Zero;

            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
            Win32Wrapper.EnumWindows(new Win32Wrapper.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = Win32Wrapper.FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            IntPtr.Zero);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = Win32Wrapper.FindWindowEx(IntPtr.Zero,
                                               tophandle,
                                               "WorkerW",
                                               IntPtr.Zero);
                    return false;
                }

                return true;
            }), IntPtr.Zero);

            Win32Wrapper.SetParent(handle, workerw);
        }
    }
}
