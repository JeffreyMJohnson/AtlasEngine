using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public MainWindow()
        {
            InitializeComponent();
            //SpriteSheet sheet = new SpriteSheet(canvasControl, "", 512, 256, false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CreateNewSheet();
            TestIt();
        }

        /// <summary>
        ///TODO: DEBUG USE ONLY, THIS SHOULD NOT BE HERE FOR RELEASE
        /// </summary>
        void TestIt()
        {
            mSheet = new SpriteSheet(canvasControl, basePath, 256, 256, false);
            for (int i = 0; i < 10; i++)
            {
                mSheet.AddSprite(@"test_images\med\green_square_med.png");
            }

            for (int i = 0; i < 10; i++)
            {
                mSheet.AddSprite(@"test_images\small\eight_ball_small.png");
            }
        }
        private void CreateNewSheet()
        {
            LoadSheetDialog dialog = new LoadSheetDialog();
            if (dialog.ShowDialog() == true)
            {
                double width, height;
                if(Double.TryParse(dialog.Width, out width) && Double.TryParse(dialog.Height, out height))
                {
                    mSheet = new SpriteSheet(canvasControl, AppDomain.CurrentDomain.BaseDirectory, width, height, false);
                }
            }
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
                string text = "";

                for (int i = 0; i < fileNames.Length; i++)
                {
                    text += fileNames[i] + "\n";
                    Console.WriteLine(fileNames[i]);
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
            this.Close();
        }
    }
}
