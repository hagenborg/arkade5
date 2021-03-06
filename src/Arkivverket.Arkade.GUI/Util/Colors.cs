using System.Windows.Media;

namespace Arkivverket.Arkade.GUI.Util
{
    /// <summary>
    /// Inspired of Google Material Design: https://material.google.com/style/color.html#color-color-palette
    /// </summary>
    public class Colors
    {
        public static readonly Brush Blue500 = (Brush)new BrushConverter().ConvertFrom("#2196F3");
        public static readonly Brush Teal500 = (Brush) new BrushConverter().ConvertFrom("#009688");
        public static readonly Brush Gray50 = (Brush)new BrushConverter().ConvertFrom("#FAFAFA");
    }
}