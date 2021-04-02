using System;
using System.Collections.Generic;
using System.IO;
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

namespace ImageProcessing.Views
{
    /// <summary>
    /// ucSimpleViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucSimpleViewer : UserControl
    {
        public ucSimpleViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// original image
        /// </summary>
        private BitmapImage origin;

        private BitmapImage image;
        public BitmapImage Image
        {
            get => image;
            private set
            {
                this.image = value;
                this.ImageViewer.Source = this.image;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // empty image
            BitmapImage image = this.CreateEmptyBitmapImage();
            this.Image = image;
        }

        /// <summary>
        /// Image Load button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                this.Image = LoadImage(ofd.FileName);
                this.origin = this.Image;
            }
        }

        /// TODO : move business
        /// <summary>
        /// Create Empty Bitmap Image
        /// </summary>
        /// <returns></returns>
        private BitmapImage CreateEmptyBitmapImage()
        {
            int width = 512;
            int height = 512;
            double dpixy = 96;
            int stride = width / 8; // stride?
            byte[] pixels = new byte[height * stride];

            for (int i = 0; i < height * stride; i++)
            {
                int mod = i % 3;

                byte value = 0;
                switch (mod)
                {
                    case 0:
                        value = 0;
                        break;
                    case 1:
                        value = 50;
                        break;
                    case 2:
                        value = 200;
                        break;
                    default:
                        break;
                }
                pixels[i] = value;
            }

            // rgb
            List<Color> colors = new List<Color>();
            colors.Add(Colors.Red);
            colors.Add(Colors.Blue);
            colors.Add(Colors.Green);
            BitmapPalette pallete = new BitmapPalette(colors);


            BitmapSource source = BitmapSource.Create(width, height, dpixy, dpixy, PixelFormats.Indexed1, pallete, pixels, stride);
            BitmapImage image = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(memoryStream);

                memoryStream.Position = 0;
                image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = memoryStream;
                image.EndInit();
            }
            return image;
        }

        /// TODO : move to business
        /// <summary>
        /// Load Image with path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private BitmapImage LoadImage(string path)
        {
            BitmapImage image = null;
            try
            {
                // not exists
                if (!File.Exists(path))
                {
                    throw new Exception("Invalid path.");
                }

                image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);    // path to uri
                image.EndInit();

                if (image.CanFreeze)
                {
                    image.Freeze();
                }

            }
            catch (Exception e)
            {
                image = null;
            }
            return image;
        }

        private void Origin_Click(object sender, RoutedEventArgs e)
        {
            this.Image = this.origin;
        }

        private void ButtonKmean_Click(object sender, RoutedEventArgs e)
        {
            // bitmap to open cv Mat
            InputDialogs.KmeanDialog kmeanDialog = new InputDialogs.KmeanDialog();

            if (kmeanDialog.ShowDialog() == false)
            {
                return;
            }

            //int kmeanValue = kmeanDialog.KmeanValue;
            //OpenCvSharp.Mat mat = Processing.CvInterface.ReadImage(this.origin.UriSource.AbsolutePath);

            //Processing.EllipseHoleDetector detector = new Processing.EllipseHoleDetector(mat, kmeanValue);
            //bool res = detector.ComputeKMeans();
            //if (res)
            //{
            //    this.Image = Processing.CvInterface.MatToBitmapImage(detector.KMeansToImage());
            //}

            //detector.Contours();

        }
    }
}
