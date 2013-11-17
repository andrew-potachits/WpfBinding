using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfBinding.Geomerty;

namespace WpfBinding
{
    /// <summary>
    /// Interaction logic for Preview2D.xaml
    /// </summary>
    //[ContentProperty("Data")]
    public partial class Preview2D : UserControl
    {
        public IEnumerable<LineDef> Data
        {
            get { return (IEnumerable<LineDef>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(IEnumerable<LineDef>), typeof(Preview2D), new PropertyMetadata(default(IEnumerable<LineDef>), OnPropertyChanged));

        private Point _oldPosition;
        private readonly CoordinatesHelper _coordinatesHelper = new CoordinatesHelper();

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as Preview2D;
            if (target != null) target.UpdateContent();
        }

        private void UpdateContent()
        {
            var collectionChanged = Data as INotifyCollectionChanged;
            if (collectionChanged != null) 
                collectionChanged.CollectionChanged += ItemsChanged;
            
            CalculateScalingFactor();
            PopulateChildren();
        }

        private void PopulateChildren()
        {
            Canvas.Children.Clear();
            foreach (var lineDef in Data)
            {
                var line = new Line
                               {
                                   StrokeThickness = 2,
                                   DataContext = lineDef,
                                   Stroke = Brushes.Black,
                                   LayoutTransform = _coordinatesHelper.Transform
                               };
                BindData(line, Line.X1Property, "From.X", _coordinatesHelper.HorizontalConverter);
                BindData(line, Line.Y1Property, "From.Y", _coordinatesHelper.VerticalConverter);
                BindData(line, Line.X2Property, "To.X", _coordinatesHelper.HorizontalConverter);
                BindData(line, Line.Y2Property, "To.Y", _coordinatesHelper.VerticalConverter);

                Canvas.Children.Add(line);
            }
        }

        private void BindData(DependencyObject target, DependencyProperty property, string path, IValueConverter valueConverter)
        {
            var binding = new Binding(path)
                              {
                                  Mode = BindingMode.TwoWay, 
                                  //Converter = valueConverter,
                              };
            BindingOperations.SetBinding(target, property, binding);
        }

        private void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PopulateChildren();
        }


        public Preview2D()
        {
            InitializeComponent();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured != null)
            {
                HandleMoving(e.GetPosition(Canvas));
            }
            else
            {
                HandleCapturing(e.GetPosition(Canvas));
            }

        }

        private void HandleCapturing(Point eventPosition)
        {
            var hitTestResult = VisualTreeHelper.HitTest(Canvas, eventPosition);
            if (hitTestResult == null)
                return;
            Mouse.OverrideCursor = GetProperCursor(hitTestResult.VisualHit is Line);
            _oldPosition = eventPosition;
        }

        private Cursor GetProperCursor(bool resizeCursor)
        {
            return resizeCursor ? Cursors.SizeWE : Cursors.Arrow;
        }

        private void HandleMoving(Point position)
        {
            var vector = position - _oldPosition;
            vector.Y = 0;

            UIElement line = (UIElement) Mouse.Captured;
            var source = line.GetValue(Line.DataContextProperty) as LineDef;
            source.From += vector;
            source.To += vector;

            _oldPosition = position;
        }

        private void Highlight(DependencyObject selectedObject, bool highlight)
        {
            ((Line) selectedObject).StrokeThickness += highlight ? 1 : -1;
            selectedObject.SetValue(Line.StrokeProperty, highlight? Brushes.Yellow : Brushes.Black);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var hitTestResult = VisualTreeHelper.HitTest(Canvas, e.GetPosition(Canvas));
            if (hitTestResult == null || hitTestResult.VisualHit == null)
                return;

            if (hitTestResult.VisualHit is Line)
            {
                Mouse.Capture((IInputElement) hitTestResult.VisualHit, CaptureMode.Element);
                ((LineDef) hitTestResult.VisualHit.GetValue(Line.DataContextProperty)).Selected = true;
            }
            else
            {
                foreach (var selectedLine in Data.AsEnumerable().Where(l => l.Selected))
                {
                    selectedLine.Selected = false;
                }
                
            }
            
            
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured == null)
                return;
            Mouse.Capture(null);
        }

        public double XScale { get; set; }
        public double YScale { get; set; }
        public double XOrigin { get; set; }
        public double YOrigin { get; set; }

        private void CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalculateScalingFactor();
        }

        private void CalculateScalingFactor()
        {
            if (Data == null)
                return;

            var bounds = new Rect(0,0,0,0);
            foreach (var lineDef in Data)
            {
                bounds.Union(new Rect(lineDef.From, lineDef.To));
            }
            Rect viewBounds = new Rect(0, 0, ActualWidth, ActualHeight);

            _coordinatesHelper.RecalculateScale(viewBounds, bounds);
        }
    }
}
