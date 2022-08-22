using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTiles.Model
{
    public class NameTileItem : TileItem
    {
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
        public NameTileItem()
        {
            Name = "Default";
        }
        public NameTileItem(TileItem baseItem)

        {
            Name = "Default";
            X = baseItem.X;
            Y = baseItem.Y;
            Width = baseItem.Width;
            Height = baseItem.Height;
            Color = baseItem.Color;
        }
    }
}
