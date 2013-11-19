using System.Windows;
using NUnit.Framework;
using WpfBinding;

namespace Geometry.Tests
{
    [TestFixture]
    public class DraggingTests
    {
        readonly ElementOptions _options = new ElementOptions
        {
            DragType = DragTypes.Both,
            SnapOptions = new SnapOptions
            {
                HorizontalSnap = true,
                HorizontalGridStep = 3.0,
                VerticalSnap = true,
                VerticalGridStep = 2.0
            }
        };
        [Test]
        public void TestSnapToGrid()
        {
            var options = new SnapOptions
                                      {
                                          HorizontalSnap = true,
                                          HorizontalGridStep = 1.0,
                                          VerticalSnap = true,
                                          VerticalGridStep = 1.0
                                      };
            var zeroVector = DraggingHelper.SnapToGrid(new Vector(0, 0), options);

            Assert.AreEqual(0, zeroVector.X);
            Assert.AreEqual(0, zeroVector.Y);

            var small = DraggingHelper.SnapToGrid(new Vector(0.4, -0.49), options);

            Assert.AreEqual(0, small.X);
            Assert.AreEqual(0, small.Y);

            var horizontal = DraggingHelper.SnapToGrid(new Vector(0.5, -0.2), options);

            Assert.AreEqual(1.0, horizontal.X);
            Assert.AreEqual(0, horizontal.Y);

            var vertical = DraggingHelper.SnapToGrid(new Vector(1.5, -3.2), options);

            Assert.AreEqual(2.0, vertical.X);
            Assert.AreEqual(-3.0, vertical.Y);
        }

        [Test]
        public void DragHorizontal()
        {
            var zero = DraggingHelper.CalculateNewSnapPosition(new Vector(0, 0), _options);
            Assert.AreEqual(0.0, zero.X);

            var more = DraggingHelper.CalculateNewSnapPosition(new Vector(2.0, 0), _options);
            Assert.AreEqual(_options.SnapOptions.HorizontalGridStep, more.X);

            more = DraggingHelper.CalculateNewSnapPosition(new Vector(3.5, 0), _options);
            Assert.AreEqual(_options.SnapOptions.HorizontalGridStep, more.X);

            more = DraggingHelper.CalculateNewSnapPosition(new Vector(5.5, 0), _options);
            Assert.AreEqual(_options.SnapOptions.HorizontalGridStep*2, more.X);
        }

        [Test]
        public void DragVertical()
        {
            var zero = DraggingHelper.CalculateNewSnapPosition(new Vector(0, 0), _options);
            Assert.AreEqual(0.0, zero.Y);

            var more = DraggingHelper.CalculateNewSnapPosition(new Vector(2.0, 0.2), _options);
            Assert.AreEqual(0.0, more.Y);

            more = DraggingHelper.CalculateNewSnapPosition(new Vector(0, 1.7), _options);
            Assert.AreEqual(_options.SnapOptions.VerticalGridStep, more.Y);

            more = DraggingHelper.CalculateNewSnapPosition(new Vector(0, 2.1), _options);
            Assert.AreEqual(_options.SnapOptions.VerticalGridStep, more.Y);

            more = DraggingHelper.CalculateNewSnapPosition(new Vector(0, 3.1), _options);
            Assert.AreEqual(_options.SnapOptions.VerticalGridStep*2, more.Y);
        }

    }
}
