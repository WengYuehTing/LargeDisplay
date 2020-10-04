using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BodySee.Windows;

namespace BodySee.Tools
{
    class WindowsHandler
    {
        // Toolbar's width equals to screen width * WRATIO
        public static double WRATIO = 0.2f;

        // Toolbar's height equals to screen height * HATIO
        public static double HRATIO = 0.15f;

        // Definition of screen touch mode.
        public static ScreenTouchMode ScreenTouchMode = ScreenTouchMode.Normal;


        #region Methods Relate to Screen Touch 
        /// <summary>
        /// Trigger while a user faces to screen. 
        /// </summary>
        public static void FaceToScreen()
        {
            ScreenTouchMode = ScreenTouchMode.Normal;
            if (IsWindowOpen<BlockingWindow>())
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "BlockingWindow" || window.Title == "GestureWindow")
                    {
                        window.Close();
                    }
                }
            }

        }

        public static void BackOnToScreen()
        {
            ScreenTouchMode = ScreenTouchMode.Blocking;

            if (!IsWindowOpen<BlockingWindow>())
            {
                BlockingWindow win = new BlockingWindow();
                win.Show();
            }
        }

        public static void TwoFingersApporaching()
        {
            ScreenTouchMode = ScreenTouchMode.BlockingAndGesture;

            if (!IsWindowOpen<GestureWindow>())
            {
                GestureWindow win = new GestureWindow();
                win.Show();
            }
        }

        public static void TwoFingersRelease()
        {
            ScreenTouchMode = ScreenTouchMode.Normal;
            if(IsWindowOpen<GestureWindow>())
            {
                foreach(Window window in Application.Current.Windows)
                {
                    if(window.Title == "GestureWindow")
                    {
                        window.Close();
                        return;
                    }
                }
            }
        }
        #endregion

        public static double getScreenWidth()
        {
            return SystemParameters.PrimaryScreenWidth;
        }

        public static double getScreenHeight()
        {
            return SystemParameters.PrimaryScreenHeight;
        }

        public static Size getScreenSize()
        {
            var width = System.Windows.SystemParameters.PrimaryScreenWidth;
            var height = System.Windows.SystemParameters.PrimaryScreenHeight;
            return new Size(width, height);
        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
                ? Application.Current.Windows.OfType<T>().Any()
                : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
    }
}
