using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfTiles.Model
{
    public class TileItem : NotifyPropertyChangedBase
    {
        private uint _X;
        private uint _Y;
        private double _Width;
        private double _Height;
        private SolidColorBrush _Color = new SolidColorBrush(System.Windows.Media.Colors.Gray);
        private int _ColorValue = 8421504;


        public uint X
        {
            get { return _X; }
            set
            {
                if (_X != value)
                {
                    _X = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public uint Y
        {
            get { return _Y; }
            set
            {
                if (_Y != value)
                {
                    _Y = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double Width
        {
            get { return _Width; }
            set
            {
                if (_Width != value)
                {
                    _Width = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double ImgWidth
        {
            get { return Width - 5; }
        }
        public double Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public SolidColorBrush Color
        {
            get { return _Color; }
            set
            {
                if (_Color != value)
                {
                    _Color = value;
                    ColorValue = int.Parse(value.Color.ToString().Substring(3), System.Globalization.NumberStyles.HexNumber);
                    NotifyPropertyChanged();
                }
            }
        }
        public int ColorValue
        {
            get { return _ColorValue; }
            private set
            {
                if (_ColorValue != value)
                {
                    _ColorValue = value;
                }
            }
        }
    }
}
