using System;

namespace WpfTiles.Model
{
    class ModelControl : ModelMap
    {
        public ModelControl()
        {
            Name = String.Empty;
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
        }
    }
}
