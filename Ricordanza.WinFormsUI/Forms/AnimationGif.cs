using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ricordanza.WinFormsUI.Forms
{
    #region AnimationGif

    /// <summary>
    /// アニメーション<c>Gif</c>の描画クラスです。
    /// </summary>
    public class AnimationGif
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 
        /// </summary>
        private bool _cancelPendding;
        
        /// <summary>
        /// 
        /// </summary>
        private int _currentFrame;
        
        /// <summary>
        /// 
        /// </summary>
        private AnimationGifFrame[] _frames;
        
        /// <summary>
        /// 
        /// </summary>
        private int _nextFrame;
        
        /// <summary>
        /// 
        /// </summary>
        private double _speed;
        
        /// <summary>
        /// 
        /// </summary>
        private Thread _t;

        #endregion

        #region static constructor

        #endregion

        #region constructor

         public AnimationGif(Image image)
        {
            this._speed = 1.0;
            this.LoadFrames(image);
        }

        public AnimationGif(string fileName)
        {
            this._speed = 1.0;
            using (Image image = Image.FromFile(fileName))
                this.LoadFrames(image);
        }

        #endregion

        #region property

        public AnimationGifFrame Current
        {
            get
            {
                return this._frames[this._currentFrame];
            }
        }

        public AnimationGifFrame[] Frames
        {
            get
            {
                return this._frames;
            }
        }

        #endregion

        #region event

        #endregion

        #region event handler

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler FrameChanged;

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (this._t != null)
            {
                this._cancelPendding = true;
                this._t.Abort();
                this._t = null;
            }
        }

        #endregion

        #region protected method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnFrameChanged(EventArgs e)
        {
            if (this.FrameChanged != null)
                this.FrameChanged(this, e);
        }

        #endregion

        #region private method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        private void LoadFrames(Image image)
        {
            int framesCount = image.GetFrameCount(FrameDimension.Time);
            if (framesCount <= 1)
            {
                throw new ArgumentException("Image not animated");
            }
            byte[] times = image.GetPropertyItem(0x5100).Value;
            int frame = 0;
            List<AnimationGifFrame> frames = new List<AnimationGifFrame>();
            while (true)
            {
                int dur = BitConverter.ToInt32(times, 4 * frame) * 10;
                frames.Add(new AnimationGifFrame(new Bitmap(image), dur));
                if (++frame >= framesCount)
                    break;

                image.SelectActiveFrame(FrameDimension.Time, frame);
            }
            this._frames = frames.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThreadProc()
        {
            int duration = 0;
            try
            {
                Label_0002:
                if (!this._cancelPendding)
                {
                    this._currentFrame = this._nextFrame;
                    AnimationGifFrame frame = this._frames[this._currentFrame];
                    duration = (int)(frame.Duration * this._speed);
                    this.OnFrameChanged(EventArgs.Empty);
                    this._nextFrame++;
                    if (this._nextFrame >= this._frames.Length)
                    {
                        this._nextFrame = 0;
                    }
                    if (!this._cancelPendding)
                    {
                        Thread.Sleep(duration);
                        goto Label_0002;
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region AnimationGifFrame

    /// <summary>
    /// 
    /// </summary>
    public class AnimationGifFrame
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 
        /// </summary>
        private int _duration;

        /// <summary>
        /// 
        /// </summary>
        private Image _image;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        #endregion

        #region property

        /// <summary>
        /// 
        /// </summary>
        public int Duration
        {
            get
            {
                return this._duration;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Image Image
        {
            get
            {
                return this._image;
            }
        }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="duration"></param>
        internal AnimationGifFrame(Image image, int duration)
        {
            this._image = image;
            this._duration = duration;
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion
}