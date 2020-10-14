using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BodySee.Tools;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Windows.Interop;

namespace BodySee.Windows
{
    /// <summary>
    /// Menu.xaml 的交互逻辑
    /// </summary>
    public partial class Menu : Window, IVolumeInterface
    {
        #region Private Fields
        private WhiteBoard _whiteBoard;
        #endregion

        public Menu()
        {
            InitializeComponent();
            this.Width = WindowsHandler.GetScreenWidth() * WindowsHandler.WRATIO;
            this.Height = WindowsHandler.GetScreenHeight() * WindowsHandler.HRATIO;
           
            Client client = new Client();
            TaskManager.getInstance().menu = this;
            VolumeAdjuster.getInstance().Delegate = this;
        }

        public void moveWindow(double x)
        {
            this.Left += x;
        }

        public void moveWindow(double x, double y)
        {
            this.Left += x;
            this.Top += y;
        }

        public void ScreenShot()
        {
            double left = SystemParameters.VirtualScreenLeft;
            double top = SystemParameters.VirtualScreenTop;
            double width = SystemParameters.VirtualScreenWidth*2;
            double height = SystemParameters.VirtualScreenHeight*2;
            
            SaveFileDialog f = new SaveFileDialog();
            f.FileName = "screenshot" + DateTime.Now.ToString("yyyyMMddhhmmss");
            f.DefaultExt = ".jpg";
            f.Filter = "Image (.jpg .png) | *.jpg *.png";
            if(f.ShowDialog() == true)
            {
                Task.Delay(500).ContinueWith(_ =>
                {
                    Bitmap bmp = new Bitmap((int)width, (int)height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen((int)left, (int)top, 0, 0, bmp.Size);
                    bmp.Save(f.FileName);
                });
            }
        }


        private void WhiteBoardIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (!WindowsHandler.IsWindowOpen<WhiteBoard>())
            {
                _whiteBoard = new WhiteBoard();
                _whiteBoard.Show();
            }
            else
            {
                _whiteBoard.Close();
                _whiteBoard = null;
            }
        }

        
        private void Background_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Q))
                WindowsHandler.AcquirePriortyofScreenTouch();

            if (e.KeyStates == Keyboard.GetKeyStates(Key.W))
                WindowsHandler.BlockingScreenTouch();

            if (e.KeyStates == Keyboard.GetKeyStates(Key.E))
            {
                BrightnessAdjuster.getInstance().SetBrightness(180);
            }
                
        }

        private void Background_Loaded(object sender, RoutedEventArgs e)
        {
            background.Focus();
        }

        #region Interface Methods
        /// <summary>
        /// Implementation of IVolumeInterface methods.
        /// </summary>
        /// <param name="_vol"></param>
        public void OnVolumeChange(double _vol)
        {
            if(_vol == 0)
                VolumeAdjuster.getInstance().SetMute(true);
            else
                VolumeAdjuster.getInstance().SetMute(false);
        }

        /// <summary>
        /// Implementation of IVolumeInterface methods.
        /// </summary>
        /// <param name="_vol"></param>
        public void OnMutedChange(bool _mute) {}
        #endregion
    }
}
