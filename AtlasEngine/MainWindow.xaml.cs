using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace AtlasEngine
{
    /*TODO
     * implement auto resize in main window
     */
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO: REMOVE FOR RELEASE
        string basePath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\resources\";
        SpriteSheet mSheet;
        private const uint DEFAULT_SHEET_WIDTH = 512;
        private const uint DEFAULT_SHEET_HEIGHT = 512;
        private AtlasDocument mAtlasDoc = new AtlasDocument(DEFAULT_SHEET_WIDTH.ToString(), DEFAULT_SHEET_HEIGHT.ToString());
        private bool mIsCanvasDirty = false;
        private bool mAutoResizeCanvas = true;


        /// <summary>
        /// MainWindow constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //SetCanvasBackground();
            //IsCanvasDirty = false;
            chkAutoResize.DataContext = this;
        }

        public bool AutoResize
        {
            get 
            {
                return mAutoResizeCanvas;
            }
            set
            {
                mAutoResizeCanvas = value;
                SwitchSettingsPanelUI();
            }
        }

        public bool IsCanvasDirty
        {
            get
            {
                return mIsCanvasDirty;
            }

            set
            {
                if (value)
                {
                    //turn background yellow
                    canvasScrollviewer.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 00));
                }
                else
                {
                    //turn green
                    canvasScrollviewer.Background = new SolidColorBrush(Color.FromArgb(100, 00, 255, 00));
                }
                mIsCanvasDirty = value;
            }
        }

        /// <summary>
        /// Window_Loaded event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TestIt();
            settingsPanel.DataContext = mSheet;
            XmlDataProvider root = FindResource("xmlData") as XmlDataProvider;
            root.Document = mAtlasDoc.XMLDoc;
        }

        /// <summary>
        ///TODO: DEBUG USE ONLY, THIS SHOULD NOT BE HERE FOR RELEASE
        /// </summary>
        void TestIt()
        {
            mSheet = new SpriteSheet(this, basePath, 256, 256, false);
            for (int i = 0; i < 2; i++)
            {
                //mSheet.AddSprite(basePath + @"test_images\med\green_square_med.png");
                AddSprite(basePath + @"test_images\med\green_square_med.png");
            }

            for (int i = 0; i < 2; i++)
            {
               // mSheet.AddSprite(basePath + @"test_images\small\eight_ball_small.png");
                AddSprite(basePath + @"test_images\small\eight_ball_small.png");
            }
        }

        /// <summary>
        /// Called to clear current sheet and create a new one.
        /// </summary>
        private void CreateNewSheet()
        {
            if (IsCanvasDirty)
            {

                if (PopAreYouSureBox() == MessageBoxResult.Yes)
                {
                    //mSheet.Clear();
                    ClearCanvas();
                }
            }

        }

        /// <summary>
        /// Helper function for popping 'Are you sure' message box.
        /// </summary>
        /// <returns></returns>
        private MessageBoxResult PopAreYouSureBox()
        {
            return MessageBox.Show("Your sheet has changed since your last save, you will lose this work.", "Are You Sure?", MessageBoxButton.YesNo);
        }

        /// <summary>
        /// Handler function called when user selects 'add new sprite(s)' in menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMenuFileSelectClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.DefaultExt = ".png";
            dialog.Filter = "Image Files|*.png;*.bmp;*.jpg";

            //show the box
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string[] fileNames = dialog.FileNames;

                for (int i = 0; i < fileNames.Length; i++)
                {
                    /*implement addSprite function and call it here*/
                    //mSheet.AddSprite(fileNames[i]);
                    AddSprite(fileNames[i]);
                }

            }
        }


        /// <summary>
        /// Handler function called when user selects '
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleNewSpriteSheetClick(object sender, RoutedEventArgs e)
        {
            CreateNewSheet();
        }

        private void HandleSaveSheetClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.FileName = "AtlasEngine";
            dlg.Filter = "XML documents (.xml)|*.xml";
            dlg.AddExtension = true;
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (dlg.ShowDialog() == true)
            {
                mSheet.Save(dlg.FileName);
            }
        }

        private void HandleMenuExitClick(object sender, RoutedEventArgs e)
        {
            if (mSheet.HasChanged)
            {
                if (PopAreYouSureBox() == MessageBoxResult.No)
                {
                    return;
                }
            }
            this.Close();

        }

        private void HighlightTextbox(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void AddSprite(string filePath)
        {
            Image newImageControl = new Image();
            BitmapImage bmp = new BitmapImage(new Uri(filePath));
            newImageControl.Source = bmp;
            newImageControl.Stretch = Stretch.None;

            SetSpritePosition(ref newImageControl);
            canvasControl.Children.Add(newImageControl);
            IsCanvasDirty = true;

            mAtlasDoc.AddSprite(
                (canvasControl.Children.Count - 1).ToString(),
                ((int)Canvas.GetLeft(newImageControl)).ToString(),
                ((int)Canvas.GetTop(newImageControl)).ToString(),
                ((int)newImageControl.Source.Width).ToString(),
                ((int)newImageControl.Source.Height).ToString());
            
        }

        private void  SetSpritePosition(ref Image img)
        {
            //assumes auto-resize when time to implement fixed size throw an exception if wolnt fit
            Image lastImg = null;
            if(canvasControl.Children.Count != 0)
            {
                lastImg = canvasControl.Children[canvasControl.Children.Count - 1] as Image;
                //lastImg = GetLastImage();
            }
            if(lastImg == null)
            {
                Canvas.SetLeft(img, 0);
                Canvas.SetTop(img, 0);
            }
            else if(lastImg.Source.Width + Canvas.GetLeft(lastImg) + img.Source.Width > canvasControl.Width)
            {
                Canvas.SetLeft(img, 0);
                //resize canvas height if needed
                if(lastImg.Source.Height + Canvas.GetTop(lastImg) + img.Source.Height > canvasControl.Height)
                {
                    canvasControl.Height = lastImg.Source.Height + Canvas.GetTop(lastImg) + img.Source.Height;
                }
                //calculate for highest y here
                Canvas.SetTop(img, GetHighestYInCanvas());
            }
            else
            {
                Canvas.SetLeft(img, lastImg.Source.Width + Canvas.GetLeft(lastImg));
                Canvas.SetTop(img, Canvas.GetTop(lastImg));
            }
           

        }

        private double GetHighestYInCanvas()
        {
            double result = 0;
            foreach(Image img in canvasControl.Children)
            {
                if(img.Source.Height + Canvas.GetTop(img) > result)
                {
                    result = img.Source.Height + Canvas.GetTop(img);
                }
            }
            return result;
        }

        private Image GetLastImage()
        {
            if (canvasControl.Children.Count == 0)
                return null;

            double highestY = 0;
            double highestX = 0;
            //find highest x and y values
            foreach(Image img in canvasControl.Children)
            {
                if(Canvas.GetLeft(img) > highestX)
                {
                    highestX = Canvas.GetLeft(img);
                }
                if(Canvas.GetTop(img) > highestY)
                {
                    highestY = Canvas.GetTop(img);
                }
            }
            foreach(Image img in canvasControl.Children)
            {
                if(Canvas.GetLeft(img) == highestX && Canvas.GetTop(img) == highestY)
                {
                    return img;
                }
            }
            return null;
        }

        private void SetCanvasBackground()
        {
            ImageBrush brush = new ImageBrush();
            BitmapImage image = new BitmapImage(new Uri(basePath + "checkerboard_tile.png"));
            brush.ImageSource = image;
            brush.TileMode = TileMode.Tile;
            brush.ViewportUnits = BrushMappingMode.Absolute;
            brush.Viewport = new Rect(0, 0, 100, 100);
            brush.Opacity = .5;
            canvasControl.Background = brush;
        }

        private void ClearCanvas()
        {
            canvasControl.Children.Clear();
            IsCanvasDirty = false;
            mAtlasDoc.Clear();
        }

        private void SwitchSettingsPanelUI()
        {
            if (AutoResize)
            {
                txtHeightWrite.Visibility = System.Windows.Visibility.Hidden;
                txtWidthWrite.Visibility = System.Windows.Visibility.Hidden;
                txtHeightReadOnly.Visibility = System.Windows.Visibility.Visible;
                txtWidthReadOnly.Visibility = System.Windows.Visibility.Visible;

                lblWidth.Content = "Width";
                lblHeight.Content = "Height";
            }
            else
            {
                txtHeightWrite.Visibility = System.Windows.Visibility.Visible;
                txtWidthWrite.Visibility = System.Windows.Visibility.Visible;
                txtHeightReadOnly.Visibility = System.Windows.Visibility.Hidden;
                txtWidthReadOnly.Visibility = System.Windows.Visibility.Hidden;
                lblWidth.Content = "_Width";
                lblHeight.Content = "_Height";
            }
        }

    }
}
