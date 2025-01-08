using System;
using System.Drawing;
using System.IO;
using System.Windows.Input;

namespace ImageCropperLibrary
{
    // 1. RectangleArea class - отвечает за работу с выделенной областью
    public class RectangleArea
    {
        public event EventHandler AreaChanged;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public double AspectRatio { get; private set; }

        public RectangleArea(int width, int height, double aspectRatio)
        {
            Width = width;
            Height = height;
            AspectRatio = aspectRatio;
        }

        public void UpdateArea(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            AreaChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetAspectRatio(double aspectRatio)
        {
            AspectRatio = aspectRatio;
            // пересчитать ширину и высоту с учетом новых пропорций
            Height = (int)(Width / aspectRatio);
            AreaChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // 2. ImageProcessor class - отвечает за обработку изображений
    public class ImageProcessor
    {
        public Bitmap LoadedImage { get; private set; }
        public double Zoom { get; private set; } = 1.0;
        public int RotationAngle { get; private set; } = 0;

        public void LoadImage(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image not found");

            LoadedImage = new Bitmap(filePath);
        }

        public void RotateImage(int angle)
        {
            RotationAngle = angle;
            // Реализация поворота изображения
        }

        public void ZoomImage(double zoom)
        {
            Zoom = zoom;
        }

        public void SaveCroppedImage(RectangleArea area, string outputPath)
        {
            if (LoadedImage == null) throw new InvalidOperationException("No image loaded");

            Rectangle cropRect = new Rectangle(area.X, area.Y, area.Width, area.Height);
            Bitmap croppedImage = LoadedImage.Clone(cropRect, LoadedImage.PixelFormat);
            croppedImage.Save(outputPath);
        }
    }

    // 3. SettingsManager class - отвечает за сохранение/восстановление настроек
    public class SettingsManager
    {
        public string SettingsFilePath { get; private set; } = "settings.json";

        public void SaveSettings(object settings)
        {
            File.WriteAllText(SettingsFilePath, System.Text.Json.JsonSerializer.Serialize(settings));
        }

        public T LoadSettings<T>() where T : new()
        {
            if (!File.Exists(SettingsFilePath)) return new T();

            string json = File.ReadAllText(SettingsFilePath);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }

    // 4. GuideLines class - отвечает за отображение направляющих
    public class GuideLines
    {
        public bool ShowCenterLines { get; set; }
        public bool ShowGridLines { get; set; }

        public event EventHandler GuideLinesChanged;

        public void ToggleCenterLines()
        {
            ShowCenterLines = !ShowCenterLines;
            GuideLinesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ToggleGridLines()
        {
            ShowGridLines = !ShowGridLines;
            GuideLinesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
