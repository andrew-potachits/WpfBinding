using System;
using System.Windows;

namespace WpfBinding
{
    public static class DraggingHelper
    {
        public static Vector GetDraggingVector(Vector originalVector, DragTypes dragType)
        {
            if (dragType == DragTypes.Horizontal)
                return new Vector(originalVector.X, 0);

            if (dragType == DragTypes.Vertical)
                return new Vector(0, originalVector.Y);

            if (dragType == DragTypes.Both)
                return new Vector(originalVector.X, originalVector.Y);

            return new Vector(0, 0);
        }

        public static Vector SnapToGrid(Vector dragVector, SnapOptions snapOptions)
        {
            //  no need to adjusting vector to nearest grid nodes
            if (!snapOptions.HorizontalSnap && !snapOptions.VerticalSnap)
                return dragVector;

            
            return new Vector
                       {
                           X = snapOptions.HorizontalSnap ? Math.Round(dragVector.X / snapOptions.HorizontalGridStep, MidpointRounding.AwayFromZero) * snapOptions.HorizontalGridStep : dragVector.X,
                           Y = snapOptions.VerticalSnap ? Math.Round(dragVector.Y / snapOptions.VerticalGridStep, MidpointRounding.AwayFromZero) * snapOptions.VerticalGridStep : dragVector.Y
                       };

        }
        public static Vector CalculateNewSnapPosition(Vector totalMove, ElementOptions options)
        {
            // cumulative dragging distance adjusted to dragging direction (vertical/horizontal/both)
            var dragVector = GetDraggingVector(totalMove, options.DragType);

            //  snap to nearest grid line
            return SnapToGrid(dragVector, options.SnapOptions);
        }
    }
}