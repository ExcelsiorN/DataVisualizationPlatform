using System.Windows.Data;
using System.Globalization;

namespace DataVisualizationPlatform.Commands
{
    public class SubtractConverter : IValueConverter
    {
        public double Offset { get; set; } = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                double result = width - Offset;
                return result > 0 ? result : 0; 
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
