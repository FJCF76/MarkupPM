using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MarkupPM.Models;

namespace MarkupPM.Converters;

public class PrioridadToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not PrioridadTarea prioridad) return Binding.DoNothing;
        return prioridad switch
        {
            PrioridadTarea.Alta => new SolidColorBrush(Color.FromRgb(239,  83,  80)),
            PrioridadTarea.Baja => new SolidColorBrush(Color.FromRgb(102, 187, 106)),
            _                   => new SolidColorBrush(Color.FromRgb(255, 152,   0))
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
