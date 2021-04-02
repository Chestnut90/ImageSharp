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
using ViewModels.IPModel;
using ViewModels.ThumbNails;

namespace ImageProcessing.Views.KMeanDebugerView
{
    /// <summary>
    /// UcKMeanDebuger.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcKMeanDebuger : UserControl
    {
        public UcKMeanDebuger()
        {
            InitializeComponent();
            this.UcThumNailList.ThumbNailClickEvent += UcThumNailList_ThumbNailClickEvent;
        }

        private void UcThumNailList_ThumbNailClickEvent(object sender, EventArgs e)
        {
            var thumbnail = sender as ViewModels.ThumbNails.ThumbNailViewModel;

            if (thumbnail.Image.Equals(this.MainImage.Source))
            {
                return;
            }

            //swap
            this.MainImage.Source = thumbnail.Image;
        }

        private EllipseDetector Detector = null;
        private SurfLoader SurfLoader = new SurfLoader();
        private BitmapImage image = null;


        private void AddThumbNail(ThumbNailViewModel viewmodel)
        {
            UcThumNailList.InsertItem(viewmodel);
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

                var zmapFloat = SurfLoader.LoadSurfImage(surfFileName, ZmapType.FLOAT);
                ThumbNailViewModel viewmodel = new ThumbNailViewModel(zmapFloat, "Zmap float");
                this.AddThumbNail(viewmodel);

                var zmapRGB = SurfLoader.LoadSurfImage(surfFileName, ZmapType.RGB);
                this.AddThumbNail(new ThumbNailViewModel(zmapRGB, "Zmap RGB"));

                this.MainImage.Source = zmapFloat;
                this.image = zmapRGB;
            }
        }

        /// <summary>
        /// Compute Kmean and viewing kmean
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonKmean_Click(object sender, RoutedEventArgs e)
        {
            InputDialogs.KmeanDialog dialog = new InputDialogs.KmeanDialog();

            if (dialog.ShowDialog() == false)
            {
                return;
            }

            int k = dialog.KmeanValue;
            this.Detector = new EllipseDetector(this.SurfLoader, k);
            bool res = this.Detector.ComputeKmean();
            if (!res)
            {
                return;
            }

            var image = this.Detector.KMeansToBitmapImage();
            this.AddThumbNail(new ThumbNailViewModel(image, "KMean"));
        }

        private void ButtonBinary_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in this.Detector.GetContours())
            {
                this.AddThumbNail(item);
            }
        }

        private void ButtonContour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDetection_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
