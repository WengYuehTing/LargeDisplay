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
using System.Windows.Threading;

namespace BodySee
{
    /// <summary>
    /// GestureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GestureWindow : Window
    {
        private IntPtr hwnd;
        private Dictionary<int, List<Point>> finger1 = new Dictionary<int, List<Point>>();
        private Dictionary<int, List<Point>> finger2 = new Dictionary<int, List<Point>>();
        private const int SWIPETHRESHOLD = 50;
        private const int LONG_PRESS_DURATION = 500;
        private const int LONG_PRESS_LIMITED_PIXELS = 20;
        private bool bLongPressing = false;
        private bool bManipulating = false;
        private DispatcherTimer longPressTimer;

        public GestureWindow()
        {
            InitializeComponent();
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

            // 初始化起始点
            int id = e.TouchDevice.Id;
            Point start = this.PointToScreen(new Point(e.GetTouchPoint(this).Position.X, e.GetTouchPoint(this).Position.Y));   
            var list = new List<Point>();
            list.Add(start);
            if (finger1.Count == 0)
                finger1.Add(id, list);
            else
                finger2.Add(id, list);

            // 找到目标窗口
            hwnd = WinApiManager.FindTopmostWindow();
        }

        private void GestureButton_TouchMove(object sender, TouchEventArgs e)
        {
            int id = e.TouchDevice.Id;
            Point position = this.PointToScreen(new Point(e.GetTouchPoint(this).Position.X, e.GetTouchPoint(this).Position.Y));
            if (finger1.ContainsKey(id))
            {
                finger1[id].Add(position);
                if (longPressTimer.IsEnabled && Point.Subtract(position, finger1[id][0]).Length > LONG_PRESS_LIMITED_PIXELS)
                    longPressTimer.Stop();
            }
            else if (finger2.ContainsKey(id))
            {
                finger2[id].Add(position);
                if (longPressTimer.IsEnabled && Point.Subtract(position, finger2[id][0]).Length > LONG_PRESS_LIMITED_PIXELS)
                    longPressTimer.Stop();
            }

            if(!longPressTimer.IsEnabled && bLongPressing && finger1.Count != 0 && finger2.Count != 0)
            {
                bManipulating = true;
            }
        }

        private void GestureButton_TouchUp(object sender, TouchEventArgs e)
        {
            if (longPressTimer.IsEnabled)
                longPressTimer.Stop();
            
            if (finger1.Count != 0 && finger2.Count != 0 && !bLongPressing)
            {
                var list1 = finger1.ElementAt(0).Value;
                var list2 = finger2.ElementAt(0).Value;

                int n1 = list1.Count;
                int n2 = list2.Count;
                if(list1[n1-1].Y - list1[0].Y > SWIPETHRESHOLD && list2[n2 - 1].Y - list2[0].Y > SWIPETHRESHOLD)
                {
                    TaskManager.getInstance().Execute("minimize\n");
                    Console.WriteLine("双指下滑");
                }
                else if (list1[0].Y - list1[n1 - 1].Y > SWIPETHRESHOLD && list2[0].Y - list2[n2 - 1].Y > SWIPETHRESHOLD)
                {
                    TaskManager.getInstance().Execute("maximize\n");
                    Console.WriteLine("双指上滑");
                }
                else if (list1[n1 - 1].X - list1[0].X > SWIPETHRESHOLD && list2[n2 - 1].X - list2[0].X > SWIPETHRESHOLD)
                {
                    TaskManager.getInstance().Execute("restore\n");
                    Console.WriteLine("双指右滑");
                }
                else if (list1[0].X - list1[n1 - 1].X > SWIPETHRESHOLD && list2[0].X - list2[n2 - 1].X > SWIPETHRESHOLD)
                {
                    TaskManager.getInstance().Execute("close\n");
                    Console.WriteLine("双指左滑");
                }
            }

            finger1 = new Dictionary<int, List<Point>>();
            finger2 = new Dictionary<int, List<Point>>();
            bLongPressing = false;
            bManipulating = false;
            hwnd = IntPtr.Zero;
        }

        private void GestureButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Q))
            {
                this.Close();
            }
        }

        private void GestureButton_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        private void GestureButton_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (!bManipulating)
                return;

            double translate_x = e.DeltaManipulation.Translation.X * 2;
            double translate_y = e.DeltaManipulation.Translation.Y * 2;
            double scale_x = e.DeltaManipulation.Scale.X;
            double scale_y = e.DeltaManipulation.Scale.Y;
            //WinApiManager.MoveWindow(hwnd, (int)translate_x, (int)translate_y);
            WinApiManager.ScaleWindow(hwnd, scale_x, scale_y);
            e.Handled = true;
        }

        private void GestureButton_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);
            e.Handled = true;
        }
    }
}
