using BodySee.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace BodySee.Windows
{
    /// <summary>
    /// GestureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GestureWindow : Window
    {
        #region Private Fields
        private IntPtr                          _hwnd;
        private Dictionary<int, List<Point>>    finger1;
        private Dictionary<int, List<Point>>    finger2;
        private bool                            bLongPressing;
        private bool                            bManipulating;
        private DispatcherTimer                 longPressTimer;
        #endregion

        #region Threshold Definitions
        
        //滑动至少需要滑动的距离
        private const int SWIPE_THRESHOLD = 10;
        
        //滑动时对于另外一个方向的容忍度
        private const int SWIPE_LIMITED = 100;

        //长按至少需要维持几秒
        private const int LONG_PRESS_DURATION = 500;

        //长按时对于手抖的容忍度
        private const int LONG_PRESS_LIMITED = 20;
        #endregion

        public GestureWindow()
        {
            InitializeComponent();

            finger1 = new Dictionary<int, List<Point>>();
            finger2 = new Dictionary<int, List<Point>>();
            bLongPressing = false;
            bManipulating = false;
            longPressTimer = new DispatcherTimer();
            longPressTimer.Interval = TimeSpan.FromMilliseconds(LONG_PRESS_DURATION);
            longPressTimer.Tick += (sender, args) =>
            {
                longPressTimer.Stop();
                bLongPressing = true;
                Console.WriteLine("Long Press");
            };
        }
        
        private void GestureButton_TouchDown(object sender, TouchEventArgs e)
        {
            // 长按开始计时
            longPressTimer.Start();

            // 找到目标窗口
            _hwnd = WindowsHandler.FindTopmostWindow();

            // 初始化起始点
            int id = e.TouchDevice.Id;
            Point start = this.PointToScreen(new Point(e.GetTouchPoint(this).Position.X, e.GetTouchPoint(this).Position.Y));   
            if (finger1.Count == 0)
                finger1.Add(id, new List<Point>() { start });
            else
                finger2.Add(id, new List<Point>() { start });

        }

        private void GestureButton_TouchMove(object sender, TouchEventArgs e)
        {
            int id = e.TouchDevice.Id;
            Point position = this.PointToScreen(new Point(e.GetTouchPoint(this).Position.X, e.GetTouchPoint(this).Position.Y));
            if (finger1.ContainsKey(id))
            {
                finger1[id].Add(position);
                if (longPressTimer.IsEnabled && Point.Subtract(position, finger1[id][0]).Length > LONG_PRESS_LIMITED)
                    longPressTimer.Stop();
            }
            else if (finger2.ContainsKey(id))
            {
                finger2[id].Add(position);
                if (longPressTimer.IsEnabled && Point.Subtract(position, finger2[id][0]).Length > LONG_PRESS_LIMITED)
                    longPressTimer.Stop();
            }

            if(!longPressTimer.IsEnabled && bLongPressing && finger1.Count != 0 && finger2.Count != 0)
                bManipulating = true;
        }

        private void GestureButton_TouchUp(object sender, TouchEventArgs e)
        {
            if (longPressTimer.IsEnabled)
                longPressTimer.Stop();
            
            //if (finger1.Count != 0 && finger2.Count != 0 && !bLongPressing)
            //{
            //    var list1 = finger1.ElementAt(0).Value;
            //    var list2 = finger2.ElementAt(0).Value;

            //    int n1 = list1.Count;
            //    int n2 = list2.Count;
            //    if(list1[n1 - 1].Y - list1[0].Y > SWIPE_THRESHOLD && list2[n2 - 1].Y - list2[0].Y > SWIPE_THRESHOLD && Math.Abs(list1[n1 - 1].X - list1[0].X) < SWIPE_LIMITED && Math.Abs(list2[n2 - 1].X - list2[0].X) < SWIPE_LIMITED)
            //    {
            //        Console.WriteLine("双指下滑");
            //        WindowsHandler.MinimizeWindow(_hwnd);
            //    }
            //    else if (list1[0].Y - list1[n1 - 1].Y > SWIPE_THRESHOLD && list2[0].Y - list2[n2 - 1].Y > SWIPE_THRESHOLD && Math.Abs(list1[n1 - 1].X - list1[0].X) < SWIPE_LIMITED && Math.Abs(list2[n2 - 1].X - list2[0].X) < SWIPE_LIMITED)
            //    {
            //        Console.WriteLine("双指上滑");
            //        WindowsHandler.MaximizeWindow(_hwnd);
            //    }
            //    else if (list1[n1 - 1].X - list1[0].X > SWIPE_THRESHOLD && list2[n2 - 1].X - list2[0].X > SWIPE_THRESHOLD && Math.Abs(list1[n1 - 1].Y - list1[0].Y) < SWIPE_LIMITED && Math.Abs(list2[n2 - 1].Y - list2[0].Y) < SWIPE_LIMITED)
            //    {
            //        Console.WriteLine("双指右滑");
            //        WindowsHandler.RestoreWindow(_hwnd);
            //    }
            //    else if (list1[0].X - list1[n1 - 1].X > SWIPE_THRESHOLD && list2[0].X - list2[n2 - 1].X > SWIPE_THRESHOLD && Math.Abs(list1[n1 - 1].Y - list1[0].Y) < SWIPE_LIMITED && Math.Abs(list2[n2 - 1].Y - list2[0].Y) < SWIPE_LIMITED)
            //    {
            //        Console.WriteLine("双指左滑");
            //        WindowsHandler.CloseWindow(_hwnd);
            //    }
            //}

            finger1 = new Dictionary<int, List<Point>>();
            finger2 = new Dictionary<int, List<Point>>();
            bLongPressing = false;
            bManipulating = false;
            _hwnd = IntPtr.Zero;
        }

        private void GestureButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Q))
                this.Close();
        }

        readonly Stopwatch _manipulationTimer = new Stopwatch();
        private void GestureButton_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            _manipulationTimer.Restart();
            e.ManipulationContainer = this;
            e.Handled = true;
            
        }

        private void GestureButton_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if(bManipulating)
            {
                double translate_x = e.DeltaManipulation.Translation.X * 2;
                double translate_y = e.DeltaManipulation.Translation.Y * 2;
                double scale_x = e.DeltaManipulation.Scale.X;
                double scale_y = e.DeltaManipulation.Scale.Y;
                WindowsHandler.MoveWindow(_hwnd, (int)translate_x, (int)translate_y);
                //WindowsHandler.ScaleWindow(_hwnd, scale_x, scale_y);
                e.Handled = true;
            }
        }
        
        private void GestureButton_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);
            e.Handled = true;
        }

        private void GestureButton_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var millis = _manipulationTimer.ElapsedMilliseconds;
            Console.WriteLine();
        }
    }
}
