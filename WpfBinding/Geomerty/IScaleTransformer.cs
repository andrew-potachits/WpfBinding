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
        double ScaleValue { get; set; }
        /// <summary>
        /// Gets or sets the shift for movement.
        /// </summary>
        /// <value>
        /// The shift value.
        /// </value>
        double Origin { get; set; }
        /// <summary>
        /// Transforms the specified value using <see cref="ScaleValue"/> and <see cref="Origin"/>.
        /// </summary>
        /// <param name="value">The value to transform (scale).</param>
        /// <returns>Value shifter by <see cref="Origin"/> and scaled by <see cref="ScaleValue"/> factor.</returns>
        double Transform(double value);
        /// <summary>
        /// Runs transformation opposite to what <see cref="Transform"/> method does.
        /// </summary>
        /// <param name="value">The value to untransform.</param>
        /// <returns>The result of opposite transformation</returns>
        double Untransform(double value);

        /// <summary>
        /// Unscales the specified value (restores original value from scaled one).
        /// </summary>
        /// <param name="value">The scaled value.</param>
        /// <returns>Original value</returns>
        double Unscale(double value);
        /// <summary>
        /// Scales the specified value (applies scale to the value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Scaled value</returns>
        double Scale(double value);
    }
}