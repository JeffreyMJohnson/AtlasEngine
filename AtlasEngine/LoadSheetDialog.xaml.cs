using System.Windows;

namespace AtlasEngine
{
    /// <summary>
    /// Interaction logic for LoadSheetDialog.xaml
    /// </summary>
    public partial class LoadSheetDialog : Window
    {
        public LoadSheetDialog()
        {
            InitializeComponent();
            width.Focus();
        }

        public string Width
        {
            get { return width.Text; }
            set { width.Text = value; }
        }

        public string Height
        {
            get { return height.Text; }
            set { height.Text = value; }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }


    }
}
