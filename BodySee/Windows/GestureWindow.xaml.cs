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
        private Dictionary<int, List<YTPoint>>    finger1;
        private Dictionary<int, List<YTPoint>>    finger2;
        private Stopwatch                       _watch1;
        private Stopwatch                       _watch2;
        private bool                            _bManipulating1;
        private bool                            _bManipulating2;
        private bool                            _bSwipe1;
        private bool                            _bSwipe2;
        private long                            _currentTime;
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
        //判断Drag和Swipe区别时的圆半径
        private const int RADIUS = 600;
        private const int TIME_THRESHOLD = 500;
        #endregion

        public GestureWindow()
        {
            InitializeComponent();

            finger1 = new Dictionary<int, List<YTPoint>>();
            finger2 = new Dictionary<int, List<YTPoint>>();
            _bSwipe1 = false;
            _bSwipe2 = false;
            _bManipulating1 = false;
            _bManipulating2 = false;
        }
        
        
        private void GestureButton_TouchDown(object sender, TouchEventArgs e)
        {
            // 找到目标窗口
            _hwnd = WindowsHandler.FindTopmostWindow();

            // 初始化起始点
            int id = e.TouchDevice.Id;
            Point start = this.PointToScreen(new Point(e.GetTouchPoint(this).Position.X, e.GetTouchPoint(this).Position.Y));

            if (finger1.Count == 0)
            {
                finger1.Add(id, new List<YTPoint>() { new YTPoint(start.X, start.Y) });
                _watch1 = Stopwatch.StartNew();
            }
            else
            {
                finger2.Add(id, new List<YTPoint>() { new YTPoint(start.X, start.Y) });
                _watch2 = Stopwatch.StartNew();
            }

            // 初始化点击时间
            _watch1 = Stopwatch.StartNew();
            _watch2 = Stopwatch.StartNew();
        }

        private void GestureButton_TouchMove(object sender, TouchEventArgs e)
        {
            int id = e.TouchDevice.Id;
            Point position = PointToScreen(new Point(e.GetTouchPoint(this).Position.X, e.GetTouchPoint(this).Position.Y));
            YTPoint point = new YTPoint(position.X, position.Y);
            if (finger1.ContainsKey(id))
            {
                finger1[id].Add(point);
                var t0 = _watch2.ElapsedMilliseconds; // in seconds
                if (!_bSwipe1 && !_bManipulating1)
                {
                    if (t0 < TIME_THRESHOLD)
                    {
                        if (point.GetDistance(finger1[id][0]) > RADIUS)
                            _bSwipe1 = true;
                    }
                    else
                    {
                        if (point.GetDistance(finger1[id][0]) < RADIUS)
                            _bManipulating1 = true;
                    }
                }
            }
            else if (finger2.ContainsKey(id))
            {
                finger2[id].Add(point);
                var t0 = _watch2.ElapsedMilliseconds; // in seconds

                if(!_bSwipe2 && !_bManipulating2)
                {
                    if (t0 < TIME_THRESHOLD)
                    {
                        if (point.GetDistance(finger2[id][0]) > RADIUS)
                            _bSwipe2 = true;
                    }
                    else
                    {
                        if (point.GetDistance(finger2[id][0]) < RADIUS)
                            _bManipulating2 = true;
                    }
                }
                
                //var xSpeed = (position.X - finger2[id][0].X) / watch.ElapsedMilliseconds;
                //var ySpeed = (position.Y - finger2[id][0].Y) / watch.ElapsedMilliseconds;
                //Console.WriteLine("id {0}, x diff {1}, y diff {2}", id, xSpeed, ySpeed);
            }
        }



        private void GestureButton_TouchUp(object sender, TouchEventArgs e)
        {
            if(_bSwipe1 && _bSwipe2)
                if (finger1.Count != 0 && finger2.Count != 0)
                {
                    var list1 = finger1.ElementAt(0).Value;
                    var list2 = finger2.ElementAt(0).Value;

                    int n1 = list1.Count;
                    int n2 = list2.Count;
                    if (list1[n1 - 1].Y - list1[0].Y > SWIPE_THRESHOLD && list2[n2 - 1].Y - list2[0].Y > SWIPE_THRESHOLD && Math.Abs(list1[n1 - 1].X - list1[0].X) < SWIPE_LIMITED && Math.Abs(list2[n2 - 1].X - list2[0].X) < SWIPE_LIMITED)
                    {
                        Console.WriteLine("双指下滑");
                        //WindowsHandler.MinimizeWindow(_hwnd);
                    }
                    else if (list1[0].Y - list1[n1 - 1].Y > SWIPE_THRESHOLD && list2[0].Y - list2[n2 - 1].Y > SWIPE_THRESHOLD && Math.Abs(list1[n1 - 1].X - list1[0].X) < SWIPE_LIMITED && Math.Abs(list2[n2 - 1].X - list2[0].X) < SWIPE_LIMITED)
                    {
                        Console.WriteLine("双指上滑");
                        //WindowsHandler.MaximizeWindow(_hwnd);
                    }
                    else if (list1[n1 - 1].X - list1[0].X > SWIPE_THRESHOLD && list2[n2 - 1].X - list2[0].X > SWIPE_THRESHOLD && Math.Abs(list1[n1 - 1].Y - list1[0].Y) < SWIPE_LIMITED && Math.Abs(list2[n2 - 1].Y - list2[0].Y) < SWIPE_LIMITED)
                    {
                        Console.WriteLine("双指右滑");
                        //WindowsHandler.RestoreWindow(_hwnd);
                    }
                    else if (list1[0].X - list1[n1 - 1].X > SWIPE_THRESHOLD && list2[0].X - list2[n2 - 1].X > SWIPE_THRESHOLD && Math.Abs(list1[n1 - 1].Y - list1[0].Y) < SWIPE_LIMITED && Math.Abs(list2[n2 - 1].Y - list2[0].Y) < SWIPE_LIMITED)
                    {
                        Console.WriteLine("双指左滑");
                        //WindowsHandler.CloseWindow(_hwnd);
                    }
                }
            finger1 = new Dictionary<int, List<YTPoint>>();
            finger2 = new Dictionary<int, List<YTPoint>>();
            _bSwipe1 = false;
            _bSwipe2 = false;
            _bManipulating1 = false;
            _bManipulating2 = false;
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
            e.Manipulators.ToArray();
        }

        private void GestureButton_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (_bManipulating1 && _bManipulating2)
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
            
        }

        private class YTPoint
        {
            public double X;
            public double Y;

            public YTPoint(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }

            public double GetDistance(YTPoint anotherPoint)
            {
                return Math.Sqrt(Math.Pow(this.Y - anotherPoint.Y, 2) + Math.Pow(this.X - anotherPoint.X, 2));
            }

            public double GetVerticalDistance(YTPoint anotherPoint)
            {
                return Math.Abs(this.Y - anotherPoint.Y);
            }

            public double GetHorozontalDistance(YTPoint anotherPoint)
            {
                return Math.Abs(this.X - anotherPoint.X);
            }

            public bool LeftOf(YTPoint anotherPoint)
            {
                return this.X < anotherPoint.X;
            }

            public bool RightOf(YTPoint anotherPoint)
            {
                return this.X > anotherPoint.X;
            }

            public bool TopOf(YTPoint anotherPoint)
            {
                return this.Y < anotherPoint.Y;
            }

            public bool BottomOf(YTPoint anotherPoint)
            {
                return this.Y > anotherPoint.Y;
            }
        }
    }
}
