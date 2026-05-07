using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MarkupPM.Models;

namespace MarkupPM.Converters;

public class EstadoToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not EstadoTarea estado) return Binding.DoNothing;
        return estado switch
        {
            EstadoTarea.EnCurso => new SolidColorBrush(Color.FromRgb(21,  101, 192)),
            EstadoTarea.Hecha   => new SolidColorBrush(Color.FromRgb(27,   94,  32)),
            _                   => new SolidColorBrush(Color.FromRgb(55,   71,  79))
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
