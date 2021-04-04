using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models.OpenCV
{
    public class CvInterface
    {
        /// <summary>
        /// Read Image using OpenCV
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static OpenCvSharp.Mat ReadImage(string path)
        {
            return OpenCvSharp.Cv2.ImRead(path);
        }

        private static OpenCvSharp.Mat BGRToRGB(OpenCvSharp.Mat image)
        {
            int width = image.Width;
            int height = image.Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // color format change
                    var origin = image.Get<OpenCvSharp.Vec3b>(y, x);

                    image.Set<OpenCvSharp.Vec3b>(y, x, new OpenCvSharp.Vec3b()
                    {
                        Item0 = origin.Item2,
                        Item1 = origin.Item1,
                        Item2 = origin.Item0,
                    });
                }
            }
            return image;
        }

        public static BitmapImage MatToBitmapImage(OpenCvSharp.Mat image)
        {
            BitmapImage bitmapImage = null;

            using (var ms = image.ToMemoryStream())
            {
                bitmapImage = new BitmapImage();
                ms.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.UriSource = null;
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        public static WriteableBitmap MatToWriteableBitmap(OpenCvSharp.Mat image)
        {
            //int width = image.Width;
            //int height = image.Height;
            //WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);

            return OpenCvSharp.Extensions.WriteableBitmapConverter.ToWriteableBitmap(image);
        }

        public static OpenCvSharp.Mat BitmapSourceToMat(BitmapSource bitmap)
        {
            return OpenCvSharp.Extensions.BitmapSourceConverter.ToMat(bitmap);
        }

        #region nXSDK image formats
        public static OpenCvSharp.Mat ReadZmapFormat(string path)
        {
            throw new NotImplementedException();
            //nXSDKNET.Format.ZMapFormat zmap = nXSDKNET.Format.ZMapFormat.Open(path);

            //int width = zmap.GetImageWidth();
            //int height = zmap.GetImageHeight();

            //OpenCvSharp.Mat image = new OpenCvSharp.Mat(width, height, OpenCvSharp.MatType.CV_32FC1, zmap.GetZmap());
            //zmap = null;
            //return image;
        }

        public static OpenCvSharp.Mat ReadZmapFormatRGB(string path)
        {
            throw new NotImplementedException();
            //nXSDKNET.Format.ZMapFormat zmap = nXSDKNET.Format.ZMapFormat.Open(path);

            //int width = zmap.GetImageWidth();
            //int height = zmap.GetImageHeight();

            //List<Tuple<byte, byte, byte>> pixels = zmap.ConvertRGB();
            //OpenCvSharp.Mat image = null;
            //if (pixels.Count != 0)
            //{
            //    image = new OpenCvSharp.Mat(width, height, OpenCvSharp.MatType.CV_8UC3, OpenCvSharp.Scalar.Black);

            //    for (int y = 0; y < height; y++)
            //    {
            //        for (int x = 0; x < width; x++)
            //        {
            //            int index = y * width + x;
            //            Tuple<byte, byte, byte> pixel = pixels[index];
            //            OpenCvSharp.Vec3b vec3b = new OpenCvSharp.Vec3b()
            //            {
            //                Item0 = pixel.Item3,
            //                Item1 = pixel.Item2,
            //                Item2 = pixel.Item1,
            //            };
            //            image.Set<OpenCvSharp.Vec3b>(y, x, vec3b);
            //        }
            //    }
            //}

            //zmap = null;
            //return image;

        }

        #endregion
    }
}
