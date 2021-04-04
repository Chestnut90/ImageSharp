using Models.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models.OpenCV.Algorithms
{
    public class KMean
    {
        #region Mats
        /// <summary>
        /// source as origin image
        /// </summary>
        public OpenCvSharp.Mat Source
        {
            get;
            protected set;
        } = null;

        /// <summary>
        /// Keman labels
        /// </summary>
        public OpenCvSharp.Mat Labels
        {
            get;
            protected set;
        } = null;

        /// <summary>
        /// Center points
        /// </summary>
        public OpenCvSharp.Mat Centers
        {
            get;
            protected set;
        } = null;

        #endregion

        /// <summary>
        /// K mean value
        /// </summary>
        public int K
        {
            get;
            protected set;
        } = 3;

        /// <summary>
        /// Is Kmean computed
        /// </summary>
        public bool IsComputed
        {
            get;
            protected set;
        } = false;

        /// <summary>
        /// Cluster points.
        /// </summary>
        public Dictionary<OpenCvSharp.Vec3b, List<Point<int>>> Clusters
        {
            get;
            protected set;
        } = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="k"></param>
        public KMean(OpenCvSharp.Mat source, int k = 3)
        {

            // copy?
            this.Source = source;
            this.K = k;
            this.IsComputed = false;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~KMean()
        {
            this.Source = null;
            this.Labels = null;
            this.Centers = null;
        }

        //@ TODO : other pixel type
        /// <summary>
        /// vectorizing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private OpenCvSharp.Mat Vecotrize(OpenCvSharp.Mat input = null)
        {
            if (input == null)
            {
                input = this.Source;
            }

            int width = input.Cols;
            int height = input.Rows;

            // generate points
            OpenCvSharp.Mat points = new OpenCvSharp.Mat();
            points.Create(width * height, 1, OpenCvSharp.MatType.CV_32FC3); // points vector

            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 3-tuples (float, float, float)
                    points.Set<OpenCvSharp.Vec3f>(i++, new OpenCvSharp.Vec3f()
                    {
                        Item0 = input.At<OpenCvSharp.Vec3b>(y, x).Item0,
                        Item1 = input.At<OpenCvSharp.Vec3b>(y, x).Item1,
                        Item2 = input.At<OpenCvSharp.Vec3b>(y, x).Item2,
                    });
                }
            }

            return points;
        }

        /// <summary>
        /// Compute K Mean
        /// </summary>
        /// <param name="input"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool ComputeKMeans()
        {
            int width = this.Source.Cols;
            int height = this.Source.Rows;

            OpenCvSharp.Mat points = Vecotrize(this.Source);
            Labels = new OpenCvSharp.Mat(); // targets
            Centers = new OpenCvSharp.Mat();

            try
            {
                IsComputed = false;

                Centers.Create(this.K, 1, OpenCvSharp.MatType.CV_32FC3);

                //  TermCriteria
                //  Type = OpenCvSharp.CriteriaType.Eps | OpenCvSharp.CriteriaType.MaxIter,
                //  MaxCount = 10,
                //  Epsilon = 1.0,
                OpenCvSharp.Cv2.Kmeans(points, this.K, Labels, new OpenCvSharp.TermCriteria(OpenCvSharp.CriteriaType.Eps | OpenCvSharp.CriteriaType.MaxIter, 10, 1.0)
                , 3, OpenCvSharp.KMeansFlags.PpCenters, Centers);
                IsComputed = true;
            }
            catch (Exception e)
            {
                IsComputed = false;
                throw e;
            }

            return IsComputed;
        }

        /// <summary>
        /// KMean labels to Mat
        /// </summary>
        /// <returns></returns>
        public OpenCvSharp.Mat KMeansToImage()
        {
            int width = Source.Cols;
            int height = Source.Rows;

            if (!IsComputed)
            {
                return null;
            }

            this.Clusters = new Dictionary<OpenCvSharp.Vec3b, List<Point<int>>>();
            OpenCvSharp.Mat result = new OpenCvSharp.Mat(Source.Size(), Source.Type());
            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = Labels.Get<int>(i++);

                    OpenCvSharp.Vec3b vec3b = new OpenCvSharp.Vec3b();

                    int tmp = Convert.ToInt32(Math.Round(Centers.At<OpenCvSharp.Vec3f>(idx).Item0));
                    tmp = tmp > 255 ? 255 : tmp < 0 ? 0 : tmp;
                    vec3b.Item0 = Convert.ToByte(tmp);

                    tmp = Convert.ToInt32(Math.Round(Centers.At<OpenCvSharp.Vec3f>(idx).Item1));
                    tmp = tmp > 255 ? 255 : tmp < 0 ? 0 : tmp;
                    vec3b.Item1 = Convert.ToByte(tmp);

                    tmp = Convert.ToInt32(Math.Round(Centers.At<OpenCvSharp.Vec3f>(idx).Item2));
                    tmp = tmp > 255 ? 255 : tmp < 0 ? 0 : tmp;
                    vec3b.Item2 = Convert.ToByte(tmp);

                    result.Set<OpenCvSharp.Vec3b>(y, x, vec3b);

                    if (!this.Clusters.ContainsKey(vec3b))
                    {
                        List<Point<int>> list = new List<Point<int>>();
                        this.Clusters.Add(vec3b, list);
                    }
                    this.Clusters[vec3b].Add(new Point<int>(x, y));
                }
            }

            return result;
        }

        /// <summary>
        /// Kmean labels to cluster
        /// </summary>
        public void KMeansToCluster()
        {


        }

    }
}
