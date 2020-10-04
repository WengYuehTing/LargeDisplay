using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

namespace BodySee
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

        public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

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
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hWnd, int dwAttribute, out int pvAttribute, int cbAttribute);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        #endregion

        #region Window Message Code
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
        private static int SWP_NOMOVE = 0x0002;
        private static int SWP_NOSIZE = 0x0001;
        private static int SWP_NOACTIVATE = 0x0010;
        private static int SWP_NOZORDER = 0x0004;
        private static int SWP_SHOWWINDOW = 0x0040;
        #endregion

        #region Window Operation
        public static void CloseWindow()
        {
            Console.WriteLine("Close Window");
            IntPtr hWnd = FindTopmostWindow();
            if (hWnd != IntPtr.Zero)
            {
                SendMessage(hWnd, WM_CLOSE, 0, IntPtr.Zero);
            }
            
        }

        public static void MinimizeWindow()
        {
            Console.WriteLine("Minimize Window");
            IntPtr hWnd = FindTopmostWindow();
            if (hWnd != IntPtr.Zero)
            {
                SendMessage(hWnd, WM_SYSCOMMAND, SC_MINIMIZE, IntPtr.Zero);
            } 
        }

        public static void MaximizeWindow()
        {
            Console.WriteLine("Maximize Window");
            IntPtr hWnd = FindTopmostWindow();
            if (hWnd != IntPtr.Zero)
            {
                SendMessage(hWnd, WM_SYSCOMMAND, SC_MAXIMIZE, IntPtr.Zero);
            }
        }

        public static void RestoreWindow()
        {
            //Console.WriteLine("Restore Window");
            //if (targetHwnd != IntPtr.Zero)
            //{
            //    SendMessage(targetHwnd, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero);
            //}
        }

        public static void MoveWindow(IntPtr hwnd, int xOffset, int yOffset)
        {
            if (hwnd == IntPtr.Zero)
                return;
            RECT rect = GetWindowRect(hwnd);
            int width = Math.Abs(rect.Right - rect.Left);
            int height = Math.Abs(rect.Bottom - rect.Top);
            int x = Math.Max(0 - width/2, Math.Min((int)Utility.getScreenWidth()+width/2, rect.Left + xOffset));
            int y = Math.Max(0, Math.Min((int)Utility.getScreenHeight(), rect.Top + yOffset));
            SetWindowPos(hwnd, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOZORDER | SWP_SHOWWINDOW);
        }

        public static void ScaleWindow(IntPtr hwnd, double xRatio, double yRatio)
        {
            if (hwnd == IntPtr.Zero)
                return;

            RECT rect = GetWindowRect(hwnd);
            int width = Math.Abs(rect.Right - rect.Left);
            int height = Math.Abs(rect.Bottom - rect.Top);
            int x = rect.Left;
            int y = rect.Top;
            int cx = x + width / 2;
            int cy = y + height / 2;
            int nWidth = (int)((double)width * xRatio);
            int nHeight = (int)((double)height * yRatio);
            int nx = cx - nWidth / 2;
            int ny = cy - nHeight / 2;
            SetWindowPos(hwnd, IntPtr.Zero, nx, ny, nWidth, nHeight, SWP_NOACTIVATE | SWP_NOZORDER | SWP_SHOWWINDOW);
        }



        /// <summary>
        /// Disable touch event and detect touching gestural input.
        /// </summary>
        public static void TakeOverOperation()
        {
            if (!Utility.IsWindowOpen<GestureWindow>())
            {
                GestureWindow gestureWin = new GestureWindow();
                gestureWin.Show();
            }
            else
            {
                foreach (Window win in Application.Current.Windows)
                {
                    if (win.Title == "GestureWindow")
                    {
                        win.Close();
                    }
                }
            }
        }

        public static RECT GetWindowRect(IntPtr hwnd)
        {
            RECT rect = new RECT();
            GetWindowRect(hwnd, out rect);
            return rect;
        }

        /// <summary>
        /// Disable or enable touch events.
        /// </summary>
        public static void HookTouchEvents(bool yes)
        {
            if(yes)
            {
                if (!Utility.IsWindowOpen<BlockingWindow>())
                {
                    BlockingWindow blockingWindow = new BlockingWindow();
                    blockingWindow.Show();
                }
            } else
            {
                foreach (Window win in Application.Current.Windows)
                {
                    if (win.Title == "BlockingWindow")
                    {
                        win.Close();
                    }
                }
            }
        }
        #endregion

        #region Helper Method
        public static int GetWindowZOrder(IntPtr hWnd)
        {
            var zOrder = -1;
            while ((hWnd = GetWindow(hWnd, 2)) != IntPtr.Zero) zOrder++;
            return zOrder;
        }

        public static string GetTitleOfForegroundWindow()
        {
            const int nChars = 256;
            StringBuilder buff = new StringBuilder(nChars);
            IntPtr hwnd = GetForegroundWindow();

            if (GetWindowText(hwnd, buff, nChars) > 0)
            {
                return buff.ToString();
            }

            return null;
        }

        public static String GetTitleFromHandle(IntPtr hWnd)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            if (GetWindowText(hWnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }

            return null;
        }



        public static List<IntPtr> EnumerateWindow()
        {
            var lShellWin = GetShellWindow();
            var currentProcessId = Process.GetCurrentProcess().Id;
            List<IntPtr> windows = new List<IntPtr>();
            EnumWindows(delegate (IntPtr wnd, int param)
            {
                if (!IsWindowValid(wnd))
                    return true;

                var entry = WindowEntryFactory.Create(wnd);

                if (entry == null || entry.ProcessId == currentProcessId)
                    return true;

                windows.Add(wnd);
                return true;
            }, 0);


            Console.WriteLine(windows.Count);
            foreach (IntPtr hWnd in windows)
            {
                Console.WriteLine(GetTitleFromHandle(hWnd));
            }

            return windows;
        }

        private static bool IsWindowValid(IntPtr window)
        {
            if (window == GetShellWindow())
                return false;

            IntPtr root = GetAncestor(window, GetAncestorFlags.GetRootOwner);

            if (GetLastVisibleActivePopUpOfWindow(root) != window)
                return false;

            if (GetTitleFromHandle(window) == null)
                return false;

            if (!IsWindowVisible(window))
                return false;

            var classNameStringBuilder = new StringBuilder(256);
            var length = GetClassName(window, classNameStringBuilder, classNameStringBuilder.Capacity);
            if (length == 0)
                return false;

            string[] classNameToSkip =
            {
                "Shell_TrayWnd",
                "DV2ControlHost",
                "MsgrIMEWindowClass",
                "SysShadow",
                "Button"
            };

            var className = classNameStringBuilder.ToString();
            if (Array.IndexOf(classNameToSkip, className) > -1)
                return false;

            if (className.StartsWith("WMP9MediaBarFlyout"))
                return false;

            return true;
        }

        private static IntPtr GetLastVisibleActivePopUpOfWindow(IntPtr window)
        {
            IntPtr lastPopUp = GetLastActivePopup(window);
            if (IsWindowVisible(lastPopUp))
                return lastPopUp;
            else if (lastPopUp == window)
                return IntPtr.Zero;
            else
                return GetLastVisibleActivePopUpOfWindow(lastPopUp);
        }

        public static IntPtr FindTopmostWindow()
        {
            List<IntPtr> hWnds = GetAllWindowHandle();
            if(hWnds.Count == 0)
            {
                return IntPtr.Zero;
            }

            int max = -1;
            IntPtr result = IntPtr.Zero;
            foreach(IntPtr hWnd in hWnds)
            {
                var z = GetWindowZOrder(hWnd);
                if(z > max)
                {
                    max = z;
                    result = hWnd;
                }
            }
            return result;
        }

        public static List<IntPtr> GetAllWindowHandle()
        {
            Process[] processList = Process.GetProcesses();
            int count = 0;
            List<IntPtr> list = new List<IntPtr>();
            foreach (Process p in processList)
            {
                IntPtr hWnd = p.MainWindowHandle;
                if (IsWindowValid(hWnd) && GetTitleFromHandle(hWnd) != "工具栏" && GetTitleFromHandle(hWnd) != "GestureWindow")
                {
                    list.Add(hWnd);
                    //Debug.WriteLine("Title: {0} ZOrder: {1}", p.MainWindowTitle, GetWindowZOrder(p.MainWindowHandle));
                    //Debug.WriteLine("Process: {0} ID: {1}, Window title: {2}", p.ProcessName, p.Id, p.MainWindowTitle);
                }
            }

            //Console.WriteLine("Found {0} windows", list.Count);
            return list;
        }
    }
    #endregion


}
