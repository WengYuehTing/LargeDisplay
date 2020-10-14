using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BodySee.Tools
{
    public struct RAMP
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public UInt16[] Red;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public UInt16[] Green;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public UInt16[] Blue;
    }

    class BrightnessAdjuster
    {
        #region Singleton
        private static BrightnessAdjuster instance;
        public static BrightnessAdjuster getInstance()
        {
            if (instance == null)
            {
                instance = new BrightnessAdjuster();
            }
            return instance;
        }
        private BrightnessAdjuster() { }
        #endregion

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        #region Private Constants
        /// <summary>
        /// 设备所能设置音量的最大值
        /// </summary>        
        private const double BRIGHTNESS_MAX = 256;

        /// <summary>
        /// 设备所能设置音量的最小值
        /// </summary>
        private const double BRIGHTNESS_MIN = 1;
        #endregion

        #region Public Methods
        public void SetBrightness(int _brightness)
        {
            if (_brightness <= BRIGHTNESS_MAX && _brightness >= BRIGHTNESS_MIN)
            {
                RAMP ramp = new RAMP();
                ramp.Red = new ushort[256];
                ramp.Green = new ushort[256];
                ramp.Blue = new ushort[256];
                for (int i = 1; i < 256; i++)
                {
                    int iArrayValue = i * (_brightness + 128);
                    if (iArrayValue > 65535)
                        iArrayValue = 65535;
                    ramp.Red[i] = ramp.Blue[i] = ramp.Green[i] = (ushort)iArrayValue;
                }
                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
            }
        }
        #endregion
    }
}
