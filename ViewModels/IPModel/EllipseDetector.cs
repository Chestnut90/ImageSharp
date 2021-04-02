using ImageProcessing.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ViewModels.IPModel
{
    public class EllipseDetector
    {
        private EllipseHoleDetector Detector;
        private SurfLoader Surf;

        public EllipseDetector(SurfLoader surf, int k)
        {
            this.Surf = surf;
            this.Detector = new EllipseHoleDetector(this.Surf.ZmapRGB, k);
        }

        public bool ComputeKmean()
        {
            return this.Detector.ComputeKMeans();
        }

        public BitmapImage KMeansToBitmapImage()
        {
            OpenCvSharp.Mat image = this.Detector.KMeansToImage();
            return ImageProcessing.Interface.CvInterface.MatToBitmapImage(image);
        }

        public List<ThumbNails.ThumbNailViewModel> GetContours()
        {
            var list = this.Detector.Contours();

            List<ThumbNails.ThumbNailViewModel> result = new List<ThumbNails.ThumbNailViewModel>();
            foreach (var item in list)
            {
                BitmapImage bitmap = ImageProcessing.Interface.CvInterface.MatToBitmapImage(item.Item1);
                result.Add(new ThumbNails.ThumbNailViewModel(bitmap, item.Item2));
            }

            return result;
        }
    }

}