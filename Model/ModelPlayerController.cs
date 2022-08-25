using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfTiles.Common;

namespace WpfTiles.Model
{
    public enum ENUM_PlayerMovesCollectionChangedType
    {
        ADD,
        REMOVE,
    }
    class ModelPlayerController
    {
        private CancellationToken _Ct;
        private CancellationTokenSource _Src;
        public PlayerTileItem Player { get; set; }
        public List<ControlTileItem> ControlTiles { get; set; }
        public List<ControlTileItem> PlayerMoves { get; set; }
        public event EventHandler<PlayerMovesCollectionChangedEventArgs> PlayerMovesCollectionChanged;
        public event EventHandler<PlayerMoveMadeEventArgs> PlayerMoveMadeEventHandler;

        private void AddToPlayerMoves(ControlTileItem item, int index)
        {
            if (index != -1)
                PlayerMoves.Insert(index, item);
            else
                PlayerMoves.Add(item);
            PlayerMovesCollectionChangedMethod(ENUM_PlayerMovesCollectionChangedType.ADD, item, index);
        }

        private void RemoveFromPlayerMoves(ControlTileItem item, int index)
        {
            if (index < 0 || index > PlayerMoves.Count - 1)
                return;
            PlayerMoves.RemoveAt(index);
            PlayerMovesCollectionChangedMethod(ENUM_PlayerMovesCollectionChangedType.REMOVE, item, index);
        }

        private void PlayerMovesCollectionChangedMethod(ENUM_PlayerMovesCollectionChangedType type, ControlTileItem item, int index)
        {
            EventHandler< PlayerMovesCollectionChangedEventArgs> handler = PlayerMovesCollectionChanged;
            var args = new PlayerMovesCollectionChangedEventArgs()
            {
                ChangeType = type,
                Item = item,
                Index = index,
            };
            handler?.Invoke(this, args);
        }
        private void PlayerMoveMadeEventMethod(int index)
        {
            EventHandler<PlayerMoveMadeEventArgs> handler = PlayerMoveMadeEventHandler;
            var args = new PlayerMoveMadeEventArgs()
            {
                Index = index,
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

        private async Task PlayerMoveSetTask(Dictionary<uint, List<ControlTileItem>> moveSetDict)
        {
            try
            {
                for (int i = 0; i < PlayerMoves.Count(); i++)
                {
                    _Ct.ThrowIfCancellationRequested();
                    await Task.Run(() =>
                    {
                        _Ct.ThrowIfCancellationRequested();
                        if (PlayerMoves[i].Sign != -1)
                        {
                            if (Player.MakeSignMove(PlayerMoves[i]));
                                PlayerMoveMadeEventMethod(i);
                        }
                        else if (!string.IsNullOrEmpty(PlayerMoves[i].Name))
                        {
                            var tmp = UInt32.Parse(PlayerMoves[i].Name.Replace("f", ""));
                            if (Player.ValidateMoveSet(PlayerMoves[i]))
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                                {
                                    AddMoveSet(moveSetDict[tmp], i, i);
                                }));
                                i--;
                            }
                        }
                    }, _Ct);

                    await Task.Delay(2000, _Ct);
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        public void StartMoveSet()
        {
            if (_Src != null)
            {
                //cancel atm running player and reset player and arena
                _Src.Cancel();
            }
            var moveSetDict = InitMoveSetDict(); 
            PlayerMoves = new List<ControlTileItem>();
            AddMoveSet(moveSetDict.FirstOrDefault().Value);
            _Src = new CancellationTokenSource();
            _Ct = _Src.Token;
            var PlayerMovementTask = new Task(async () => await PlayerMoveSetTask(moveSetDict), _Ct);
            //PlayerMovementTask.ContinueWith(); //check result
            PlayerMovementTask.Start();
        }

        public ModelPlayerController(PlayerTileItem player, List<ControlTileItem> contItems)
        {
            Player = player;
            ControlTiles = contItems;
            PlayerMoves = new List<ControlTileItem>();
        }
    }
}
