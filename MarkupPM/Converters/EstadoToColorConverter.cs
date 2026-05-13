using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MarkupPM.Models;

namespace MarkupPM.Converters;

/// <summary>Returns the background brush for a status chip. Respects light/dark theme via resource lookup.</summary>
public class EstadoToColorConverter : IValueConverter
{
    /// <summary>When true, returns the foreground text color instead of the background.</summary>
    public bool Foreground { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not EstadoTarea estado) return Binding.DoNothing;

        var (bgKey, fgKey) = estado switch
        {
            EstadoTarea.Hecha   => ("StatusDoneBg",       "StatusDoneFg"),
            EstadoTarea.EnCurso => ("StatusInProgressBg", "StatusInProgressFg"),
            _                   => ("StatusPendingBg",    "StatusPendingFg")
        };

        var key = Foreground ? fgKey : bgKey;

        if (Application.Current.TryFindResource(key) is Brush brush)
            return brush;

        // Fallback hard-coded values
        return Foreground
            ? new SolidColorBrush(estado == EstadoTarea.Hecha ? Color.FromRgb(46, 125, 50)
                : estado == EstadoTarea.EnCurso ? Color.FromRgb(21, 101, 192)
                : Color.FromRgb(97, 97, 97))
            : new SolidColorBrush(estado == EstadoTarea.Hecha ? Color.FromRgb(232, 245, 233)
                : estado == EstadoTarea.EnCurso ? Color.FromRgb(227, 242, 253)
                : Color.FromRgb(245, 245, 245));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

