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
using System.Windows.Interop;
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

        private List<string> appTitles;



        public AppList(Menu menu)
        {
            InitializeComponent();

            this.Width = menu.Width;
            this.Height = 120;
            this.Top = menu.Top + menu.Height + 10;
            this.Left = menu.Left;
            appTitles = new List<string>();
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
                appTitles.Add(title);
            }

            EventManager.RegisterClassHandler(typeof(ListBoxItem), ListBoxItem.TouchDownEvent, new RoutedEventHandler(this.OnTouchDownEvent));
            AppItemList.ItemsSource = items;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized || WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;
        }

        private void OnTouchDownEvent(object sender, RoutedEventArgs e)
        {
            int index = AppItemList.ItemContainerGenerator.IndexFromContainer(sender as ListBoxItem);
            IntPtr hwnd = WindowsHandler.GetHandleFromTitle(appTitles[index]);
            var processes = Process.GetProcesses();
            foreach(Process process in processes)
            {
                if(process.MainWindowTitle == appTitles[index])
                {
                    WinApiManager.BringWindowToTop(process.MainWindowHandle);
                    this.Close();
                }
            }

        }
    }

    public class AppItem
    {
        public string Title { get; set; }
        public ImageSource Source { get; set; }
    }
}
