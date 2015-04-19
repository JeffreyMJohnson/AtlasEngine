using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AtlasEngine
{
    class Image2
    {
        string mFilePath;
        BitmapImage mBMP;//the actual image in memory
        Image mControl;//the image control to be placed on a canvas
        ImageHighlight mHighlightRec;
        bool mIsHighlighted = false;
        double mLeft, mTop;

        /// <summary>
        /// Path to image file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Height of image.
        /// </summary>
        public double Height { get; private set; }

        /// <summary>
        /// Width of image.
        /// </summary>
        public double Width { get; private set; }

        /// <summary>
        /// Image control for placing on a canvas.
        /// </summary>
        public Image ImageControl
        {
            get { return mControl; }
        }

        /// <summary>
        /// Left position of image.
        /// i.e. The minimum x value of image.
        /// </summary>
        public double Left
        {
            get { return mLeft; }
            set
            {
                mLeft = value;
                Canvas.SetLeft(mControl, mLeft);
            }
        }

        /// <summary>
        /// Top position of image.
        /// i.e. The minimum y value of image.
        /// </summary>
        public double Top
        {
            get { return mTop; }
            set
            {
                mTop = value;
                Canvas.SetTop(mControl, mTop);
            }
        }

        /// <summary>
        /// Rectangle shape for highlighting this image.
        /// </summary>
        public Rectangle HighlightRec
        {
            get { return mHighlightRec.Rectangle; }
        }

        /// <summary>
        /// MyImage constructor
        /// </summary>
        /// <param name="path">Path to image file.</param>
        public Image2(string path)
        {
            mFilePath = path;
            mBMP = new BitmapImage(new Uri(mFilePath));
            Width = mBMP.Width;
            Height = mBMP.Height;
            InitControl();
            mHighlightRec = new ImageHighlight(this);
            mControl.MouseLeftButtonUp += OnLeftButtonUp;


        }

        private void OnLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            HighlightIt();
        }

        public void HighlightIt()
        {
            Canvas canvas = mControl.Parent as Canvas;
            if (canvas == null)
                return;
            if (!mIsHighlighted)
            {
                //insert highlight rectangle before this control in canvas children
                int i = canvas.Children.IndexOf(this.mControl);
                Canvas.SetLeft(mHighlightRec.Rectangle, Left);
                canvas.Children.Insert(i + 1, mHighlightRec.Rectangle);
                mIsHighlighted = true;
            }
            else
            {
                canvas.Children.Remove(mHighlightRec.Rectangle);
                mIsHighlighted = false;
            }

        }

        void InitControl()
        {
            mControl = new Image();
            mControl.Source = mBMP;
            mControl.Stretch = Stretch.None;
        }

    }
}
