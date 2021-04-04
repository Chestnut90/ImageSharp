using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models.IPModels
{

    public class ImageLoader
    {
        public enum ZmapType : uint
        {
            FLOAT = 0,
            RGB = 1,
        }

        public enum ImageType : uint
        {
            NORMAL = 0,
            SURF = 1,
        }

        public ImageLoader()
        {

        }

        public BitmapImage LoadSurfImage(string path, ZmapType zmapType)
        {
            BitmapImage image = null;
            OpenCvSharp.Mat mat = null;
            try
            {
                switch (zmapType)
                {
                    case ZmapType.FLOAT:
                        mat = OpenCV.CvInterface.ReadZmapFormat(path);
                        image = OpenCV.CvInterface.MatToBitmapImage(mat);
                        break;
                    case ZmapType.RGB:
                        mat = OpenCV.CvInterface.ReadZmapFormatRGB(path);
                        image = OpenCV.CvInterface.MatToBitmapImage(mat);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                // @ TODO
                image = null;
            }

            return image;
        }

        public WriteableBitmap LoadImage(string path)
        {
            OpenCvSharp.Mat mat = OpenCvSharp.Cv2.ImRead(path);
            return OpenCV.CvInterface.MatToWriteableBitmap(mat);
        }
        
    }
}
