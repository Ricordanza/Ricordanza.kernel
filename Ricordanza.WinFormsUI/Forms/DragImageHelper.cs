using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class DragImageHelper
    {
        #region const

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himlTrack"></param>
        /// <param name="iTrack"></param>
        /// <param name="dxHotspot"></param>
        /// <param name="dyHotspot"></param>
        /// <returns></returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_BeginDrag(IntPtr himlTrack, int iTrack, int dxHotspot, int dyHotspot);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndLock"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragMove(int x, int y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndLock"></param>
        /// <returns></returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragLeave(IntPtr hwndLock);

        /// <summary>
        /// 
        /// </summary>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern void ImageList_EndDrag();
 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.Dll")]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fShow"></param>
        /// <returns></returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragShowNolock(bool fShow);

        #endregion

        #region private variable

        /// <summary>
        /// 
        /// </summary>
        private static bool _drag = false;
        
        /// <summary>
        /// 
        /// </summary>
        private static ImageList _imageList = null;
        
        /// <summary>
        /// 
        /// </summary>
        private static IntPtr _lockHandle = IntPtr.Zero;
        
        /// <summary>
        /// 
        /// </summary>
        private static bool _show = false;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        #endregion

        #region property

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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dxHotspot"></param>
        /// <param name="dyHotspot"></param>
        public static void BeginDrag(Image image, int x, int y, int dxHotspot, int dyHotspot)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            if (_drag)
                EndDrag();
 
            if (_imageList == null)
            {
                _imageList = new ImageList();
                _imageList.ColorDepth = ColorDepth.Depth32Bit;
            }

            _imageList.Images.Clear();
            int width = (image.Width > 0x100) ? 0x100 : image.Width;
            int height = (image.Height > 0x100) ? 0x100 : image.Height;
            _imageList.ImageSize = new Size(width, height);
            _imageList.Images.Add(image);
            if (ImageList_BeginDrag(_imageList.Handle, 0, dxHotspot, dyHotspot))
            {
                _drag = true;
                _show = true;
                _lockHandle = GetDesktopWindow();
                ImageList_DragEnter(_lockHandle, x, y);
                Application.DoEvents();
                DragMove(x, y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DragMove(int x, int y)
        {
            if (_drag && _show)
                ImageList_DragMove(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="show"></param>
        public static void DragShowNolock(bool show)
        {
            if (_drag)
            {
                _show = show;
                if (_show)
                    Application.DoEvents();

                ImageList_DragShowNolock(show);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void EndDrag()
        {
            if (_drag)
            {
                ImageList_DragLeave(_lockHandle);
                ImageList_EndDrag();
                if (_imageList != null)
                    _imageList.Images.Clear();

                _drag = false;
            }
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
