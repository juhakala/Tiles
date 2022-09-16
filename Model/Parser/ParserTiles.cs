using Resources.Schemas.XMLSchema1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfTiles.Model.Parser
{
    enum ENUM_TileSizes
    {
        MapBackground = 20,
        MapPlayer = 18,
    }
    class ParserTiles
    {

        static private SolidColorBrush CorrectColor(TileColorType color)
        {
            var res = new SolidColorBrush(Colors.Gray);
            switch (color)
            {
                case TileColorType.gray:
                    res = new SolidColorBrush(Colors.Gray);
                    break;
                case TileColorType.green:
                    res = new SolidColorBrush(Colors.Green);
                    break;
                case TileColorType.red:
                    res = new SolidColorBrush(Colors.Red);
                    break;
                case TileColorType.blue:
                    res = new SolidColorBrush(Colors.Blue);
                    break;
                default:
                    break;
            }
            return res;
        }

        static public List<ControlTileItem> ParseAvailableTiles(List<NameTileItem> tiles)
        {
            var res = new List<ControlTileItem>();
            foreach (var item in tiles)
            {
                res.Add(new ControlTileItem() { X = (uint)res.Count(), Name=item.Name, Y = 2 });
            }
            return res;
        }

        static public List<ControlTileItem> ParseAvailableTiles(AvailableControlTilesType tiles, int mode)
        {
            var res = new List<ControlTileItem>();
            foreach (var item in tiles.AvailableControlTile)
            {
                if (mode == 0 && item.Item is uint)
                    res.Add(new ControlTileItem() { X=(uint)res.Count(), Sign=Convert.ToInt32((uint)item.Item), Y = 0 });
                else if (mode == 1 && item.Item is TileColorType)
                    res.Add(new ControlTileItem() { X=(uint)res.Count(), Color=CorrectColor((TileColorType)item.Item), Y = 1 });
            }
            return res;
        }

        static public PlayerTileItem Parse(PlayerTilesType tiles)
        {
            var tmp = Parse(new TileType[1] { tiles.PlayerTile } );
            if (tmp.Count() != 1)
            {
                //add relevant error message and logging once inplemented
                return new PlayerTileItem();
            }
            return new PlayerTileItem(tmp[0]);
        }

        static public (List<ControlTileItem>, List<NameTileItem>) Parse(ControlTilesType tiles)
        {
            var res1 = new List<ControlTileItem>();
            var res2 = new List<NameTileItem>();

            for (int i = 0; i < tiles.ControlTile.Length; i++)
            {
                res2.Add(new NameTileItem() { X = 0, Y = Convert.ToUInt32(i), Name = $"f{i+1}"});
                for (int j = 0; j < tiles.ControlTile[i].Length; j++)
                {
                    res1.Add(new ControlTileItem() { X=Convert.ToUInt32(j) + 1, Y=Convert.ToUInt32(i) });
                }
            }
            return (res1, res2);
        }

        static public List<TileItem> Parse(MapTilesType tiles)
        {
            return Parse(tiles.MapTile);
        }

        static private List<TileItem> Parse(TileType[] tiles)
        {
            var res = new List<TileItem>();
            foreach (var item in tiles)
            {
                res.Add(new TileItem() { X=item.X, Y=item.Y, Color=CorrectColor(item.Color), Star=item.Star });
            }
            if (res.GroupBy(p => new { p.X, p.Y }).Count() != res.Count())
            {
                //add relevant error message and logging once inplemented
                return new List<TileItem>();
            }
            return res;
        }

        static public void ParseTilesNewXY(ModelMap map, TileItem tile)
        {
            tile.X -= map.MinX;
            tile.Y -= map.MinY;
        }

        static public void ParseTilesNewXY(ModelMap map, List<ControlTileItem> tiles)
        {
            tiles.ForEach(o => {
                o.X -= map.MinX;
                o.Y -= map.MinY;
            });
        }

        static public void ParseTilesNewXY(ModelMap map, List<ControlTileItem> tiles, List<NameTileItem> tiles2)
        {
            tiles.ForEach(o => {
                o.X -= map.MinX;
                o.Y -= map.MinY;
            });
            tiles2.ForEach(o => {
                o.X -= map.MinX;
                o.Y -= map.MinY;
            });
        }

        static public void ParseTilesNewXY(ModelMap map, List<TileItem> tiles)
        {
            tiles.ForEach(o => { 
                o.X -= map.MinX; 
                o.Y -= map.MinY; 
            } );
        }

    }
}
