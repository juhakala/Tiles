using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfTiles.Common;

namespace WpfTiles.Model
{
    public enum ENUM_SignTypes
    {
        Forward,
        RotateRight,
        RotateLeft,
    }
    public class ControlTileItem : TileItem
    {
        private int _Sign;
        public int Sign
        {
            get { return _Sign; }
            set
            {
                if (_Sign != value)
                {
                    _Sign = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                if (value != _Selected)
                {
                    _Selected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand LeftClickCommand => new RelayCommand(o => SetSelected());

        private void SetSelected()
        {
            Selected = !Selected;
        }

        public ControlTileItem() 
        {
            Sign = -1;
            Name = string.Empty;
        }
        public ControlTileItem(TileItem baseItem)
        {
            Sign = -1;
            Name = string.Empty;
            X = baseItem.X;
            Y = baseItem.Y;
            Width = baseItem.Width;
            Height = baseItem.Height;
            Color = baseItem.Color;
        }
        public ControlTileItem Clone()
        {
            var res = new ControlTileItem();
            res.Name = Name;
            res.X = X;
            res.Y = Y;
            res.Width = Width;
            res.Height = Height;
            res.Color = Color;
            res.Sign = Sign;
            res.Selected = Selected;
            return res;
        }
    }
}
