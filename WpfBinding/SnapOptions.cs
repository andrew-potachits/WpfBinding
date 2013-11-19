using System;

namespace WpfBinding
{
    public class SnapOptions
    {
        private double _horizontalGridStep;
        private double _verticalGridStep;
        public bool VerticalSnap { get; set; }
        public double VerticalGridStep
        {
            get { return _verticalGridStep; }
            set
            {
                if (Math.Abs(value - 0) < double.Epsilon)
                    throw new ArgumentException("step must be non-zero", "value");
                _verticalGridStep = value;
            }
        }

        public bool HorizontalSnap { get; set; }
        public double HorizontalGridStep
        {
            get { return _horizontalGridStep; }
            set
            {
                if (Math.Abs(value - 0) < double.Epsilon)
                    throw new ArgumentException("step must be non-zero", "value");
                    
                _horizontalGridStep = value;
            }
        }
    }
}