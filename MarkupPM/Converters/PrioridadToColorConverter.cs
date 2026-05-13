using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MarkupPM.Models;

namespace MarkupPM.Converters;

/// <summary>Returns the background or foreground brush for a priority chip.</summary>
public class PrioridadToColorConverter : IValueConverter
{
    /// <summary>When true, returns the foreground text color instead of the background.</summary>
    public bool Foreground { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not PrioridadTarea prioridad) return Binding.DoNothing;

        var (bgKey, fgKey) = prioridad switch
        {
            PrioridadTarea.Alta => ("PriorityHighBg", "PriorityHighFg"),
            PrioridadTarea.Baja => ("PriorityLowBg",  "PriorityLowFg"),
            _                   => ("PriorityMedBg",  "PriorityMedFg")
        };

        var key = Foreground ? fgKey : bgKey;

        if (Application.Current.TryFindResource(key) is Brush brush)
            return brush;

        // Fallback
        return Foreground
            ? new SolidColorBrush(prioridad == PrioridadTarea.Alta ? Color.FromRgb(198, 40, 40)
                : prioridad == PrioridadTarea.Baja ? Color.FromRgb(46, 125, 50)
                : Color.FromRgb(230, 81, 0))
            : new SolidColorBrush(prioridad == PrioridadTarea.Alta ? Color.FromRgb(255, 235, 238)
                : prioridad == PrioridadTarea.Baja ? Color.FromRgb(232, 245, 233)
                : Color.FromRgb(255, 243, 224));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

