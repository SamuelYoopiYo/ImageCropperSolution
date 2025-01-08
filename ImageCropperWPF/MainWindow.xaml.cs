using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageCropperLibrary;

namespace ImageCropperWPF
{
    public partial class MainWindow : Window
    {
        private readonly ImageProcessor _imageProcessor = new();
        private readonly RectangleArea _rectangleArea = new(200, 200, 16.0 / 9.0);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _imageProcessor.LoadImage(openFileDialog.FileName);
                LoadedImage.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(openFileDialog.FileName));
            }
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e) => _imageProcessor.RotateImage(-90);
        private void RotateRight_Click(object sender, RoutedEventArgs e) => _imageProcessor.RotateImage(90);
        private void ZoomIn_Click(object sender, RoutedEventArgs e) => _imageProcessor.ZoomImage(_imageProcessor.Zoom + 0.1);
        private void ZoomOut_Click(object sender, RoutedEventArgs e) => _imageProcessor.ZoomImage(_imageProcessor.Zoom - 0.1);
        private void SaveCropped_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                _imageProcessor.SaveCroppedImage(_rectangleArea, saveFileDialog.FileName);
            }
        }
    }
}