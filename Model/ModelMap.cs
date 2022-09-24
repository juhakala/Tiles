using System;

namespace WpfTiles.Model
{
    class ModelMap : NotifyPropertyChangedBase
    {
        public string Name { get; set; }
        public int Width { get; set; } //not needed? since it's calculated from tiles min/maxXY 
        public int Height { get; set; } //not needed? since it's calculated from tiles min/maxXY
        public uint MaxX { get; set; }
        public uint MinX { get; set; }
        public uint MaxY { get; set; }
        public uint MinY { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public ModelMap()
        {
            Name = String.Empty;
        }
    }
}
