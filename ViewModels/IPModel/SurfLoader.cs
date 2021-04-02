using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ViewModels.IPModel
{
    public enum ZmapType : uint
    {
        FLOAT = 0,
        RGB = 1,
    }


    public class SurfLoader
    {
        public static SurfLoader OpenSurf(string path)
        {
            SurfLoader loader = new SurfLoader();
            loader.ZmapFloat = ImageProcessing.Interface.CvInterface.ReadZmapFormat(path);
            loader.ZmapRGB = ImageProcessing.Interface.CvInterface.ReadZmapFormatRGB(path);
            return loader;
        }

        public SurfLoader()
        {

        }

        public OpenCvSharp.Mat ZmapFloat { get; private set; }

        public OpenCvSharp.Mat ZmapRGB { get; private set; }

        public BitmapImage LoadSurfImage(string path, ZmapType zmapType)
        {
            BitmapImage image = null;
            try
            {
                this.ZmapFloat = ImageProcessing.Interface.CvInterface.ReadZmapFormat(path);
                this.ZmapRGB = ImageProcessing.Interface.CvInterface.ReadZmapFormatRGB(path);

                switch (zmapType)
                {
                    case ZmapType.FLOAT:
                        image = ImageProcessing.Interface.CvInterface.MatToBitmapImage(ZmapFloat);
                        break;
                    case ZmapType.RGB:
                        image = ImageProcessing.Interface.CvInterface.MatToBitmapImage(ZmapRGB);
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

        public BitmapImage GetZmapFloat()
        {
            return ImageProcessing.Interface.CvInterface.MatToBitmapImage(ZmapFloat);
        }

        public BitmapImage GetZmapRGB()
        {
            return ImageProcessing.Interface.CvInterface.MatToBitmapImage(ZmapRGB);
        }

    }
}
