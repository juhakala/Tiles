using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTiles.Model
{
    class ModelControl : ModelMap
    {
        public ModelControl()
        {
            Name = String.Empty;
            MapAreaWidth = 200;
            MapAreaHeight = 200;
        }
        public ModelControl(ModelMap map)
        {
            MaxX = map.MaxX;
            MinX = map.MinX;
            MaxY = map.MaxY;
            MinY = map.MinY;
            OffsetX = map.OffsetX;
            OffsetY = map.OffsetY;
            Name = map.Name;
            MapAreaWidth = 200;
            MapAreaHeight = 200;
        }

    }
}
