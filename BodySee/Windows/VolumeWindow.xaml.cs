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
    /// VolumeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VolumeWindow : Window
    {
        public VolumeWindow(Menu menu)
        {
            InitializeComponent();

            VolumeSlider.Maximum = VolumeAdjuster.VOLUME_MAX;
            VolumeSlider.Minimum = VolumeAdjuster.VOLUME_MIN;
            VolumeSlider.Value = VolumeAdjuster.getInstance().Volume;
            VolumeSlider.ValueChanged += VolumeSlider_ValueChanged;
            this.Width = menu.Width;
            this.Height = 100;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeAdjuster.getInstance().SetVolume(e.NewValue);
        }
    }
}
