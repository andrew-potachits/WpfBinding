using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
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
    public partial class Preview2D
    {
        public IEnumerable<LineBase> Data
        {
            get { return (IEnumerable<LineBase>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(IEnumerable<LineBase>), typeof(Preview2D), new PropertyMetadata(default(IEnumerable<LineBase>), OnPropertyChanged));

        private Point _oldPosition;
        private readonly CoordinatesHelper _coordinatesHelper = new CoordinatesHelper();


        public Style LineStyle
        {
            get { return (Style)GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register("LineStyle", typeof(Style), typeof(Preview2D), new PropertyMetadata(null));




        public Style SelectedLineStyle
        {
            get { return (Style)GetValue(SelectedLineStyleProperty); }
            set { SetValue(SelectedLineStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLineStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLineStyleProperty =
            DependencyProperty.Register("SelectedLineStyle", typeof(Style), typeof(Preview2D), new PropertyMetadata(null));

        private IElementOptionsProvider _elementOptionsProvider = new DemoElementOptionsProvider();
        private Vector _cumulativeDragging;
        private Vector _snapToGridNode;


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
            if (Data == null)
                return;

            foreach (var lineDef in Data)
            {
                var line = new Line
                               {
                                   DataContext = lineDef,
                                   Style = LineStyle,
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
                                  Converter = valueConverter,
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

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured != null)
            {
                HandleMoving(e.GetPosition(Canvas));
            }
            else
            {
                HandleHovering(e.GetPosition(Canvas));
            }

        }

        private void HandleHovering(Point eventPosition)
        {
            var element = HitElement(eventPosition);
            if (element == null)
                return;
            
            Mouse.OverrideCursor = GetProperCursor(element as Line);
        }

        private Cursor GetProperCursor(Line element)
        {
            if (element != null)
            {
                var line = element.DataContext as LineBase;
                if (line != null)
                {
                    switch (_elementOptionsProvider.GetElementOptions(line).DragType)
                    {
                        case DragTypes.Horizontal:
                            return Cursors.SizeWE;
                        case DragTypes.Vertical:
                            return Cursors.SizeNS;
                        case DragTypes.Both:
                            return Cursors.SizeAll;
                    }
                }
            }

            return Cursors.Arrow;
        }

        private void HandleMoving(Point position)
        {
            var line = (UIElement)Mouse.Captured;
            var source = line.GetValue(Line.DataContextProperty) as LineBase;
            if (source == null)
                return;

            //TODO: move this to separate class for handling drag vector calculation
            //  current movement (step)
            var movement = position - _oldPosition;
            //  adjusted gradding vector
            var dragging = AdjustVector(movement, source);

            if (dragging != _snapToGridNode)
            {
                var vector = dragging - _snapToGridNode;
                source.From += vector;
                source.To += vector;
                _snapToGridNode = dragging;
            }
            _oldPosition = position;
            _cumulativeDragging += movement;
        }

        private Vector AdjustVector(Vector vector, LineBase element)
        {
            ElementOptions options = _elementOptionsProvider.GetElementOptions(element);

            var totalMove = _coordinatesHelper.Unscale(_cumulativeDragging) + _coordinatesHelper.Unscale(vector);

            //  snap to nearest grid line
            return DraggingHelper.CalculateNewSnapPosition(totalMove, options);
        }


        private void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(Canvas);
            var element = HitElement(position);
            if (element == null) 
                return;

            // BUG: HitTest may return elements that have IsHitTestVisible = false
            if (element.IsHitTestVisible)
            {
                BeginDragging(position, element, e);
            }
            else
            {
                UnselectElements();
            }
            
            
        }

        private UIElement HitElement(Point position)
        {
            var hitTestResult = VisualTreeHelper.HitTest(Canvas, position);
            if (hitTestResult == null || hitTestResult.VisualHit == null)
                return null;

            var element = (UIElement) hitTestResult.VisualHit;
            return element;
        }

        private void BeginDragging(Point position, UIElement element, MouseButtonEventArgs e)
        {
            Mouse.Capture(element, CaptureMode.Element);
            SelectElement(element, e.LeftButton == MouseButtonState.Pressed);
            _oldPosition = position;
            // reset distance
            _cumulativeDragging = new Vector(0, 0);
            _snapToGridNode = new Vector(0, 0);
        }

        private void UnselectElements()
        {
            foreach (var selectedLine in Data.AsEnumerable().Where(l => l.Selected))
            {
                selectedLine.Selected = false;
            }
        }

        private void SelectElement(UIElement element, bool addSelection)
        {
            if (!addSelection)
                UnselectElements();

            var line = element.GetValue(Line.DataContextProperty) as LineBase;
            if (line != null)
            {
                line.Selected = true;
            }
        }

        private void CanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured == null)
                return;
            Mouse.Capture(null);
            _snapToGridNode = _cumulativeDragging = new Vector(0, 0);
            
        }

        private void CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalculateScalingFactor();
            // force updating the content
            PopulateChildren();
        }

        private void CalculateScalingFactor()
        {
            if (Data == null)
                return;

            var bounds = new Rect();
            foreach (var lineDef in Data)
            {
                bounds.Union(new Rect(lineDef.From, lineDef.To));
            }
            //TODO: inrtoduce property for bounds 
            Rect viewBounds = new Rect(0, 0, Canvas.ActualWidth, Canvas.ActualHeight);


            _coordinatesHelper.RecalculateScale(viewBounds, bounds);
        }
    }
}
