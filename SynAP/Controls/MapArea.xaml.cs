using SynAP.Controls.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SynAP.Controls
{
    /// <summary>
    /// Interaction logic for MapArea.xaml
    /// </summary>
    public partial class MapArea : UserControl
    {
        public MapArea()
        {
            InitializeComponent();
            Output?.Invoke(this, "Initializing...");
            CreateCanvasObjects();
            Output?.Invoke(this, "Ready.");
        }

        #region Variables and Events

        public event EventHandler<string> Output;

        [Bindable(true), Category("Common")]
        public Area ForegroundArea
        {
            set
            {
                SetValue(ForegroundAreaProperty, value);
                UpdateCanvas();
            }
            get => (Area)GetValue(ForegroundAreaProperty);
        }

        [Bindable(true), Category("Common")]
        public Area BackgroundArea
        {
            set
            {
                SetValue(BackgroundAreaProperty, value);
                UpdateCanvas();
            }
            get => (Area)GetValue(BackgroundAreaProperty);
        }

        private Rectangle foreground;
        private Rectangle background;

        private AreaDrag AreaDrag;

        #endregion

        #region DependencyProperties

        public static readonly DependencyProperty ForegroundAreaProperty = DependencyProperty.Register(
            "ForegroundArea", typeof(Area), typeof(MapArea));

        public static readonly DependencyProperty BackgroundAreaProperty = DependencyProperty.Register(
            "BackgroundArea", typeof(Area), typeof(MapArea));

        #endregion

        private void CreateCanvasObjects()
        {
            AreaCanvas.Children.Clear();

            foreground = new Rectangle
            {
                Fill = SystemParameters.WindowGlassBrush ?? Brushes.SkyBlue,
            };

            AreaCanvas.Children.Add(foreground);

            background = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1.0,
                Fill = Brushes.Transparent
            };

            AreaCanvas.Children.Add(background);

            Output?.Invoke(this, "Canvas objects created.");

            AreaCanvas.MouseDown += AreaMouseDown;
            AreaCanvas.MouseMove += AreaMouseMove;
        }

        private void AreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            AreaDrag = new AreaDrag(foreground, e.GetPosition(AreaCanvas));
            AreaDrag.MouseUp += AreaDrag_MouseUp;
        }

        private void AreaDrag_MouseUp(object sender, System.Windows.Point e)
        {
            if (AreaDrag != null)
            {
                ForegroundArea.Position.X += Math.Round(e.X / CanvasScale.Value);
                ForegroundArea.Position.Y += Math.Round(e.Y / CanvasScale.Value);
            }
        }

        private void AreaMouseMove(object sender, MouseEventArgs e)
        {
            if (AreaDrag != null && AreaDrag.IsDragging)
                AreaDrag.TranslatePosition = e.GetPosition(AreaCanvas);
        }

        public void UpdateCanvas()
        {
            try
            {
                double width = ForegroundArea.Width;
                double height = ForegroundArea.Height;
                Point position = ForegroundArea.Position;

                Point centerOffset = new Point
                {
                    X = this.ActualWidth / 2.0 - BackgroundArea.Width * CanvasScale.Value / 2.0,
                    Y = this.ActualHeight / 2.0 - BackgroundArea.Height * CanvasScale.Value / 2.0,
                };

                background.Width = BackgroundArea.Width * CanvasScale.Value;
                background.Height = BackgroundArea.Height * CanvasScale.Value;
                MoveObject(background, centerOffset);

                foreground.Width = ForegroundArea.Width * CanvasScale.Value;
                foreground.Height = ForegroundArea.Height * CanvasScale.Value;
                MoveObject(foreground, new Point(centerOffset.X + ForegroundArea.Position.X * CanvasScale.Value, centerOffset.Y + ForegroundArea.Position.Y * CanvasScale.Value));

                Output?.Invoke(this, "Canvas updated.");
            }
            catch
            {
                Output?.Invoke(this, "Invalid settings.");
            }
        }

        public Task MoveObject(UIElement obj, Point position)
        {
            Canvas.SetLeft(obj, position.X);
            Canvas.SetTop(obj, position.Y);
            return Task.CompletedTask;
        }

        private ScaleTool CanvasScale => new ScaleTool(ActualWidth, ActualHeight, BackgroundArea.Width, BackgroundArea.Height);
    }
}
