using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodySee.Tools
{
    interface IVolumeInterface
    {
        void OnVolumeChange(double _vol);
        void OnMutedChange(bool _mute);
    }

    class VolumeAdjuster
    {
        #region Singleton
        private static VolumeAdjuster instance;
        public static VolumeAdjuster getInstance()
        {
            if (instance == null)
            {
                instance = new VolumeAdjuster();
            }
            return instance;
        }
        private VolumeAdjuster()
        {
        }
        #endregion

        #region Private Constants
        private CoreAudioDevice Device = new CoreAudioController().DefaultPlaybackDevice;

        /// <summary>
        /// 设备所能设置音量的最大值
        /// </summary>        
        public const double VOLUME_MAX = 100;

        /// <summary>
        /// 设备所能设置音量的最小值
        /// </summary>
        public const double VOLUME_MIN = 0;
        #endregion

        #region Public Properties
        /// <summary>
        /// 当前系统音量
        /// </summary>
        public double Volume
        {
            get
            {
                return Device.Volume;
            }
        }


        /// <summary>
        /// 当前系统是否静音
        /// </summary>
        public bool IsMuted
        {
            get
            {
                return Device.IsMuted;
            }
        }

        public IVolumeInterface Delegate { get; set; }
        #endregion

        #region Public Methods
        public void SetVolume(double _vol)
        {
            Device.Volume = Math.Min(VOLUME_MAX, Math.Max(VOLUME_MIN, _vol)); // limit it in 0 ~ 100
            if(Delegate != null)
                Delegate.OnVolumeChange(_vol);
        }

        public void SetMute(bool _mute)
        {
            Device.Mute(_mute);
            if (Delegate != null)
                Delegate.OnMutedChange(_mute);
        }
        #endregion
    }
}
