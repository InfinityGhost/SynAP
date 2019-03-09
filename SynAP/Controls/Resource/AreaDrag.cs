using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SynAP.Controls.Resource
{
    class AreaDrag
    {
        public AreaDrag(UIElement areaVisual, System.Windows.Point point)
        {
            IsDragging = true;
            SourcePoint = point;
            Element = areaVisual;

            Element.CaptureMouse();
            Element.MouseUp += Element_MouseUp;

            Element.RenderTransform = new TranslateTransform();
        }

        private void Element_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Element.ReleaseMouseCapture();
            IsDragging = false;
            MouseUp?.Invoke(this, TranslatePosition);
            Element.RenderTransform = new TranslateTransform();
        }

        public event EventHandler<System.Windows.Point> MouseUp;

        public bool IsDragging { private set; get; }
        public UIElement Element;

        public System.Windows.Point SourcePoint;
        public System.Windows.Point TranslatePosition
        {
            set
            {
                var transform = Element.RenderTransform as TranslateTransform;
                transform.X = value.X - SourcePoint.X;
                transform.Y = value.Y - SourcePoint.Y;
            }
            get
            {
                var transform = Element.RenderTransform as TranslateTransform;
                return new System.Windows.Point(transform.X, transform.Y);
            }
        }
    }
}
