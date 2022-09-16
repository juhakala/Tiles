using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTiles.Model
{
    public class PlaybackTileItem : ControlTileItem
    {
        private int _VideoOffset;
        public int VideoOffset
        {
            get { return _VideoOffset; }
            set
            {
                if (_VideoOffset != value)
                {
                    _VideoOffset = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
