namespace WpfBinding
{
    /// <summary>
    /// Provides the X and Y scale values
    /// </summary>
    public interface IScaleProvider
    {
        /// <summary>
        /// Gets or sets the x scale.
        /// </summary>
        /// <value>
        /// The x scale.
        /// </value>
        double XScale { get; set; }
        /// <summary>
        /// Gets or sets the y scale.
        /// </summary>
        /// <value>
        /// The y scale.
        /// </value>
        double YScale { get; set; }
    }
}