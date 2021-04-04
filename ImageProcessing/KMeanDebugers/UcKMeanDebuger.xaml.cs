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

using Models.IPModels;
using Models.OpenCV.Algorithms;
using Models.ViewModels.ThumbNails;

namespace Views.KMeanDebugers
{
    /// <summary>
    /// UcKMeanDebuger.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcKMeanDebuger : UserControl
    {
        public UcKMeanDebuger()
        {
            InitializeComponent();
            this.UcThumNailList.ThumbNailClik += this.UcThumNailList_ThumbNailClickEvent;
        }

        private void UcThumNailList_ThumbNailClickEvent(object sender, EventArgs e)
        {
            var thumbnail = sender as ThumbNailViewModel;

            if (thumbnail.Image.Equals(this.MainImage.GetBitmap()))
            {
                return;
            }

            //swap
            this.MainImage.SetBitmap(thumbnail.Image);
        }

        private EllipseDetectorInterface Detector = null;
        private ImageLoader ImageLoader = new ImageLoader();
        private BitmapImage image = null;


        private void AddThumbNail(BitmapSource image, string title)
        {
            this.UcThumNailList.InsertThumbNail(new ThumbNailViewModel(image, title));
        }

        /// <summary>
        /// Load surf image and viewing float gray / rgb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLoadSurf_Click(object sender, RoutedEventArgs e)
        {
            // load surf and rgb / float gray
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Surf Files(*.surf)|*.surf";
            if (ofd.ShowDialog() == true)
            {
                string surfFileName = ofd.FileName;

                //var zmapFloat = SurfLoader.LoadSurfImage(surfFileName, ZmapType.FLOAT);
                //ThumbNailViewModel viewmodel = new ThumbNailViewModel(zmapFloat, "Zmap float");
                //this.AddThumbNail(viewmodel);

                //var zmapRGB = SurfLoader.LoadSurfImage(surfFileName, ZmapType.RGB);
                //this.AddThumbNail(new ThumbNailViewModel(zmapRGB, "Zmap RGB"));

                //this.MainImage.Source = zmapFloat;
                //this.image = zmapRGB;
            }
        }

        /// <summary>
        /// Compute Kmean and viewing kmean
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonKmean_Click(object sender, RoutedEventArgs e)
        {
            WinKMeanInputDialog dialog = new WinKMeanInputDialog();

            if (dialog.ShowDialog() == false)
            {
                return;
            }

            int k = dialog.KmeanValue;
            this.Detector = new EllipseDetectorInterface(this.MainImage.GetBitmap(), k);
            var image = this.Detector.GetKmeanImage();
            if (image is null)
            {
                return;
            }

            this.AddThumbNail(image, "KMean");
        }

        private void ButtonBinary_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.Detector.GetContours())
            {
                this.AddThumbNail(item.Item1, item.Item2);
            }
        }

        private void ButtonContour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDetection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonLoadImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg)|*.jpg";
            if (ofd.ShowDialog() == true)
            {
                string filepath = ofd.FileName;
                var image = this.ImageLoader.LoadImage(filepath);

                string fileTitle = System.IO.Path.GetFileName(filepath);
                this.AddThumbNail(image, fileTitle);
                
            }
        }
    }
}
