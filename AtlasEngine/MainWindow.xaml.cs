using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;


namespace AtlasEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO: REMOVE FOR RELEASE
        string basePath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\resources\";
        SpriteSheet mSheet;

        /// <summary>
        /// MainWindow constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
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
            root.Document = mSheet.AtlasDoc;
        }

        /// <summary>
        ///TODO: DEBUG USE ONLY, THIS SHOULD NOT BE HERE FOR RELEASE
        /// </summary>
        void TestIt()
        {
            mSheet = new SpriteSheet(this, canvasControl, basePath, 256, 256, false);
            for (int i = 0; i < 10; i++)
            {
                mSheet.AddSprite(basePath + @"test_images\med\green_square_med.png");
            }

            for (int i = 0; i < 10; i++)
            {
                mSheet.AddSprite(basePath + @"test_images\small\eight_ball_small.png");
            }
        }

        /// <summary>
        /// Called to clear current sheet and create a new one.
        /// </summary>
        private void CreateNewSheet()
        {
            if (mSheet.HasChanged)
            {

                if (PopAreYouSureBox() == MessageBoxResult.Yes)
                {
                    mSheet.Clear();
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
                    mSheet.AddSprite(fileNames[i]);
                }

            }
        }

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

    }
}
