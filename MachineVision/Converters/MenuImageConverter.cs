using System;
using System.Globalization;
using System.Windows.Data;

namespace MachineVision.Converters
{
    public class MenuImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return $"/Assets/Images/{value.ToString()}.png";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
