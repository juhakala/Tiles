using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfTiles.Model.Parser;

namespace WpfTiles.Converters
{
    class ConverterCoordinateToOffset : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if (parameter is bool && (bool)parameter == false)
                {
                    var tmp = (int)(values[1]) + (double)((int)(values[0]) * (int)ENUM_TileSizes.MapBackground);
                    return (int)(values[1]) + (double)((int)(values[0]) * (int)ENUM_TileSizes.MapBackground); //to negative
                }
                else
                    return (double)((uint)(values[0]) * (int)ENUM_TileSizes.MapBackground + (int)(values[1])); //to positive
            }

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
