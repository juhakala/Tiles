using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfTiles.Model.Parser;

namespace WpfTiles.Model
{
    public class TileItem : NotifyPropertyChangedBase, ITileItem
    {
        private uint _X;
        private uint _Y;
        private SolidColorBrush _Color = new SolidColorBrush(System.Windows.Media.Colors.Gray);
        private int _ColorValue = StaticTileInfo.TileColor_DefaultInt;
        private bool _Star;

        public int TileWidth { get { return (int)ENUM_TileSizes.MapBackground; } }
        public int TileHeight { get { return (int)ENUM_TileSizes.MapBackground; } }

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

        public bool Star
        {
            get { return _Star; }
            set
            {
                if (_Star != value)
                {
                    _Star = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }
}
