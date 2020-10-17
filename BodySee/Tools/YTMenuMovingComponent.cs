using BodySee.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodySee.Tools
{
    public enum MovingMode
    {
        X,
        Y,
        XY
    }

    public class YTMenuMovingComponent
    {
        
        private Menu _Menu;
        private MovingMode _Mode;
        private int ScreenWidth;
        private int ScreenHeight;
        private Queue<int> xQueue;
        private Queue<int> yQueue;

        private const int SMOOTH_CAPACILITY = 5;
        private const int SHAKING_INGORE_THRESHOLD = 5;
        private const int OUTLIER_THRESHOLD = 200;
        private const int GLOBLE_WIDTH = 1600;
        private const int GLOBLE_HEIGHT = 1200;

        public YTMenuMovingComponent(Menu menu, MovingMode mode)
        {
            _Menu = menu;
            _Mode = mode;
            ScreenWidth = (int)WindowsHandler.GetScreenWidth();
            ScreenHeight = (int)WindowsHandler.GetScreenHeight();
            xQueue = new Queue<int>();
            yQueue = new Queue<int>();
        }

        public void run(string source)
        {
            //Step1 解码
            int[] posData = Decode(source); 
            if (posData[3] == 0) // error code
                throw new Exception("Fail to decode body position");

            //Step2 转成屏幕坐标系
            posData = Translate(posData[1], posData[2]);

            //Step3 均值优化
            int N = (xQueue.Count + yQueue.Count) / 2;
            int xSum = 0;
            int ySum = 0;
            for(int i=0;i<N; i++)
            {
                xSum += xQueue.ElementAt(i);
                ySum += yQueue.ElementAt(i);
            }

            int x = N == 0 ? posData[0] : (int)(double)xSum / N;
            int y = N == 0 ? posData[1] : (int)(double)ySum / N;

            xQueue.Enqueue(posData[0]);
            yQueue.Enqueue(posData[1]);
            if (xQueue.Count > SMOOTH_CAPACILITY)
                xQueue.Dequeue();
            if (yQueue.Count > SMOOTH_CAPACILITY)
                yQueue.Dequeue();

            //Step4 移动
            if (_Mode == MovingMode.X)
            {
                _Menu.Left = x;
            }
            else if(_Mode == MovingMode.Y)
            {
                _Menu.Top = y;
            }
            else 
            {
                _Menu.Left = x;
                _Menu.Top = y;
            }
         

            

        }

        

        private int[] Decode(string source)
        {
            int n = 0, x = 0, y = 0, e = 0;
            try
            {
                var data = source.Split(' ');
                n = int.Parse(data[0]); 
                x = (int)double.Parse(data[1]);
                y = (int)double.Parse(data[2]);
                e = 1;

            } catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            
            return new int[] { n, x, y, e };
        }

        private int[] Translate(int x, int y)
        {
            x = (int)(((double)x / GLOBLE_WIDTH) * ScreenWidth);
            y = (int)(((double)x / GLOBLE_HEIGHT) * ScreenHeight);
            return new int[] { x, y };
        }
    }
}
