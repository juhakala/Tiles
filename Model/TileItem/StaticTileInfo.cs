using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Model.Parser;

namespace WpfTiles.Model
{
    public static class StaticTileInfo
    {
        public static int MapBackground_Height { get { return (int)ENUM_TileSizes.MapBackground; } }
        public static int MapBackground_Width { get { return (int)ENUM_TileSizes.MapBackground; } }
        public static int MapPlayer_Height { get { return (int)ENUM_TileSizes.MapPlayer; } }
        public static int MapPlayer_Width { get { return (int)ENUM_TileSizes.MapPlayer; } }

        public static int TileImg_Height { get { return (int)ENUM_TileSizes.MapPlayer - 5; } }
        public static int TileImg_Width { get { return (int)ENUM_TileSizes.MapPlayer - 5; } }

    }
}
