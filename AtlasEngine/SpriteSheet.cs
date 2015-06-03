using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace AtlasEngine
{
    /// <summary>
    /// Class representing sprite sheet.
    /// </summary>
    public class SpriteSheet : INotifyPropertyChanged
    {
        //MAGIC NUMBERS
        int DEFAULT_IMAGE_WIDTH = 256;
        int DEFAULT_IMAGE_HEIGHT = 256;

        public event PropertyChangedEventHandler PropertyChanged;
        List<Image2> mSpritesList = new List<Image2>();
        bool mIsNormalized = false;
        bool mAutoResize = false;
        double mWidth = 0;
        double mHeight = 0;
        AtlasDocument mAtlasDoc = new AtlasDocument();
        bool mHasChanged = false;//flag for checking if need to remind to save.
        MainWindow mWindow = null;

        /// <summary>
        /// Returns and sets the path and file to the sprite sheet.
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// Sets the MainWindow property of the sprite sheet.
        /// </summary>
        public MainWindow Window
        {
            set { mWindow = value; }
        }

        /// <summary>
        /// Sets and returns the HasChanged property.
        /// </summary>
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
                        mWindow.canvasScrollviewer.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 00));
                    }
                    else
                    {
                        mWindow.canvasScrollviewer.Background = new SolidColorBrush(Color.FromArgb(100, 00, 255, 00));
                    }
                }
            }
        }

        /// <summary>
        /// Sets and returns the width of the sprite sheet.
        /// </summary>
        public double Width
        {
            get { return mWidth; }
            set
            {
                mWidth = (double)((int)value);
                mAtlasDoc.SheetWidth = mWidth.ToString();
                if (mWindow != null)
                {
                    mWindow.canvasControl.Width = value;
                }
                OnPropertyChanged("Width");
            }
        }


        /// <summary>
        /// Set the height of the sprite sheet.
        /// </summary>
        public double Height
        {
            get { return mHeight; }
            set
            {
                mHeight = (double)((int)value);
                mAtlasDoc.SheetHeight = mHeight.ToString();
                if (mWindow != null)
                {
                    mWindow.canvasControl.Height = value;
                }
                OnPropertyChanged("Height");
            }
        }

        /// <summary>
        /// Sets and returns wether or not the sprite sheet sets it's width and height automatically.
        /// </summary>
        public bool AutoResize
        {
            get { return mAutoResize; }
            set
            {
                mAutoResize = value;
                SwitchSettingsPanelUI();
                OnPropertyChanged("AutoResize");
            }
        }

        /// <summary>
        /// Returns the xMLDocument of this sprite sheet.\n
        /// *NOTE: this property is read-only.*
        /// </summary>
        public XmlDocument AtlasDocument
        {
            get { return mAtlasDoc.XMLDoc; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SpriteSheet()
        {
            Width = DEFAULT_IMAGE_WIDTH;
            Height = DEFAULT_IMAGE_HEIGHT;
        }

        /// <summary>
        /// Overloaded constructor.\n
        /// *note:used for testing. Should use SpriteSheet(width, height) for production.
        /// </summary>
        /// <param name="width">Width of the sprite sheet.</param>
        /// <param name="height">Height of the sprite sheet.</param
        public SpriteSheet(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Overloaded spritesheet\n
        /// *note:This is the constructor to use for production.*
        /// </summary>
        /// <param name="window">Handle to the MainWindow of the application.</param>
        /// <param name="basePath">Base path of this project.</param>
        /// <param name="width">Width of the sprite sheet.</param>
        /// <param name="height"><Height of the sprite sheet./param>
        /// <param name="normalize">*NOT USED, RESERVED FOR FUTURE USE.*</param>
        public SpriteSheet(MainWindow window, string basePath, double width, double height, bool normalize)
        {
            mWindow = window;
            BasePath = basePath;
            mIsNormalized = normalize;
            Width = width;
            Height = height;
            AutoResize = false;
        }

        /// <summary>
        /// Save the sprite sheet and the atlas map file(XML).
        /// </summary>
        /// <param name="filePath">Path and file name of the atlas map file (XML).</param>
        public void Save(string filePath)
        {
            //build image file name
            string xmlFile = "";
            string path = "";
            ParseFilePath(filePath, out  path, out xmlFile);
            string imageFile = XmlToPngFile(xmlFile);

            SaveImageFile(path + imageFile);
            mAtlasDoc.SheetPath = imageFile;
            mAtlasDoc.Save(path + xmlFile);
            HasChanged = false;

        }

        /// <summary>
        /// Add a sprite image to the sprite sheet.
        /// </summary>
        /// <param name="path">File path and name of image to load.</param>
        public void AddSprite(string path)
        {
            Image2 img = new Image2(path, mSpritesList.Count);
            //getnextimage will return a bool representing if there is room for new image.
            if (!GetNextImagePosition(img))
            {
                if (PopNotEnoughRoom() == MessageBoxResult.Yes)
                {
                    //set to auto resize
                    AutoResize = true;
                    GetNextImagePosition(img);
                }
                else
                {
                    return;
                }
            }
            mWindow.canvasControl.Children.Add(img.ImageControl);
            HasChanged = true;

            mSpritesList.Add(img);

            mAtlasDoc.AddSprite(
                img.ID.ToString(),
                ((int)img.Left).ToString(),
                ((int)img.Top).ToString(),
                ((int)img.Width).ToString(),
                ((int)img.Height).ToString());


        }

        /// <summary>
        /// Clears all previously loaded sprites and sets the sheet to default settings.
        /// </summary>
        public void Clear()
        {
            mWindow.canvasControl.Children.RemoveRange(0, mWindow.canvasControl.Children.Count);
            Width = DEFAULT_IMAGE_WIDTH;
            Height = DEFAULT_IMAGE_HEIGHT;
            mSpritesList.Clear();
            mAtlasDoc.Clear();
            HasChanged = false;
        }

        private string XmlToPngFile(string file)
        {
            int dotIndex = file.LastIndexOf('.');
            //substring file without extension and concatenate .png to it.
            return file.Substring(0, dotIndex) + ".png";
        }

        private void ParseFilePath(string filePath, out string path, out string file)
        {
            int whackIndex = filePath.LastIndexOf('\\');
            path = filePath.Substring(0, whackIndex + 1);
            file = filePath.Substring(whackIndex + 1, filePath.Length - (whackIndex + 1));
        }

        private void SaveImageFile(string filePath)
        {

            //create bitmap to hold sprites
            WriteableBitmap finalImage = BitmapFactory.New((int)Width, (int)Height);

            //add the sprites to the bitmap
            foreach (Image2 img in mSpritesList)
            {
                //
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

        private MessageBoxResult PopNotEnoughRoom()
        {
            return MessageBox.Show("Your sheet is too small to add this image. Would you like to resize automatically?", "Not enough room.", MessageBoxButton.YesNo);
        }

        private void SwitchSettingsPanelUI()
        {
            if (mAutoResize)
            {
                mWindow.txtHeightWrite.Visibility = System.Windows.Visibility.Hidden;
                mWindow.txtWidthWrite.Visibility = System.Windows.Visibility.Hidden;
                mWindow.txtHeightReadOnly.Visibility = System.Windows.Visibility.Visible;
                mWindow.txtWidthReadOnly.Visibility = System.Windows.Visibility.Visible;

                mWindow.lblWidth.Content = "Width";
                mWindow.lblHeight.Content = "Height";
            }
            else
            {
                mWindow.txtHeightWrite.Visibility = System.Windows.Visibility.Visible;
                mWindow.txtWidthWrite.Visibility = System.Windows.Visibility.Visible;
                mWindow.txtHeightReadOnly.Visibility = System.Windows.Visibility.Hidden;
                mWindow.txtWidthReadOnly.Visibility = System.Windows.Visibility.Hidden;
                mWindow.lblWidth.Content = "_Width";
                mWindow.lblHeight.Content = "_Height";
            }
        }

        private bool GetNextImagePosition(Image2 newImage)
        {
            //position will be default values 0,0
            if (mSpritesList.Count == 0)
            {
                //need to check if image fits
                if (Width > newImage.Width && Height > newImage.Height)
                {
                    return true;
                }
                else
                {
                    if (!AutoResize)
                    {
                        return false;
                    }
                    else//auto resize so adjust the canvas
                    {
                        if (Width > newImage.Width)
                        {
                            Width = newImage.Width;
                        }
                        if (Height > newImage.Height)
                        {
                            Height = newImage.Height;
                        }
                        return true;
                    }

                }
            }

            Image2 lastImage = mSpritesList[mSpritesList.Count - 1];
            //check if need new row
            double highestYInRow = GetHighestYInRow();
            if (lastImage.Left + lastImage.Width + newImage.Width > Width)
            {
                ////wont fit in this row, will fit in new row?
                if (highestYInRow + newImage.Height > Height || newImage.Width > Width)
                {
                    if (!AutoResize)
                    {
                        return false;
                    }
                    else
                    {
                        //resize the canvas
                        if (newImage.Width > Width)
                        {
                            Width = newImage.Width;
                        }
                        if (highestYInRow + newImage.Height > Height)
                        {
                            Height = highestYInRow + newImage.Height;
                        }
                    }

                }

                newImage.Left = 0;
                newImage.Top = highestYInRow;
            }
            else
            {
                if (lastImage.Top + newImage.Height > Height)
                {
                    if (!AutoResize)
                    {
                        return false;
                    }
                    else
                    {
                        Height = lastImage.Top + newImage.Height;
                    }

                }
                newImage.Left = lastImage.Left + lastImage.Width;
                newImage.Top = lastImage.Top;
            }
            return true;
        }

        private double GetHighestYInRow()
        {
            double result = 0;
            foreach (Image2 img in mSpritesList)
            {
                if (img.Top + img.Height > result) result = img.Top + img.Height;
            }
            return result;
        }

        

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
