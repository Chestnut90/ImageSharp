using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Algorithms
{
    public class EllipseHoleDetector : KMean
    {
        public EllipseHoleDetector(Mat image, int k = 3) : base(image, k)
        {
        }

        private double OpencvBGRToGray(OpenCvSharp.Vec3b pixel)
        {
            // bgr
            // opencv rgb to gray = r * 0.299 + g * 0.587 + b * 0.114

            byte B = pixel.Item0;
            byte G = pixel.Item1;
            byte R = pixel.Item2;

            return R * 0.299 + G * 0.587 + B * 0.114;
        }

        public List<Tuple<OpenCvSharp.Mat, String>> Contours()
        {
            if (!this.IsComputed)
            {
                this.ComputeKMeans();
            }

            // to gray
            OpenCvSharp.Mat gray = new Mat(Source.Size(), OpenCvSharp.MatType.CV_8UC1);
            OpenCvSharp.Cv2.CvtColor(this.Source, gray, ColorConversionCodes.BGR2GRAY);

            // kmean to gray
            OpenCvSharp.Mat kmeanGray = new Mat(Source.Size(), OpenCvSharp.MatType.CV_8UC1);
            OpenCvSharp.Cv2.CvtColor(this.KMeansToImage(), kmeanGray, ColorConversionCodes.BGR2GRAY);

            // check values
            Dictionary<byte, bool> values = new Dictionary<byte, bool>();
            for (int y = 0; y < kmeanGray.Height; y++)
            {
                for (int x = 0; x < kmeanGray.Width; x++)
                {
                    byte pixel = kmeanGray.Get<byte>(y, x);
                    if(!values.ContainsKey(pixel))
                    {
                        values.Add(pixel, true);
                    }
                }
            }

            var keys = this.Clusters.Keys.ToList();
            keys.Sort((pixel0, pixel1) =>
            {
                double threshold0 = this.OpencvBGRToGray(pixel0);
                double threshold1 = this.OpencvBGRToGray(pixel1);

                return threshold0 > threshold1 ? 1 : 0;
            });

            List<Tuple<OpenCvSharp.Mat, String>> result = new List<Tuple<OpenCvSharp.Mat, String>>();
            for(int i = 0; i < this.K * 3; i++)
            {
                result.Add(new Tuple<Mat, string>(null, string.Empty));
            }

            int index = 0;
            foreach (var pixel in keys)
            {

                double threshold = (int)this.OpencvBGRToGray(pixel);    // threshold to floor
                var points = this.Clusters[pixel];
                
                // binaries
                OpenCvSharp.Mat binary = new Mat(gray.Size(), OpenCvSharp.MatType.CV_8UC1, Scalar.Black);
                foreach(var point in points)
                {
                    binary.Set(point.Y, point.X, 255);
                }
                result[index + 0] = new Tuple<Mat, string>(binary, $"{threshold}-binary");

                // contours
                Mat hierarch = new Mat();
                OpenCvSharp.Cv2.FindContours(binary, out Mat[] contours, hierarch, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                Mat kmeanContour = Source.Clone();
                for (int i = 0; i < contours.Length; i++)
                {
                    OpenCvSharp.Cv2.DrawContours(kmeanContour, contours, i, Scalar.Red, 2, LineTypes.AntiAlias);
                }
                result[index + 1] = new Tuple<Mat, string>(kmeanContour, $"{threshold}-contour");

                // ellipse
                List<RotatedRect> ellipses = new List<RotatedRect>();
                foreach (var contour in contours)
                {
                    if (contour.Rows < 20)
                    {
                        continue;
                    }
                    RotatedRect ellipse = contour.FitEllipse();
                    ellipses.Add(ellipse);
                }

                Mat kmeanEllipse = Source.Clone();
                foreach(var ellipse in ellipses)
                {
                    kmeanEllipse.Ellipse(ellipse, Scalar.Red, 2, LineTypes.AntiAlias);
                }
                result[index + 2] = new Tuple<Mat, string>(kmeanEllipse, $"{threshold}-ellipse");

                index += 3;
            }

            return result;
        }

    }
}
