using Resources.Schemas.XMLSchema1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTiles.Model.Parser
{
    static class ParserMap
    {

        static public ModelControl ParseControl(MapType map)
        {
            return new ModelControl(ParseMap(map));
        }
        static public ModelMap ParseMap(MapType map)
        {
            var res = new ModelMap()
            {
                Name = map.Name,
                Width = (int)map.Width,
                Height = (int)map.Height,
            };
            return res;
        }

        static public void ParseMapMaxMinXY(ModelMap map, List<ControlTileItem> tiles)
        {
            var lst = new List<uint>();
            foreach (var item in tiles)
            {
                if (!lst.Contains(item.Y))
                    lst.Add(item.Y);
            }
            lst.Sort();

            foreach (var item in tiles)
                item.Y = Convert.ToUInt32(lst.IndexOf(item.Y)) + 1 + Convert.ToUInt32(lst.IndexOf(item.Y));
            map.MaxX = (uint)(tiles.Aggregate((c, d) => c.X > d.X ? c : d).X);
            map.MaxY = (uint)(tiles.Aggregate((c, d) => c.Y > d.Y ? c : d).Y);

            map.MinX = (uint)(tiles.Aggregate((c, d) => c.X < d.X ? c : d).X);
            map.MinY = (uint)(tiles.Aggregate((c, d) => c.Y < d.Y ? c : d).Y);
        }

        static public void ParseMapMaxMinXY(ModelMap map, List<ControlTileItem> tiles1, List<NameTileItem> tiles2)
        {
            var lst = new List<uint>();
            var tmplst = new List<TileItem>();
            tmplst.AddRange(tiles2);
            tmplst.AddRange(tiles1);
            foreach (var item in tmplst)
            {
                if (!lst.Contains(item.Y))
                    lst.Add(item.Y);
            }
            lst.Sort();

            foreach (var item in tiles2)
                item.Y = Convert.ToUInt32(lst.IndexOf(item.Y)) + 1 + Convert.ToUInt32(lst.IndexOf(item.Y));
            foreach (var item in tiles1)
                item.Y = Convert.ToUInt32(lst.IndexOf(item.Y)) + 1 + Convert.ToUInt32(lst.IndexOf(item.Y));
            map.MaxX = (uint)(tmplst.Aggregate((c, d) => c.X > d.X ? c : d).X);
            map.MaxY = (uint)(tmplst.Aggregate((c, d) => c.Y > d.Y ? c : d).Y);

            map.MinX = (uint)(tmplst.Aggregate((c, d) => c.X < d.X ? c : d).X);
            map.MinY = (uint)(tmplst.Aggregate((c, d) => c.Y < d.Y ? c : d).Y);
        }

        static public void ParseMapMaxMinXY(ModelMap map, List<TileItem> tiles)
        {
            map.MaxX = (uint)(tiles.Aggregate((c, d) => c.X > d.X ? c : d).X);
            map.MaxY = (uint)(tiles.Aggregate((c, d) => c.Y > d.Y ? c : d).Y);

            map.MinX = (uint)(tiles.Aggregate((c, d) => c.X < d.X ? c : d).X);
            map.MinY = (uint)(tiles.Aggregate((c, d) => c.Y < d.Y ? c : d).Y);
        }

        static public void ParseOffsetXY(ModelMap map)
        {
            var mapXmid = map.MapAreaWidth / 2;
            var mapYmid = map.MapAreaHeight / 2;

            var tilesXmid = (map.MaxX - map.MinX) / 2.0;
            var tilesYmid = (map.MaxY - map.MinY) / 2.0;

            map.OffsetX = (int)Math.Floor(mapXmid - tilesXmid * (int)ENUM_TileSizes.MapBackground) - (int)ENUM_TileSizes.MapBackground / 2;
            map.OffsetY = (int)Math.Floor(mapYmid - tilesYmid * (int)ENUM_TileSizes.MapBackground) - (int)ENUM_TileSizes.MapBackground / 2;
        }
    }
}
