using System;
using System.Globalization;
using System.Windows.Data;
using WpfTiles.Model;

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
                    var tmp = (int)(values[1]) + (double)((int)(values[0]) * StaticTileInfo.MapBackground_Width);
                    return (int)(values[1]) + (double)((int)(values[0]) * StaticTileInfo.MapBackground_Width); //to negative
                }
                else
                    return (double)((uint)(values[0]) * StaticTileInfo.MapBackground_Width + (int)(values[1])); //to positive
            }

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
