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

namespace BodySee
{
    /// <summary>
    /// GestureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GestureWindow : Window
    {
        Dictionary<int, List<Point>> finger1 = new Dictionary<int, List<Point>>();
        Dictionary<int, List<Point>> finger2 = new Dictionary<int, List<Point>>();
        private const int SWIPETHRESHOLD = 50;

        public GestureWindow()
        {
            InitializeComponent();
        }

        private void GestureButton_TouchDown(object sender, TouchEventArgs e)
        {
            TouchDevice device = e.TouchDevice;
            int id = device.Id;
            TouchPoint pointFromWindow = e.GetTouchPoint(this);
            Point start = this.PointToScreen(new Point(pointFromWindow.Position.X, pointFromWindow.Position.Y));   
            
            var list = new List<Point>();
            list.Add(start);

            if (finger1.Count == 0)
            {
                finger1.Add(id, list);
            } else
            {
                finger2.Add(id, list);
            }

            Console.WriteLine("Pointer ID {0}, position: {1}", device.Id, start);
           
        }

        private void GestureButton_TouchMove(object sender, TouchEventArgs e)
        {
            TouchDevice device = e.TouchDevice;
            int id = device.Id;
            TouchPoint pointFromWindow = e.GetTouchPoint(this);
            Point position = this.PointToScreen(new Point(pointFromWindow.Position.X, pointFromWindow.Position.Y));
            

            if (finger1.ContainsKey(id))
            {
                var list = finger1[id];
                list.Add(position);
                finger1[id] = list;
            } else if (finger2.ContainsKey(id))
            {
                var list = finger2[id];
                list.Add(position);
                finger2[id] = list;
            }

            Console.WriteLine("Pointer ID {0}, position: {1}", device.Id, position);
        }

        private void GestureButton_TouchUp(object sender, TouchEventArgs e)
        {
            TouchDevice device = e.TouchDevice;
            int id = device.Id;

            if (finger1.Count != 0 && finger2.Count != 0)
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
        }
        
        private void GestureButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Q))
            {
                this.Close();
            }
        }
    }
}
