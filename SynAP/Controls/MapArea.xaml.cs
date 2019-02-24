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
        }

        public void UpdateCanvas()
        {
            try
            {
                double scaleX = this.ActualWidth / BackgroundArea.Width;
                double scaleY = this.ActualHeight / BackgroundArea.Height;
                double scale = scaleX;
                if (scaleX > scaleY)
                    scale = scaleY;

                double width = ForegroundArea.Width;
                double height = ForegroundArea.Height;
                Point position = ForegroundArea.Position;

                Point centerOffset = new Point
                {
                    X = this.ActualWidth / 2.0 - BackgroundArea.Width * scale / 2.0,
                    Y = this.ActualHeight / 2.0 - BackgroundArea.Height * scale / 2.0,
                };

                background.Width = BackgroundArea.Width * scale;
                background.Height = BackgroundArea.Height * scale;
                MoveObject(background, centerOffset);

                foreground.Width = ForegroundArea.Width * scale;
                foreground.Height = ForegroundArea.Height * scale;
                MoveObject(foreground, new Point(centerOffset.X + ForegroundArea.Position.X * scale, centerOffset.Y + ForegroundArea.Position.Y * scale));

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

        public Task LazyAreaUpdate(Area foreground)
        {
            ForegroundArea = foreground;
            return Task.CompletedTask;
        }
    }
}
