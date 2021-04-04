using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Views.ImageViewers
{
    /// <summary>
    /// Zoom and Pan UI element
    /// </summary>
    public class UIZoomElement : Border
    {
        private Point origin;
        private Point start;

        private UIElement child = null;
        public override UIElement Child
        {
            get => base.Child;
            set
            {
                // new child
                if (value != null & value != this.Child)
                {
                    this.Initialize(value);
                }
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            this.child = element;
            if (element == null)
            {
                return;
            }

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform());      // add scale transform
            transformGroup.Children.Add(new TranslateTransform());  // add translate transform

            child.RenderTransform = transformGroup;
            child.RenderTransformOrigin = new Point(0.0, 0.0);

            // enroll events
            this.MouseWheel += Child_MouseWheel;
            this.MouseLeftButtonDown += Child_MouseLeftButtonDown;
            this.MouseLeftButtonUp += Child_MouseLeftButtonUp;
            this.MouseMove += Child_MouseMove;
            this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(
              Child_PreviewMouseRightButtonDown);
        }

        private TranslateTransform GetChildTranslateTransform()
        {
            if (this.child is null)
            {
                return null;
            }
            return (TranslateTransform)((TransformGroup)child.RenderTransform).Children.First(transform => transform is TranslateTransform);
        }

        private ScaleTransform GetChildScaleTransform()
        {
            if (this.child is null)
            {
                return null;
            }
            return (ScaleTransform)((TransformGroup)child.RenderTransform).Children.First(transform => transform is ScaleTransform);

        }

        public void Reset()
        {
            if (child is null)
            {
                return;
            }

            // reset zoom
            var scaleTransform = GetChildScaleTransform();
            scaleTransform.ScaleX = 1.0;
            scaleTransform.ScaleY = 1.0;

            // reset pan
            var translateTransform = GetChildTranslateTransform();
            translateTransform.X = 0.0;
            translateTransform.Y = 0.0;
        }

        #region Events

        private void Child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (child == null)
            {
                return;
            }

            // get transforms from child render.
            var scaleTransfrom = this.GetChildScaleTransform();
            var translateTransform = this.GetChildTranslateTransform();

            // delta check
            if (!(e.Delta > 0) && (scaleTransfrom.ScaleX < .4 || scaleTransfrom.ScaleY < .4))
                return;

            double zoom = e.Delta > 0 ? 0.2 : -0.2;

            Point relative = e.GetPosition(child);
            double abosuluteX;
            double abosuluteY;

            abosuluteX = relative.X * scaleTransfrom.ScaleX + translateTransform.X;
            abosuluteY = relative.Y * scaleTransfrom.ScaleY + translateTransform.Y;

            scaleTransfrom.ScaleX += zoom;
            scaleTransfrom.ScaleY += zoom;

            translateTransform.X = abosuluteX - relative.X * scaleTransfrom.ScaleX;
            translateTransform.Y = abosuluteY - relative.Y * scaleTransfrom.ScaleY;
        }

        private void Child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child is null)
            {
                return;
            }

            child.ReleaseMouseCapture();
            this.Cursor = Cursors.Arrow;
        }

        private void Child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child is null)
            {
                return;
            }

            var translateTransform = GetChildTranslateTransform();
            start = e.GetPosition(this);
            origin = new Point(translateTransform.X, translateTransform.Y);
            this.Cursor = Cursors.Hand;
            child.CaptureMouse();
        }

        void Child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Reset();
        }

        private void Child_MouseMove(object sender, MouseEventArgs e)
        {
            if (child is null)
            {
                return;
            }

            if (child.IsMouseCaptured)
            {
                var translateTransform = GetChildTranslateTransform();
                Vector v = start - e.GetPosition(this);
                translateTransform.X = origin.X - v.X;
                translateTransform.Y = origin.Y - v.Y;
            }
        }
        #endregion

    }
}
