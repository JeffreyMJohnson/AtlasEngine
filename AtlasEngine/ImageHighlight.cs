using System.Windows.Media;
using System.Windows.Shapes;

namespace AtlasEngine
{
    class ImageHighlight
    {
        Rectangle mRec = new Rectangle();
        DoubleCollection mStrokeDashArray = new DoubleCollection() { 5, 5 };
        const double mStrokeThickness = 3;
        Color mStrokeColor = Colors.Blue;
        Image2 mImage;

        public Rectangle Rectangle
        {
            get { return mRec; }
        }

        public ImageHighlight(Image2 image)
        {
            mImage = image;
            mRec.Width = mImage.Width;
            mRec.Height = mImage.Height;
            mRec.Stroke = new SolidColorBrush(mStrokeColor);
            mRec.StrokeThickness = mStrokeThickness;
            mRec.StrokeDashArray = mStrokeDashArray;
        }

    }
}
