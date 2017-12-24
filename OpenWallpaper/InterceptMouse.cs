using OpenWallpaper.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using static OpenWallpaper.Win32Wrapper;

namespace OpenWallpaper
{
    public class InterceptMouse
    {
        public static LowLevelMouseProc _proc = HookCallback;
        public static IntPtr _hookID = IntPtr.Zero;
        public static MainWindow _window;

        public static IntPtr SetHook(LowLevelMouseProc proc)
        {

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx((int)HookType.WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
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

            //        hwndDesktop = Win32Wrapper.FindWindowEx(p,
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
            //    return SetWindowsHookEx((int)HookType.WH_MOUSE_LL, proc,
            //        GetModuleHandle(curModule.ModuleName), GetWindowThreadProcessId(hwndDesktop, IntPtr.Zero));
            //}
        }

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                // Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);
                // if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
                // {
                    // SendMessage(_windowHandle.Handle, (uint)MouseMessages.WM_SETCURSOR, _windowHandle.Handle, new IntPtr((uint)HitTestValues.HTCLIENT | (uint)MouseMessages.WM_MOUSEMOVE << 16));
                    // PostMessage(_windowHandle, (uint)wParam.ToInt32(), IntPtr.Zero, new IntPtr((uint)hookStruct.pt.x | ((uint)hookStruct.pt.y << 16)));
                // }
                _window.MouseEventHandler((MouseMessages)wParam, hookStruct.pt.x, hookStruct.pt.y);
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
