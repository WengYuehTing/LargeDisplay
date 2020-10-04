using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BodySee
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields
        private double width;
        private double height;
        private Size size;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            resizeWindow();
            Client client = new Client();
            TaskManager.getInstance().mainWindow = this;
            
        }
        
        private void resizeWindow()
        {
            width = Utility.getScreenWidth() * Utility.wRATIO;
            height = Utility.getScreenHeight() * Utility.hRATIO;
            size = new Size(width, height);
            this.Width = width;
            this.Height = height;
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
        

        private void WhiteBoardIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (!Utility.IsWindowOpen<WhiteBoard>())
                new WhiteBoard(this).Show();
            else
            {
                foreach (Window win in Application.Current.Windows)
                {
                    if (win.Title == "WhiteBoard")
                    {
                        win.Close();
                    }
                }
            }
        }
        

        private void Background_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Z))
            {
                IntPtr hwnd = WinApiManager.FindWindowByCaption(IntPtr.Zero, "WhiteBoard");
                if(hwnd != IntPtr.Zero)
                {
                    WinApiManager.SendMessage(hwnd, WinApiManager.WM_CLOSE, 0, IntPtr.Zero);
                } 
            }

            if (e.KeyStates == Keyboard.GetKeyStates(Key.Q))
            {
                WinApiManager.TakeOverOperation();
            }

            if (e.KeyStates == Keyboard.GetKeyStates(Key.W))
            {
                WinApiManager.GetAllWindowHandle();
            }

            if (e.KeyStates == Keyboard.GetKeyStates(Key.E))
            {
                WinApiManager.HookTouchEvents(true);
            }
        }

        private void Background_Loaded(object sender, RoutedEventArgs e)
        {
            background.Focus();
        }
    }
}
