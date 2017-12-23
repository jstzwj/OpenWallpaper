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
        }

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                _window.KeyEventHandler((Win32Wrapper.KeyMessages)wParam, vkCode);
            }

            return Win32Wrapper.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
