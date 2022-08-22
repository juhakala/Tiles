using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfTiles.Converters
{
    internal class ConverterSignToImage : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 1 && values[0] is int sign)
            {
                var img = new ImageBrush();
                if (sign == 0)
                    img.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/WpfTiles;component/Resources/Images/RightArrow.png"));
                else if (sign == 1)
                    img.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/WpfTiles;component/Resources/Images/RotateRight.png"));
                else if (sign == 2)
                    img.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/WpfTiles;component/Resources/Images/RotateLeft.png"));
                else
                    img.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/WpfTiles;component/Resources/Images/TransparentImage.png"));
                return img;
            }
            throw new NotImplementedException();

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
