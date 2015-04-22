using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.IO;

namespace AtlasEngine
{
    class SpriteSheet
    {
        List<Image2> mSpritesList = new List<Image2>();
        string mBasePath;
        bool mIsNormalized;
        double mWidth, mHeight;
        Canvas mCanvasControl;
        XmlDocument mAtlasDoc = new XmlDocument();
        XmlElement mRootNode;
        //XmlElement mGroupsNode;

        public double Width
        {
            get { return mWidth; }
            set
            {
                mWidth = value;
                mCanvasControl.Width = value;
            }
        }

        public double Height
        {
            get { return mHeight; }
            set
            {
                mHeight = value;
                mCanvasControl.Height = value;
            }
        }


        public SpriteSheet(Canvas canvas, string basePath, double width, double height, bool normalize)
        {
            mBasePath = basePath;
            mIsNormalized = normalize;
            InitAtlasDoc();
            mCanvasControl = canvas;
            Width = width;
            Height = height;
        }

        public void Save(string filePath)
        {
            double totalWidth = 0;
            double totalHeight = 0;

                //foreach (var img in mSpritesList)
                //{
                //    if (img.Left + img.Width > totalWidth)
                //        totalWidth = img.Left + img.Width;
                //    if (img.Top + img.Height > totalHeight)
                //        totalHeight = img.Top + img.Height;
                //}

            totalWidth = Width;
            totalHeight = Height;

            WriteableBitmap finalImage = BitmapFactory.New((int)totalWidth, (int)totalHeight);

            foreach (Image2 img in mSpritesList)
            {
                System.Windows.Rect imageRect = new System.Windows.Rect(img.Left, img.Top, img.Width, img.Height);
                WriteableBitmap wBmp = new WriteableBitmap(BitmapFactory.ConvertToPbgra32Format(img.mBMP));
                /*Use the BitmapFactory.ConvertToPbgra32Format method to automatically convert any input BitmapSource to the right format accepted by this class.*/
                finalImage.Blit(imageRect, wBmp , new System.Windows.Rect(0, 0, wBmp.PixelWidth, wBmp.PixelHeight));
            }

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(finalImage));
            FileStream fStream = null;
            try
            {
                fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                encoder.Save(fStream);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if(fStream != null)
                fStream.Close();
            }


            /*
            //save current canvas transform

            Transform transform = mCanvasControl.LayoutTransform;
            mCanvasControl.LayoutTransform = null;
            System.Windows.Size size = new System.Windows.Size(mCanvasControl.Width, mCanvasControl.Height);


            mCanvasControl.Measure(size);
            mCanvasControl.Arrange(new System.Windows.Rect(size));

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);

            //create file stream for saving image
            using (FileStream outStream = new FileStream(filePath, FileMode.Create))
            {
                //use .png encoder for data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                //push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                //save data to stream
                encoder.Save(outStream);
            }

            //restore previously saved layout
            mCanvasControl.LayoutTransform = transform;
            */
        }


        public void AddSprite(string relativePath)
        {
            Image2 img = new Image2(mBasePath + relativePath);
            GetNextImagePosition(img);
            mCanvasControl.Children.Add(img.ImageControl);

            //XmlElement spriteNode = mAtlasDoc.CreateElement("sprite");
            //XmlAttribute att = mAtlasDoc.CreateAttribute("id");
            //att.Value = id;
            //spriteNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("x");
            //att.Value = x.ToString();
            //spriteNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("y");
            //att.Value = y.ToString();
            //spriteNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("width");
            //att.Value = width.ToString();
            //spriteNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("height");
            //att.Value = height.ToString();
            //spriteNode.SetAttributeNode(att);


            mSpritesList.Add(img);
        }

        public void Clear()
        {
            
        }

        void GetNextImagePosition(Image2 newImage)
        {
            if (mSpritesList.Count == 0) return;

            Image2 lastImage = mSpritesList[mSpritesList.Count - 1];
            //check if need new row
            if (lastImage.Left + lastImage.Width + newImage.Width > Width)
            {
                newImage.Left = 0;
                newImage.Top = GetHighestYInRow();
            }
            else
            {
                newImage.Left = lastImage.Left + lastImage.Width;
                newImage.Top = lastImage.Top;
            }
        }

        double GetHighestYInRow()
        {
            double result = 0;
            foreach (Image2 img in mSpritesList)
            {
                if (img.Top + img.Height > result) result = img.Top + img.Height;
            }
            return result;
        }

        private void InitAtlasDoc()
        {
            mRootNode = mAtlasDoc.CreateElement("SpriteSheet");
            mAtlasDoc.AppendChild(mRootNode);
            XmlAttribute att = mAtlasDoc.CreateAttribute("filePath");
            att.Value = mBasePath;
            mRootNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("width");
            att.Value = Width.ToString();
            mRootNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("height");
            att.Value = Height.ToString();
            mRootNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("page");
            att.Value = "1";
            mRootNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("totalPages");
            //att.Value = mPageCount.ToString();
            //mRootNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("isNormalized");
            att.Value = mIsNormalized.ToString();
            mRootNode.SetAttributeNode(att);

            // mGroupsNode = mAtlasDoc.CreateElement("groups");
            //mRootNode.AppendChild(mGroupsNode);

            XmlElement groupNode = mAtlasDoc.CreateElement("group"); ;
            // groupNode.SetAttribute("name", "group0");
            mRootNode.AppendChild(groupNode);
        }
    }
}
