using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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

        // need better system, get gray value as default for now
        private static int _TileColor_DefaultInt = int.Parse(new SolidColorBrush(System.Windows.Media.Colors.Gray).Color.ToString().Substring(3), System.Globalization.NumberStyles.HexNumber); 
        public static int TileColor_DefaultInt { get { return _TileColor_DefaultInt; } }

        public static int MapArea_Width { get { return 400; } }
        public static int MapArea_Height { get { return 400; } }

        public static int CanvasControlArea_Width { get { return 200; } }
        public static int CanvasControlArea_Height { get { return 200; } }

        public static int CanvasAvailavleControlArea_Width { get { return 200; } }
        public static int CanvasAvailavleControlArea_Height { get { return 150; } }


    }
}
