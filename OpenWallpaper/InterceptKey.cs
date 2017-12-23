using OpenWallpaper.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenWallpaper
{
    class InterceptKey
    {
        public static Win32Wrapper.LowLevelKeyboardProc _proc = HookCallback;

        public static IntPtr _hookID = IntPtr.Zero;

        public static MainWindow _window;

        public static IntPtr SetHook(Win32Wrapper.LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return Win32Wrapper.SetWindowsHookEx((int)Win32Wrapper.HookType.WH_KEYBOARD_LL, proc,
                    Win32Wrapper.GetModuleHandle(curModule.ModuleName), 0);
            }

            //IntPtr hwndWorkerW = IntPtr.Zero;
            //IntPtr hShellDefView = IntPtr.Zero;
            //IntPtr hwndDesktop = IntPtr.Zero;

            //IntPtr hProgMan = Win32Wrapper.FindWindow("ProgMan", null);
            //Win32Wrapper.EnumWindows(new Win32Wrapper.EnumWindowsProc((tophandle, topparamhandle) =>
            //{
            //    IntPtr p = Win32Wrapper.FindWindowEx(tophandle,
            //                                IntPtr.Zero,
            //                                "SHELLDLL_DefView",
            //                                null);

            //    if (p != IntPtr.Zero)
            //    {
            //        // Gets the WorkerW Window after the current one.
            //        hwndWorkerW = Win32Wrapper.FindWindowEx(IntPtr.Zero,
            //                                   tophandle,
            //                                   "WorkerW",
            //                                   null);

            //        hwndDesktop = Win32Wrapper.FindWindowEx(tophandle,
            //                                    IntPtr.Zero,
            //                                   "SysListView32",
            //                                   null);
            //        return false;
            //    }
            //    return true;
            //}), IntPtr.Zero);

            //using (Process curProcess = Process.GetCurrentProcess())
            //using (ProcessModule curModule = curProcess.MainModule)
            //{
            //    return Win32Wrapper.SetWindowsHookEx((int)Win32Wrapper.HookType.WH_KEYBOARD_LL, proc,
            //        Win32Wrapper.GetModuleHandle(curModule.ModuleName), Win32Wrapper.GetWindowThreadProcessId(hwndDesktop, IntPtr.Zero));
            //}
        }

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                _window.KeyEventHandler((Win32Wrapper.KeyMessages)wParam, (int)wParam, vkCode);
            }

            return Win32Wrapper.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
