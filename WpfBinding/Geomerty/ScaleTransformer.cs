namespace WpfBinding.Geomerty
{
    public class ScaleTransformer : IScaleTransformer
    {
        public ScaleTransformer(double origin, double scale)
        {
            Origin = origin;
            Scale = scale;
        }

        public double Scale { get; set; }
        public double Origin { get; set; }
        public double Transform(double value)
        {
            return Origin + value*Scale;
        }

        public double Untransform(double value)
        {
            return Unscale(value - Origin);
        }

        public double Unscale(double value)
        {
            return value/Scale;
        }
    }
}