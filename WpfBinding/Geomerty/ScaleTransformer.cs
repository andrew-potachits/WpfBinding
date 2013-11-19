namespace WpfBinding.Geomerty
{
    public class ScaleTransformer : IScaleTransformer
    {
        public ScaleTransformer(double origin, double scale)
        {
            Origin = origin;
            ScaleValue = scale;
        }

        public double ScaleValue { get; set; }
        public double Origin { get; set; }
        public double Transform(double value)
        {
            return Origin + Scale(value);
        }

        public double Untransform(double value)
        {
            return Unscale(value - Origin);
        }

        public double Unscale(double value)
        {
            return value/ScaleValue;
        }

        public double Scale(double value)
        {
            return value*ScaleValue;
        }
    }
}