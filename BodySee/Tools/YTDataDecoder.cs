using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BodySee.Windows;

namespace BodySee.Tools
{
    //测试用
    class YTDataDecoder
    {
        private static int count = 0;
        private static List<string> list;

        public static void run()
        {
            list = YTFileReader.GetFileContent(@"C:\Users\SDI-Surface1\Downloads\face_2_0_position.txt");
            int N = list.Count;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50); // 20 FPS
            timer.Tick += new EventHandler(ExampleTick);
            timer.Start();
        }

        private static void ExampleTick(object sender, EventArgs e)
        {
            string posData = list[count++];
            Menu menu = null;
            foreach(Window win in Application.Current.Windows)
            {
                if (win is Menu)
                    menu = win as Menu;
            }
            YTMenuMovingComponent component = menu.GetComponent();
            component.run(posData);
        }
    }
}
