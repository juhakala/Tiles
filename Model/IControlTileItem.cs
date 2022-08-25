using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTiles.Model
{
    public interface IControlTileItem : ITileItem
    {
        public int Sign { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
