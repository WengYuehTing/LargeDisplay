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
    /// BrightnessWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BrightnessWindow : Window
    {
        public BrightnessWindow(Menu menu)
        {
            InitializeComponent();

            BrightnessSlider.Maximum = BrightnessAdjuster.BRIGHTNESS_MAX;
            BrightnessSlider.Minimum = BrightnessAdjuster.BRIGHTNESS_MIN;
            BrightnessSlider.Value = BrightnessAdjuster.getInstance().Brightness;
            BrightnessSlider.ValueChanged += BrightnessSlider_ValueChanged;
            this.Width = menu.Width;
            this.Height = 60;
            this.Top = menu.Top + menu.Height;
            this.Left = menu.Left;
        }

        private void BrightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BrightnessAdjuster.getInstance().SetBrightness(e.NewValue);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized || WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;
        }
    }
}
