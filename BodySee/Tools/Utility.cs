using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;

namespace BodySee.Tools
{
    class Utility
    {
        // MainWindow's width equals to screen width * wRATIO
        public static double wRATIO = 0.2f;


        // MainWindow's height equals to screen height * hRATIO
        public static double hRATIO = 0.15f;


        // The default shape of eraser
        public static EllipseStylusShape ERASER_SIZE = new EllipseStylusShape(50, 50);


        public Utility()
        {
            var width = Application.Current.MainWindow.Width;
            var height = Application.Current.MainWindow.Height;
            Debug.WriteLine(getScreenWidth());
        }

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
