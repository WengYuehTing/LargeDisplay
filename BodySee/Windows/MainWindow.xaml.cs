using BodySee.Tools;
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

namespace BodySee.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields
        private WhiteBoard _whiteBoard;  
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.Width = WindowsHandler.GetScreenWidth() * WindowsHandler.WRATIO;
            this.Height = WindowsHandler.GetScreenHeight() * WindowsHandler.HRATIO;
            Client client = new Client();
            TaskManager.getInstance().mainWindow = this;
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
            {
                _whiteBoard = new WhiteBoard(this);
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
        }

        private void Background_Loaded(object sender, RoutedEventArgs e)
        {
            background.Focus();
        }
    }
}
