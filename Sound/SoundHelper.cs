//using System;
//using System.Diagnostics;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using System.Windows.Threading;
//using System.ComponentModel;

//namespace Wp7Shared.Sound
//{
//    public class SoundHelper : INotifyPropertyChanged
//    {
//        private DispatcherTimer _tmr = null;
//        private SoundEffectInstance _currentSoundEffect;
//        private DateTime _currentSoundEffectStartTime;
//        private TimeSpan _currentSoundEffectDuration;

//        private bool _isPlaying;
//        public bool IsPlaying
//        {
//            get { return _isPlaying; }
//            set
//            {
//                _isPlaying = value;
//                NotifyPropertyChanged("IsPlaying");
//            }
//        }

//        public SoundHelper()
//        {
//            _tmr = new DispatcherTimer();
//            _tmr.Interval = TimeSpan.FromMilliseconds(250);
//            _tmr.Tick += new EventHandler(_tmr_Tick);
//        }

//        void _tmr_Tick(object sender, EventArgs e)
//        {
//            if (_currentSoundEffect == null)
//            {
//                return;
//            }

//            TimeSpan span = DateTime.Now.Subtract(_currentSoundEffectStartTime);
//            if (span < _currentSoundEffectDuration)
//            {
//                return;
//            }

//            SoundState state = _currentSoundEffect.State;
//            switch (state)
//            {
//                case SoundState.Paused:
//                    break;
//                //case SoundState.Playing:
//                //    IsPlaying = true;
//                //    break;
//                case SoundState.Stopped:
//                    Cleanup();
//                    break;
//            }
//        }

//        private void Cleanup()
//        {
//            if (_tmr != null && _tmr.IsEnabled)
//            {
//                _tmr.Stop();
//            }

//            IsPlaying = false;
//        }

//        public void Play(SoundEffect sound)
//        {
//            try
//            {
//                if (!_tmr.IsEnabled)
//                {
//                    _tmr.Start();
//                }
//                FrameworkDispatcher.Update();
//                if (_currentSoundEffect != null)
//                {
//                    //stoppa il sample corrente
//                    _currentSoundEffect.Stop(true);
//                }

//                _currentSoundEffect = sound.CreateInstance();
//                _currentSoundEffectDuration = sound.Duration;

//                _currentSoundEffect.Play();
//                IsPlaying = true;
//                _currentSoundEffectStartTime = DateTime.Now;
//            }
//            catch (Exception)
//            {
//            }
//        }


//        public event PropertyChangedEventHandler PropertyChanged;
//        protected void NotifyPropertyChanged(String propertyName)emu
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (null != handler)
//            {
//                handler(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }
//}
