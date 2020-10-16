using BodySee.Tools;
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

namespace BodySee.Windows
{
    /// <summary>
    /// AppList.xaml 的交互逻辑
    /// </summary>
    public partial class AppList : Window
    {
        public AppList(Menu menu)
        {
            InitializeComponent();

            this.Width = 1000;
            this.Height = 1000;

            GenerateData();
        }

        private void GenerateData()
        {
            var apps = WindowsHandler.EnumerateWindow();
            List<AppItem> items = new List<AppItem>();
            foreach (IntPtr hwnd in apps)
            {
                string title = WindowsHandler.GetWindowTitle(hwnd);
                ImageSource source = WindowsHandler.GetAppIcon(hwnd);
                AppItem item = new AppItem() { Title = title, Source = source };
                items.Add(item);
            }

            AppItemList.ItemsSource = items;
        }

        

        
    }

    public class AppItem
    {
        public string Title { get; set; }
        public ImageSource Source { get; set; }
    }
}
