using Models.OpenCV.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models.IPModels
{
    public class EllipseDetectorInterface
    {
        EllipseDetector detector;

        public EllipseDetectorInterface(BitmapSource bitmap, int k)
        {
            OpenCvSharp.Mat image = OpenCV.CvInterface.BitmapSourceToMat(bitmap);

            this.detector = new EllipseDetector(image, k);
            this.detector.ComputeKMeans();
        }

        public BitmapSource GetKmeanImage()
        {
            if (!this.detector.IsComputed)
            {
                return null;
            }

            OpenCvSharp.Mat image = this.detector.KMeansToImage();
            return OpenCV.CvInterface.MatToWriteableBitmap(image);
        }

        public List<Tuple<BitmapSource, string>> GetContours()
        {
            if(!this.detector.IsComputed)
            {
                return null;
            }
            List<Tuple<BitmapSource, string>> list = new List<Tuple<BitmapSource, string>>();
            foreach(var item in this.detector.Contours())
            {
                WriteableBitmap bitmap = OpenCV.CvInterface.MatToWriteableBitmap(item.Item1);
                list.Add(new Tuple<BitmapSource, string>(bitmap, item.Item2));
            }
            return list;
        }

    }
}
