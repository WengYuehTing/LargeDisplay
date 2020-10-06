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
using System.Windows.Media.Imaging;

namespace BodySee.Windows
{
    /// <summary>
    /// Menu.xaml 的交互逻辑
    /// </summary>
    public partial class Menu : Window
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
            double width = SystemParameters.VirtualScreenWidth;
            double height = SystemParameters.VirtualScreenHeight;

            using (BitmapFrame bmp = new BitmapFrame((int)width, (int)height))
            {

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
                ScreenShot();
        }

        private void Background_Loaded(object sender, RoutedEventArgs e)
        {
            background.Focus();
        }
    }
}
