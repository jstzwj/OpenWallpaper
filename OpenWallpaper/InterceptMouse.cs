﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using static OpenWallpaper.Win32Wrapper;

namespace OpenWallpaper
{
    public class InterceptMouse
    {
        public static LowLevelMouseProc _proc = HookCallback;
        public static IntPtr _hookID = IntPtr.Zero;
        public static HandleRef _windowHandle;

        public static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                // Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);
                if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
                    PostMessage(_windowHandle, (uint)MouseMessages.WM_MOUSEMOVE, IntPtr.Zero, new IntPtr((uint)hookStruct.pt.x | ((uint)hookStruct.pt.y << 16)));
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
