using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using BodySee.Windows;

namespace BodySee.Tools
{
    class WinApiManager
    {
        public enum GetAncestorFlags
        {
            GetParent = 1,
            GetRoot = 2,
            GetRootOwner = 3
        }

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filter) : this()
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
            }
        }


        #region Win32 API 
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, int lParam);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hwnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hWnd, int dwAttribute, out int pvAttribute, int cbAttribute);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, uint dwAttibute, out uint pvAttribute, uint cbAttribute);

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hwnd);

        public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
        #endregion

        #region Windows Message Code
        public static UInt32 WM_CLOSE = 0x0010;
        public static UInt32 WM_MOVE = 0x0003;
        public static UInt32 WM_SYSCOMMAND = 0x0112;
        public static UInt32 SC_MINIMIZE = 0xF020;
        public static UInt32 SC_MAXIMIZE = 0xF030;
        public static UInt32 SC_RESTORE = 0xF120;
        public static UInt32 SC_NEXTWINDOW = 0xF040;
        public static UInt32 SC_LASTWINDOW = 0xF050;
        public static UInt32 SC_MOVE = 0xF010;
        public static UInt32 SC_SIZE = 0xF000;
        public static int SWP_NOMOVE = 0x0002;
        public static int SWP_NOSIZE = 0x0001;
        public static int SWP_NOACTIVATE = 0x0010;
        public static int SWP_NOZORDER = 0x0004;
        public static int SWP_SHOWWINDOW = 0x0040;
        #endregion

        //private static List<IntPtr> EnumerateWindow()
        //{
        //    var lShellWin = GetShellWindow();
        //    var currentProcessId = Process.GetCurrentProcess().Id;
        //    List<IntPtr> windows = new List<IntPtr>();
        //    EnumWindows(delegate (IntPtr wnd, int param)
        //    {
        //        if (!IsWindowValid(wnd))
        //            return true;

        //        var entry = WindowEntryFactory.Create(wnd);

        //        if (entry == null || entry.ProcessId == currentProcessId)
        //            return true;

        //        windows.Add(wnd);
        //        return true;
        //    }, 0);


        //    Console.WriteLine(windows.Count);
        //    foreach (IntPtr hWnd in windows)
        //    {
        //        Console.WriteLine(GetTitleFromHandle(hWnd));
        //    }

        //    return windows;
        //}
    }
}
