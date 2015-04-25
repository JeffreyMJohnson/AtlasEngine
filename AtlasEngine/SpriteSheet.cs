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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace AtlasEngine
{
    public class SpriteSheet : INotifyPropertyChanged
    {
        //MAGIC NUMBERS
        const int DEFAULT_IMAGE_WIDTH = 256;
        const int DEFAULT_IMAGE_HEIGHT = 256;

        List<Image2> mSpritesList = new List<Image2>();
        string mBasePath = "";
        bool mIsNormalized = false;
        double mWidth = 0;
        double mHeight = 0;
        Canvas mCanvasControl = null;
        XmlDocument mAtlasDoc = new XmlDocument();
        XmlElement mRootNode = null;
        bool mHasChanged = false;//flag for checking if need to remind to save.
        //XmlElement mGroupsNode;
        MainWindow mWindow;

        public XmlDocument AtlasDoc
        {
            get { return mAtlasDoc; }
        }

        public bool HasChanged
        {
            get { return mHasChanged; }
            set
            {
                mHasChanged = value;
                OnPropertyChanged("HasChanged");
                if (mWindow != null)
                {
                    if (mHasChanged)
                    {
                        mWindow.canvasScrollviewer.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 00));
                    }
                    else
                    {
                        mWindow.canvasScrollviewer.Background = new SolidColorBrush(Color.FromArgb(25, 00, 255, 00));
                    }
                }
            }
        }

        public double Width
        {
            get { return mWidth; }
            set
            {
                mWidth = value;
                mCanvasControl.Width = value;
                OnPropertyChanged("Width");
            }
        }

        public double Height
        {
            get { return mHeight; }
            set
            {
                mHeight = value;
                mCanvasControl.Height = value;
                OnPropertyChanged("Height");
            }
        }


        public SpriteSheet(MainWindow window, Canvas canvas, string basePath, double width, double height, bool normalize)
        {
            mBasePath = basePath;
            mIsNormalized = normalize;
            mCanvasControl = canvas;
            Width = width;
            Height = height;
            mWindow = window;
            InitAtlasDoc();
        }

        public void Save(string filePath)
        {
            //build image file name
            string file = "";
            string path = "";
            ParseFilePath(filePath, out  path, out file);

            SaveImageFile(path + XmlToPngFile(file));
            SetAtlasFileAttribute(XmlToPngFile(file));
            SaveAtlasFile(path, file);
            HasChanged = false;

        }

        void SetAtlasFileAttribute(string file)
        {
            //set filepath attribute on atlas file
            XmlAttribute att = mAtlasDoc.CreateAttribute("filePath");
            att.Value = file;
            mRootNode.SetAttributeNode(att);
        }


        string XmlToPngFile(string file)
        {
            int dotIndex = file.LastIndexOf('.');
            //substring file without extension and concatenate .png to it.
            return file.Substring(0, dotIndex) + ".png";
        }

        void ParseFilePath(string filePath, out string path, out string file)
        {
            int whackIndex = filePath.LastIndexOf('\\');
            path = filePath.Substring(0, whackIndex + 1);
            file = filePath.Substring(whackIndex + 1, filePath.Length - (whackIndex + 1));
        }


        void SaveAtlasFile(string path, string file)
        {
            //save atlas file
            mAtlasDoc.Save(path + file);
        }

        void SaveImageFile(string filePath)
        {

            //create bitmap to hold sprites
            WriteableBitmap finalImage = BitmapFactory.New((int)Width, (int)Height);

            //add the sprites to the bitmap
            foreach (Image2 img in mSpritesList)
            {
                System.Windows.Rect imageRect = new System.Windows.Rect(img.Left, img.Top, img.Width, img.Height);
                WriteableBitmap wBmp = new WriteableBitmap(BitmapFactory.ConvertToPbgra32Format(img.mBMP));
                finalImage.Blit(imageRect, wBmp, new System.Windows.Rect(0, 0, wBmp.PixelWidth, wBmp.PixelHeight));
            }

            //saving in png format
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(finalImage));

            //save the file
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
                if (fStream != null)
                    fStream.Close();
            }
        }


        public void AddSprite(string path)
        {
            Image2 img = new Image2(path, mSpritesList.Count);
            GetNextImagePosition(img);
            mCanvasControl.Children.Add(img.ImageControl);
            HasChanged = true;

            mSpritesList.Add(img);

            //build xml
            XmlElement spriteNode = mAtlasDoc.CreateElement("sprite");
            XmlAttribute att = mAtlasDoc.CreateAttribute("id");
            att.Value = img.ID.ToString();
            spriteNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("x");
            att.Value = ((int)img.Left).ToString();
            spriteNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("y");
            att.Value = ((int)img.Top).ToString();
            spriteNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("width");
            att.Value = ((int)img.Width).ToString();
            spriteNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("height");
            att.Value = ((int)img.Height).ToString();
            spriteNode.SetAttributeNode(att);

            //append to group node
            mRootNode.FirstChild.AppendChild(spriteNode);


        }

        public void Clear()
        {

            mCanvasControl.Children.RemoveRange(0, mCanvasControl.Children.Count);
            Width = DEFAULT_IMAGE_WIDTH;
            Height = DEFAULT_IMAGE_HEIGHT;
            mSpritesList.Clear();
            mAtlasDoc.RemoveAll();
            HasChanged = false;
            InitAtlasDoc();

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


            XmlAttribute att = mAtlasDoc.CreateAttribute("width");
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

            mAtlasDoc.AppendChild(mRootNode);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
