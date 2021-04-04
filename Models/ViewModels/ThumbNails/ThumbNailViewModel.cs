using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models.ViewModels.ThumbNails
{
    public class ThumbNailViewModel
    {
        public ThumbNailViewModel()
        {
            Image = null;
            Title = String.Empty;
            Description = String.Empty;
        }

        public ThumbNailViewModel(BitmapSource image, String title)
        {
            Image = image;
            Title = title;
        }

        public BitmapSource Image { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

    }
}
