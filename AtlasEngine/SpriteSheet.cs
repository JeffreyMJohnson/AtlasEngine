using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

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
