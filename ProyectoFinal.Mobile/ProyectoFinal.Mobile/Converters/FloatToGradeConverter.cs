using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.Converters
{
    class FloatToGradeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float floatValue = (float)value;
            return $"{Math.Round(floatValue, 2):F2}/{5:F2}";
        }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
