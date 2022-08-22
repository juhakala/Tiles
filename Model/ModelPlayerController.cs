using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTiles.Model
{
    public enum ENUM_PlayerMovesCollectionChangedType
    {
        ADD,
        REMOVE,
    }
    public class PlayerMovesCollectionChangedEventArgs : EventArgs
    {
        public ENUM_PlayerMovesCollectionChangedType ChangeType { get; set; }
        public ControlTileItem Item { get; set; }
    }
    class ModelPlayerController
    {
        public PlayerTileItem Player { get; set; }
        public List<ControlTileItem> ControlTiles { get; set; }
        public List<ControlTileItem> PlayerMoves { get; set; }

        public event EventHandler<PlayerMovesCollectionChangedEventArgs> PlayerMovesCollectionChanged;

        private void AddToPlayerMoves(ControlTileItem item, int index)
        {
            if (index != -1)
                PlayerMoves.Insert(index, item);
            else
                PlayerMoves.Add(item);
            PlayerMovesCollectionChangedMethod(ENUM_PlayerMovesCollectionChangedType.ADD, item);
        }

        private void RemoveFromPlayerMoves(ControlTileItem item, int index)
        {
            if (index < 0 || index > PlayerMoves.Count - 1)
                return;
            PlayerMoves.RemoveAt(index);
            PlayerMovesCollectionChangedMethod(ENUM_PlayerMovesCollectionChangedType.REMOVE, item);
        }

        private void PlayerMovesCollectionChangedMethod(ENUM_PlayerMovesCollectionChangedType type, ControlTileItem item)
        {
            EventHandler< PlayerMovesCollectionChangedEventArgs> handler = PlayerMovesCollectionChanged;
            var args = new PlayerMovesCollectionChangedEventArgs()
            {
                ChangeType = type,
                Item = item,
            };
            handler?.Invoke(this, args);
        }

        private Dictionary<uint, List<ControlTileItem>> InitMoveSetDict()
        {
            var rDict = new Dictionary<uint, List<ControlTileItem>>();
            foreach (var item in ControlTiles)
            {
                var key = item.Y / 2 + 1;
                if (rDict.ContainsKey(key))
                {
                    rDict[key].Add(item);
                }
                else
                {
                    rDict.Add(key, new List<ControlTileItem>() { item });
                }
            }
            rDict.OrderBy(o => o.Key);
            foreach (var item in rDict)
            {
                item.Value.OrderBy(o => o.X);
            }
            return rDict;
        }

        private void AddMoveSet(List<ControlTileItem> mlst, int indexToAdd = -1, int indexToRemove = -1)
        {
            if (mlst == null)
            {
                //relevant error logging
                throw new NotImplementedException($"ModelPlayerController.AddMoveSet mlst null");
            }
            if (indexToRemove != -1 && mlst.FirstOrDefault() != null)
                RemoveFromPlayerMoves(mlst.First(), indexToRemove);
            foreach (var item in mlst)
            {
                AddToPlayerMoves(item, indexToAdd);
                indexToAdd = indexToAdd != -1 ? indexToAdd + 1: indexToAdd;
            }
        }

        public void StartMoveSet()
        {
            var moveSetDict = InitMoveSetDict(); 
            PlayerMoves = new List<ControlTileItem>();
            AddMoveSet(moveSetDict.FirstOrDefault().Value);
            for (int i = 0; i < PlayerMoves.Count(); i++)
            {
                if (PlayerMoves[i].Sign != -1)
                {
                    Player.MakeSignMove(PlayerMoves[i]);
                }
                else if (!string.IsNullOrEmpty(PlayerMoves[i].Name))
                {
                    var tmp = UInt32.Parse(PlayerMoves[i].Name.Replace("f", ""));
                    if (Player.ValidateMoveSet(PlayerMoves[i]))
                        AddMoveSet(moveSetDict[tmp], i, i);
                    i--;
                }
            }
        }

        public ModelPlayerController(PlayerTileItem player, List<ControlTileItem> contItems)
        {
            Player = player;
            ControlTiles = contItems;
            PlayerMoves = new List<ControlTileItem>();
        }
    }
}
