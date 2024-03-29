﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.Converters
{
    class EndDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var endDateValue = (DateTime)value;
            bool vigente = DateTime.Compare(DateTime.Now, endDateValue) <= 0; // Si la fecha y hora actuales son anteriores a la fecha límite, la subasta se encuentra vigente
            if (vigente)
            {
                return endDateValue.ToString("dd MMM yyyy, HH:mm");
            }
            return "Finalizado";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
