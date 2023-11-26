using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TaskForElogistics.Service
{
    /*
     * Класс реализующий интерфейс IMultiValueConverter
    */
    public class MultiValueConverter : IMultiValueConverter
    {
        // функция возвращает временной период
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {           
            if (values[0] == null || values[1] == null)
            {
                return null;
            }

            DateTime start = (DateTime)values[0];
            DateTime end = (DateTime)values[1];

            (DateTime StartDay, DateTime EndDay) period = (start, end);

            return period; 
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
