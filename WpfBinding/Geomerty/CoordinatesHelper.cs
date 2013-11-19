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
        private readonly Transform _transform;
        private readonly ScaleTransform _scale;
        private readonly TranslateTransform _move;

        public CoordinatesHelper()
        {
            _horizontalTransformer = new ScaleTransformer(0, 1);
            _verticalTransformer = new ScaleTransformer(0, 1);
            _scale = new ScaleTransform();
            _move = new TranslateTransform();
            var group = new TransformGroup();
            
            group.Children.Add(_move);
            group.Children.Add(_scale);

            _transform = group;
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
            _verticalTransformer.ScaleValue = _horizontalTransformer.ScaleValue = Math.Min(scaleX, scaleY);

            _scale.ScaleX = _horizontalTransformer.ScaleValue;
            _scale.ScaleY = _verticalTransformer.ScaleValue;
            _scale.CenterX = drawingBounds.Left;
            _scale.CenterY = drawingBounds.Top;

            if (scaleX > scaleY)
            {
                _horizontalTransformer.Origin = _move.X = (viewBounds.Width - (drawingBounds.Width * _verticalTransformer.ScaleValue)) / 2;
                _verticalTransformer.Origin = _move.Y = 0;
            }
            else
            {
                _horizontalTransformer.Origin = _move.X = 0;
                _verticalTransformer.Origin = _move.Y = (viewBounds.Height - (drawingBounds.Height *_verticalTransformer.ScaleValue)) / 2;
            }
        }

        public Vector Unscale(Vector vector)
        {
            return new Vector(_horizontalTransformer.Unscale(vector.X),
                              _verticalTransformer.Unscale(vector.Y));
        }

        public Vector Scale(Vector vector)
        {
            return new Vector(_horizontalTransformer.Scale(vector.X),
                              _verticalTransformer.Scale(vector.Y));
        }
    }
}
