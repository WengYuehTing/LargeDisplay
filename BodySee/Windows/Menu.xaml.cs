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
    public partial class Menu : Window
    {
        #region Fields
        private WhiteBoard              _whiteBoard;
        private YTMenuMovingComponent   _MovingComponent;
        private VolumeWindow            _VolumeWindow;
        private BrightnessWindow        _BrightnessWindow;
        private AppList                 _AppList;
        #endregion

        public Menu()
        {
            InitializeComponent();
            this.Width = WindowsHandler.GetScreenWidth() * WindowsHandler.WRATIO;
            this.Height = WindowsHandler.GetScreenHeight() * WindowsHandler.HRATIO;
           
            Client client = new Client();
            TaskManager.getInstance().menu = this;
            _MovingComponent = new YTMenuMovingComponent(this, MovingMode.X);
        }
           
        public YTMenuMovingComponent GetComponent()
        {
            return _MovingComponent;
        }


        #region UI Events
        /// <summary>
        /// Toggle whiteboard. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhiteBoardIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (!WindowsHandler.IsWindowOpen<WhiteBoard>())
            {
                _whiteBoard = new WhiteBoard();
                _whiteBoard.Show();
                WhiteBoardIcon.Source = new BitmapImage(new Uri(@"/Images/新建_高光.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                _whiteBoard.Close();
                _whiteBoard = null;
                WhiteBoardIcon.Source = new BitmapImage(new Uri(@"/Images/新建.png", UriKind.RelativeOrAbsolute));
            }
        }

        private void UndoIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (_whiteBoard != null)
                _whiteBoard.Undo();
        }

        private void RedoIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (_whiteBoard != null)
                _whiteBoard.Redo();
        }

        private void ScreenshotIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (_whiteBoard != null)
                _whiteBoard.Clear();
        }

        private void DesktopIcon_TouchDown(object sender, TouchEventArgs e)
        {
            WindowsHandler.ReturnToDesktop();
        }

        private void AppListIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (_VolumeWindow != null)
                CloseVolumeWindow();

            if (_BrightnessWindow != null)
                CloseBrightnessWindow();

            if (_AppList != null)
                CloseAppListWindow();
            else
                OpenAppListWindow();
        }

        private void VolumeIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (_BrightnessWindow != null)
                CloseBrightnessWindow();

            if (_AppList != null)
                CloseAppListWindow();

            if (_VolumeWindow != null)
                CloseVolumeWindow();
            else
                OpenVolumeWindow();
            //TODO fix a position.
        }

        private void BrightnessIcon_TouchDown(object sender, TouchEventArgs e)
        {
            if (_VolumeWindow != null)
                CloseVolumeWindow();

            if (_AppList != null)
                CloseAppListWindow();

            if (_BrightnessWindow != null)
                CloseBrightnessWindow();
            else
                OpenBrightnessWindow();

            //TODO fix a position.
        }

        private void Menu_LocationChanged(object sender, EventArgs e)
        {
            //TODO Make volume, brightness, app list follow menu
            if(_VolumeWindow != null)
            {
                _VolumeWindow.Left = this.Left;
                _VolumeWindow.Top = this.Top + this.Height;
            }

            if(_BrightnessWindow != null)
            {
                _BrightnessWindow.Left = this.Left;
                _BrightnessWindow.Top = this.Top + this.Height;
            }

            if(_AppList != null)
            {
                _AppList.Left = this.Left;
                _AppList.Top = this.Top + this.Height;
            }


        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized || WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;
        }

        private void OpenVolumeWindow()
        {
            _VolumeWindow = new VolumeWindow(this);
            _VolumeWindow.Show();
            VolumeIcon.Source = new BitmapImage(new Uri(@"/Images/音量_高光.png", UriKind.RelativeOrAbsolute));
        }

        private void CloseVolumeWindow()
        {
            _VolumeWindow.Close();
            _VolumeWindow = null;
            VolumeIcon.Source = new BitmapImage(new Uri(@"/Images/音量.png", UriKind.RelativeOrAbsolute));
        }

        private void OpenBrightnessWindow()
        {
            _BrightnessWindow = new BrightnessWindow(this);
            _BrightnessWindow.Show();
            BrightnessIcon.Source = new BitmapImage(new Uri(@"/Images/亮度_高光.png", UriKind.RelativeOrAbsolute));
        }

        private void CloseBrightnessWindow()
        {
            _BrightnessWindow.Close();
            _BrightnessWindow = null;
            BrightnessIcon.Source = new BitmapImage(new Uri(@"/Images/亮度.png", UriKind.RelativeOrAbsolute));
        }

        private void OpenAppListWindow()
        {
            _AppList = new AppList(this);
            _AppList.Show();
            AppListIcon.Source = new BitmapImage(new Uri(@"/Images/后台管理_高光.png", UriKind.RelativeOrAbsolute));
        }

        private void CloseAppListWindow()
        {
            _AppList.Close();
            _AppList = null;
            AppListIcon.Source = new BitmapImage(new Uri(@"/Images/后台管理.png", UriKind.RelativeOrAbsolute));
        }
        #endregion


        #region Debug
        private void Background_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Q))
                WindowsHandler.AcquirePriortyofScreenTouch();

            if (e.KeyStates == Keyboard.GetKeyStates(Key.W))
                WindowsHandler.BlockingScreenTouch();

            if (e.KeyStates == Keyboard.GetKeyStates(Key.E))
                YTDataDecoder.run();

            if (e.KeyStates == Keyboard.GetKeyStates(Key.R))
                Console.WriteLine(BrightnessAdjuster.getInstance().Brightness);
        }

        private void Background_Loaded(object sender, RoutedEventArgs e)
        {
            background.Focus();
        }

        #endregion

        
        
    }
}
