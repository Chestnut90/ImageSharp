using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Formats
{
    public class Point<T>
    {
        public T X { get; set; }
        public T Y { get; set; }

        public Point()
        {
            this.X = default(T);
            this.Y = default(T);
        }

        public Point(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }

    }
}
