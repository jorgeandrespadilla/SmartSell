using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ProyectoFinal.UWP.Converters
{
    class CurrencyConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            float floatValue = (float)value;
            return floatValue.ToString("C2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
