using BodySee.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows;

namespace BodySee.Tools
{

    public enum MovingState
    {
        Static,
        Dynamic
    }

    public enum MovingMode
    {
        X,
        Y,
        XY
    }

    public class YTMenuMovingComponent
    {
        
        private Menu _Menu;
        private MovingMode _MovingMode;
        private MovingState _MovingState;
        private int _ScreenWidth;
        private int _ScreenHeight;
        private Queue<double> _xQueue;
        private Queue<double> _yQueue;

        private const int SMOOTH_CAPACILITY = 5;
        private const int SHAKING_INGORE_THRESHOLD = 10;
        private const int OUTLIER_INGORE_THRESHOLD = 100;
        private const int GLOBLE_WIDTH = 1600;
        private const int GLOBLE_HEIGHT = 1200;
        private const int ANIMATION_DURATION = 500; 

        public YTMenuMovingComponent(Menu menu, MovingMode mode)
        {
            _Menu = menu;
            _MovingMode = mode;
            _MovingState = MovingState.Static;
            _ScreenWidth = (int)WindowsHandler.GetScreenWidth();
            _ScreenHeight = (int)WindowsHandler.GetScreenHeight();
            _xQueue = new Queue<double>();
            _yQueue = new Queue<double>();
        }

        public void run(string source)
        {
            //Step1 解码
            double[] posData = Decode(source); 
            if (posData[3] == 0.0f) // error code
                throw new Exception("Fail to decode body position");

            //Step2 转成屏幕坐标系
            posData = Translate(posData[1], posData[2]);

            //Step3 均值优化
            int N = (_xQueue.Count + _yQueue.Count) / 2;
            double xSum = 0;
            double ySum = 0;
            for(int i=0;i<N; i++)
            {
                xSum += _xQueue.ElementAt(i);
                ySum += _yQueue.ElementAt(i);
            }

            double x = N == 0 ? posData[0] : xSum / N;
            double y = N == 0 ? posData[1] : ySum / N;
            double gx = Math.Abs(_Menu.Left - x);
            double gy = Math.Abs(_Menu.Top - y);

            _xQueue.Enqueue(posData[0]);
            _yQueue.Enqueue(posData[1]);
            if (_xQueue.Count > SMOOTH_CAPACILITY)
                _xQueue.Dequeue();
            if (_yQueue.Count > SMOOTH_CAPACILITY)
                _yQueue.Dequeue();

            if (gx < SHAKING_INGORE_THRESHOLD)
                return;




            //Step4 创建动画
            Storyboard story = new Storyboard();
            DoubleAnimation animX = new DoubleAnimation(_Menu.Left, x, TimeSpan.FromMilliseconds(ANIMATION_DURATION));
            DoubleAnimation animY = new DoubleAnimation(_Menu.Top, y, TimeSpan.FromMilliseconds(ANIMATION_DURATION));


            //Step5 移动
            if (_MovingMode == MovingMode.X)
            {
                story.Children.Add(animX);
                Storyboard.SetTargetName(animX, _Menu.Name);
                Storyboard.SetTargetProperty(animX, new PropertyPath(Window.LeftProperty));
                story.Begin(_Menu);
            }
            else if(_MovingMode == MovingMode.Y)
            {
                story.Children.Add(animY);
                Storyboard.SetTargetName(animY, _Menu.Name);
                Storyboard.SetTargetProperty(animY, new PropertyPath(Window.TopProperty));
                story.Begin(_Menu);
            }
            else 
            {
                story.Children.Add(animX);
                story.Children.Add(animY);
                Storyboard.SetTargetName(animX, _Menu.Name);
                Storyboard.SetTargetName(animY, _Menu.Name);
                Storyboard.SetTargetProperty(animX, new PropertyPath(Window.LeftProperty));
                Storyboard.SetTargetProperty(animY, new PropertyPath(Window.TopProperty));
                story.Begin(_Menu);
            }

            
        }

        

        private double[] Decode(string source)
        {
            double n = 0.0f, x = 0.0f, y = 0.0f, e = 0.0f;
            try
            {
                var data = source.Split(' ');
                n = double.Parse(data[0]); 
                x = double.Parse(data[1]);
                y = double.Parse(data[2]);
                e = 1.0f;

            } catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            
            return new double[] { n, x, y, e };
        }

        private double[] Translate(double x, double y)
        {
            x = _ScreenWidth - (x / GLOBLE_WIDTH) * _ScreenWidth; // reverse
            y = (x / GLOBLE_HEIGHT) * _ScreenHeight;
            return new double[] { x, y };
        }
    }
}
