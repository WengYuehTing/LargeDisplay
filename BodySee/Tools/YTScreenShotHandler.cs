using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BodySee.Tools
{
    //TODO add a audio feedback.
    class YTScreenShotHandler
    {
        /// <summary>
        /// Take a screenshot and store into local machine.
        /// </summary>
        /// <param name="path"></param>
        public static void ScreenShot(string path="")
        {
            double left = SystemParameters.VirtualScreenLeft;
            double top = SystemParameters.VirtualScreenTop;
            double width = SystemParameters.VirtualScreenWidth * 2;
            double height = SystemParameters.VirtualScreenHeight * 2;
            if (path == "" || path.ToLower() == "default") // SaveFileDialog
            {
                SaveFileDialog f = new SaveFileDialog();
                f.FileName = "screenshot" + DateTime.Now.ToString("yyyyMMddhhmmss");
                f.DefaultExt = ".jpg";
                f.Filter = "Image (.jpg .png) | *.jpg *.png";
                if (f.ShowDialog() == true)
                {
                    Task.Delay(500).ContinueWith(_ =>
                    {
                        Bitmap bmp = new Bitmap((int)width, (int)height);
                        Graphics g = Graphics.FromImage(bmp);
                        g.CopyFromScreen((int)left, (int)top, 0, 0, bmp.Size);
                        bmp.Save(f.FileName);
                    });
                }
            } else
            {
                Bitmap bmp = new Bitmap((int)width, (int)height);
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen((int)left, (int)top, 0, 0, bmp.Size);
                bmp.Save(path);
            }
        }
    }
}
