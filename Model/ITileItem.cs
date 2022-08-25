using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfTiles.Model
{
    public interface ITileItem
    {
        uint X { get; set; }
        uint Y { get; set; }
        double Width { get; set; }
        double ImgWidth { get; }
        double Height { get; set; }
        SolidColorBrush Color { get; set; }
        int ColorValue { get; }
    }
}
