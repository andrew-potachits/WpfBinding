using System.Windows;

namespace WpfBinding
{
    public class DemoElementOptionsProvider : IElementOptionsProvider
    {
        public ElementOptions GetElementOptions(LineBase element)
        {
            var options = new ElementOptions
                              {
                                  SnapOptions = new Vector(1, 1)
                              };

            if (element.Id.StartsWith("BOX"))
            {
                options.DragType = DragTypes.None;
            }
            else
            {
                if (element.Id.StartsWith("REBAR"))
                {
                    if (element.Id.Contains("HORIZONTAL"))
                        options.DragType = DragTypes.Vertical;
                    else if (element.Id.Contains("VERTICAL"))
                        options.DragType = DragTypes.Horizontal;
                    else
                        options.DragType = DragTypes.Both;
                }
            }
            return options;
        }
    }
}