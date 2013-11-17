using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfBinding.Geomerty
{
    public class CoordinatesHelper
    {
        private readonly IScaleTransformer _horizontalTransformer;
        private readonly IScaleTransformer _verticalTransformer;
        private readonly ScaleTransform _transform;

        public CoordinatesHelper()
        {
            _horizontalTransformer = new ScaleTransformer(0, 1);
            _verticalTransformer = new ScaleTransformer(0, 1);
            _transform = new ScaleTransform(_horizontalTransformer.Origin, _verticalTransformer.Origin,
                _horizontalTransformer.Scale, _verticalTransformer.Scale);
        }

        public IValueConverter HorizontalConverter
        {
            get { return new CoordinateConverter(_horizontalTransformer); }
        }

        public IValueConverter VerticalConverter
        {
            get { return new CoordinateConverter(_verticalTransformer); }
        }

        public Transform Transform
        {
            get { return _transform; }
        }

        public void RecalculateScale(Rect viewBounds, Rect drawingBounds)
        {
            double scaleX = viewBounds.Width/drawingBounds.Width;
            double scaleY = viewBounds.Height/drawingBounds.Height;
            _verticalTransformer.Scale =_horizontalTransformer.Scale = Math.Min(scaleX, scaleY);
            _transform.ScaleX = _transform.ScaleY = _horizontalTransformer.Scale;
        }
    }
}
