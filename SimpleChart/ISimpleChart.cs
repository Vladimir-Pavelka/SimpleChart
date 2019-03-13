namespace SimpleChart
{
    using System.Collections.Generic;

    public interface ISimpleChart
    {
        /// <summary>
        /// Gets or sets this window's Title
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets Y axis min value. Set it to double.NaN to make this property Auto. Default value is 0
        /// </summary>
        double MinValueAxisY { get; set; }

        /// <summary>
        /// Gets or sets amount of X axis points triggering chart rescale. Default value is 1000
        /// </summary>
        int PointCountRescaleAxisX { get; set; }

        /// <summary>
        /// Gets or sets whether an animation is played as new points are added to the chart. Default value is false
        /// </summary>
        bool Animate { get; set; }

        /// <summary>
        /// Sets the chart point series.
        /// Chart automatically rescales on the X axis, showing at most <see cref="PointCountRescaleAxisX"/> points
        /// </summary>
        void SetSeries(IReadOnlyCollection<double> points);

        /// <summary>
        /// Appends a single value point to the current chart series.
        /// Chart automatically rescales on the X axis, showing at most <see cref="PointCountRescaleAxisX"/> points
        /// </summary>
        void AddPoint(double value);

        /// <summary>
        /// Removes all value points from the chart
        /// </summary>
        /// <param name="forceRedraw"></param>
        void Clear(bool forceRedraw = true);

        /// <summary>
        /// Programatically closes the chart window
        /// </summary>
        void Close();
    }
}