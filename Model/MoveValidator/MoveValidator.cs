using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTiles.Model
{
    public static class MoveValidator
    {
        private static bool CheckColor(PlayerTileItem player, TileItem item, List<TileItem> mapTiles)
        {
            if (item.ColorValue == StaticTileInfo.TileColor_DefaultInt)
                return true;
            var curMapTile = mapTiles.Find(o => o.X == player.X && o.Y == player.Y);
            if (curMapTile == null)
                throw new InvalidOperationException($"MoveValidator.CheckColor: invalid x='{player.X}' || y='{player.Y}'");
            else if (curMapTile.ColorValue == StaticTileInfo.TileColor_DefaultInt)
                return true;
            return item.ColorValue == curMapTile.ColorValue;
        }
        public static bool ValidateForwardMove(PlayerTileItem player, TileItem item, List<TileItem> mapTiles)
        {
            if (!CheckColor(player, item, mapTiles)) { return false; }

            var x = player.X;
            var y = player.Y;
            if (player.Direction == 0)
                x++;
            else if (player.Direction == 1)
                y++;
            else if (player.Direction == 2)
                x--;
            else if (player.Direction == 3)
                y--;
            else
                throw new NotImplementedException($"ValidateForwardMove->_Direction:{player.Direction}, invalid");
            var nextMapTile = mapTiles.Find(o => o.X == x && o.Y == y);
            return nextMapTile != null;
        }

        public static bool ValidateRotation(PlayerTileItem player, TileItem item, List<TileItem> mapTiles)
        {
            return CheckColor(player, item, mapTiles);
        }

        public static bool ValidateMoveSet(PlayerTileItem player, TileItem item, List<TileItem> mapTiles)
        {
            return CheckColor(player, item, mapTiles);
        }
    }
}
