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
        private Menu _Menu;
        public Menu Menu
        {
            set { _Menu = value; }
            get { return _Menu; }
        }



        public AppList(Menu menu)
        {
            InitializeComponent();
            GenerateData();

            this.Width = appTitles.Count * (100 + 20) - 40;
            this.AppItemList.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            this.Height = menu.Height;
            this.Top = menu.Top + menu.Height + 10;
            this.Left = menu.Left;
            this.Menu = menu;
        }

        private void GenerateData()
        { 
            var apps = WindowsHandler.EnumerateWindow();
            appTitles = new List<string>();
            List<AppItem> items = new List<AppItem>();
            foreach (IntPtr hwnd in apps)
            {
                string title = WindowsHandler.GetWindowTitle(hwnd);
                ImageSource source = WindowsHandler.GetAppIcon(hwnd);
                AppItem item = new AppItem() { Title = title, Source = source };
                items.Add(item);
                appTitles.Add(title);
            }
            
            //EventManager.RegisterClassHandler(typeof(ListBoxItem), ListBoxItem.TouchLeaveEvent, new RoutedEventHandler(OnTouchUpEvent));
            AppItemList.ItemsSource = items;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized || WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;
        }

        private void OnTouchUpEvent(object sender, RoutedEventArgs e)
        {
            int index = AppItemList.ItemContainerGenerator.IndexFromContainer(sender as ListBoxItem);
            if (index >= appTitles.Count)
                return;

            Console.WriteLine(index);
            IntPtr hwnd = WindowsHandler.GetHandleFromTitle(appTitles[index]);
            var processes = Process.GetProcesses();
            foreach(Process process in processes)
            {
                if(process.MainWindowTitle == appTitles[index])
                {
                    WinApiManager.BringWindowToTop(process.MainWindowHandle);
                    
                }
            }

            Menu.CloseAppListWindow();
        }

        private void AppItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IntPtr hwnd = WindowsHandler.GetHandleFromTitle(appTitles[AppItemList.SelectedIndex]);
            foreach (Process process in Process.GetProcesses())
            {
                if (process.MainWindowTitle == appTitles[AppItemList.SelectedIndex])
                    WinApiManager.SwitchToThisWindow(process.MainWindowHandle, true);
            }
            Menu.CloseAppListWindow();
        }
    }

    public class AppItem
    {
        public string Title { get; set; }
        public ImageSource Source { get; set; }
    }
}
