namespace WpfBinding.Geomerty
{
    /// <summary>
    /// Provides the X and Y scale values
    /// </summary>
    public interface IScaleTransformer
    {
        /// <summary>
        /// Gets or sets the factor for scaling.
        /// </summary>
        /// <value>
        /// The scale factor.
        /// </value>
        double Scale { get; set; }
        /// <summary>
        /// Gets or sets the shift for movement.
        /// </summary>
        /// <value>
        /// The shift value.
        /// </value>
        double Origin { get; set; }
        /// <summary>
        /// Transforms the specified value using <see cref="Scale"/> and <see cref="Origin"/>.
        /// </summary>
        /// <param name="value">The value to transform (scale).</param>
        /// <returns>Value shifter by <see cref="Origin"/> and scaled by <see cref="Scale"/> factor.</returns>
        double Transform(double value);
        /// <summary>
        /// Runs transformation opposite to what <see cref="Transform"/> method does.
        /// </summary>
        /// <param name="value">The value to untransform.</param>
        /// <returns>The result of opposite transformation</returns>
        double Unstransform(double value);
    }
}