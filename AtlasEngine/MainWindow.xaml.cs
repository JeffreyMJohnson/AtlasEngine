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
        SpriteSheet mSheet;
        public MainWindow()
        {
            InitializeComponent();
            SpriteSheet sheet = new SpriteSheet(canvasControl, "", 512, 256, false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateNewSheet();
        }

        private void CreateNewSheet()
        {
            LoadSheetDialog dialog = new LoadSheetDialog();
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Width: " + dialog.Width + "\nHeight: " + dialog.Height);
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
    }
}
