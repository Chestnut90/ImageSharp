using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Bitmap Images
using System.Windows.Media; 
using System.Windows.Media.Imaging; // bitmap image

namespace ImageProcessing.Interface
{
    public class CvInterface
    {
        public static OpenCvSharp.Mat ReadImage(string path)
        {
            return OpenCvSharp.Cv2.ImRead(path);
        }

        public static BitmapImage MatToBitmapImage(OpenCvSharp.Mat source)
        {
            int width = source.Width;
            int height = source.Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // color format change
                    var origin = source.Get<OpenCvSharp.Vec3b>(y, x);

                    source.Set<OpenCvSharp.Vec3b>(y, x, new OpenCvSharp.Vec3b()
                    {
                        Item0 = origin.Item2,
                        Item1 = origin.Item1,
                        Item2 = origin.Item0,
                    });
                }
            }

            BitmapImage image = null;

            using (var ms = source.ToMemoryStream())
            {
                image = new BitmapImage();
                ms.Position = 0;
                image.BeginInit();
                image.UriSource = null;
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
            }
            return image;
        }

        public static OpenCvSharp.Mat ReadZmapFormat(string path)
        {
            nXSDKNET.Format.ZMapFormat zmap = nXSDKNET.Format.ZMapFormat.Open(path);

            int width = zmap.GetImageWidth();
            int height = zmap.GetImageHeight();

            OpenCvSharp.Mat image = new OpenCvSharp.Mat(width, height, OpenCvSharp.MatType.CV_32FC1, zmap.GetZmap());
            zmap = null;
            return image;
        }

        public static OpenCvSharp.Mat ReadZmapFormatRGB(string path)
        {
            nXSDKNET.Format.ZMapFormat zmap = nXSDKNET.Format.ZMapFormat.Open(path);

            int width = zmap.GetImageWidth();
            int height = zmap.GetImageHeight();
            
            List<Tuple<byte, byte, byte>> pixels = zmap.ConvertRGB();
            OpenCvSharp.Mat image = null;
            if (pixels.Count != 0)
            {
                image = new OpenCvSharp.Mat(width, height, OpenCvSharp.MatType.CV_8UC3, OpenCvSharp.Scalar.Black);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * width + x;
                        Tuple<byte, byte, byte> pixel = pixels[index];
                        OpenCvSharp.Vec3b vec3b = new OpenCvSharp.Vec3b()
                        {
                            Item0 = pixel.Item3,
                            Item1 = pixel.Item2,
                            Item2 = pixel.Item1,
                        };
                        image.Set<OpenCvSharp.Vec3b>(y, x, vec3b);
                    }
                }
            }

            zmap = null;
            return image;

        }
    }
}
