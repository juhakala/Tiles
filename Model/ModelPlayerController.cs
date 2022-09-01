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
        NORMAL_FORWARD,
        HISTORY_FORWARD,
        HISTORY_BACKWARD,
    }
    class ModelPlayerController
    {
        private CancellationToken _Ct;
        private CancellationTokenSource _Src;
        private int _PlayerMovesIndex;
        private Dictionary<uint, List<ControlTileItem>> _MoveSetDict;
        private int _HistoryOffsetIndex;

        public PlayerTileItem Player { get; set; }
        public List<ControlTileItem> ControlTiles { get; set; }
        public List<ControlTileItem> PlayerMoves { get; set; }
        public List<ControlTileItem> PlayerMoveHistoryTiles { get; set; } = new List<ControlTileItem>();

        public event EventHandler<PlayerMovesCollectionChangedEventArgs> PlayerMovesCollectionChanged;
        public event EventHandler<PlayerMovesCollectionChangedEventArgs> PlayerMoveMadeEventHandler;

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
                Id = item.Id,
            };
            handler?.Invoke(this, args);
        }
        private void PlayerMoveMadeEventMethod(ENUM_PlayerMovesCollectionChangedType type, ControlTileItem item, int index)
        {
            EventHandler<PlayerMovesCollectionChangedEventArgs> handler = PlayerMoveMadeEventHandler;
            var args = new PlayerMovesCollectionChangedEventArgs()
            {
                ChangeType = type,
                Item = PlayerMoves[_PlayerMovesIndex],
                Index = index,
                Id = item.Id,
            };
            handler?.Invoke(this, args);
        }

        private Dictionary<uint, List<ControlTileItem>> InitMoveSetDict()
        {
            var rDict = new Dictionary<uint, List<ControlTileItem>>();
            foreach (var item in ControlTiles)
            {
                item.Selected = false;
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

        private void AddMoveSet(uint movesetDictIndex, int indexToAdd = -1, int indexToRemove = -1)
        {
            if (_MoveSetDict == null)
            {
                //relevant error logging
                throw new NotImplementedException($"ModelPlayerController.AddMoveSet mlst null");
            }
            if (indexToRemove != -1)
                RemoveFromPlayerMoves(PlayerMoves[indexToRemove], indexToRemove);
            foreach (var item in _MoveSetDict[movesetDictIndex])
            {
                AddToPlayerMoves(item, indexToAdd);
                indexToAdd = indexToAdd != -1 ? indexToAdd + 1: indexToAdd;
            }
        }

        public void PlayerMoveStepForwardOne(object sender, EventArgs e)
        {
            if (_Src == null)
                InitMoveSet();
            else
            {
                var task = new Task(async () => await PlayerMoveSetAdvanceOneTask());
                task.Start();
            }
        }

        private async Task PlayerMoveSetAdvanceOneTask()
        {
            await Task.Run(() =>
            {
                if (_HistoryOffsetIndex == 0) // make normal move
                {
                    if (_PlayerMovesIndex < PlayerMoves.Count)
                    {
                        PlayerMoveSetAdvanceOne();
                        _PlayerMovesIndex++;
                    }
                    else
                    {
                        //no more moves left in bank
                        // show error that not passed?
                        throw new NotImplementedException($"ModelPlayerController.PlayerMoveSetAdvanceOneTask => _PlayerMovesIndex:{_PlayerMovesIndex}, PlayerMoves.Count:{PlayerMoves.Count}");
                    }
                }
                else //make history move
                {
                    //and advance from history tiles (with last?)
                    //think is need to add fcalls too to history tiles since coloring will be there?
                    _HistoryOffsetIndex++;
                }
            });
        }

        private void PlayerMoveSetAdvanceOne()
        {
            if (PlayerMoves[_PlayerMovesIndex].Sign != -1)
            {
                // for move tiles
                if (Player.MakeSignMove(PlayerMoves[_PlayerMovesIndex]))
                {
                    PlayerMoveHistoryTiles.Add(PlayerMoves[_PlayerMovesIndex]);
                }
                PlayerMoveMadeEventMethod(ENUM_PlayerMovesCollectionChangedType.NORMAL_FORWARD, PlayerMoves[_PlayerMovesIndex], _PlayerMovesIndex);
            }
            else if (!string.IsNullOrEmpty(PlayerMoves[_PlayerMovesIndex].Name))
            {
                // for fcall tiles
                var tmp = UInt32.Parse(PlayerMoves[_PlayerMovesIndex].Name.Replace("f", ""));
                if (Player.ValidateMoveSet(PlayerMoves[_PlayerMovesIndex]))
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        AddMoveSet(tmp, _PlayerMovesIndex, _PlayerMovesIndex);
                    }));
                    _PlayerMovesIndex--;
                }
                else
                {
                    PlayerMoveMadeEventMethod(ENUM_PlayerMovesCollectionChangedType.NORMAL_FORWARD, PlayerMoves[_PlayerMovesIndex], _PlayerMovesIndex);
                }
            }
            else
            {
                // for empty tiles
                PlayerMoveMadeEventMethod(ENUM_PlayerMovesCollectionChangedType.NORMAL_FORWARD, PlayerMoves[_PlayerMovesIndex], _PlayerMovesIndex);
            }
        }

        private async Task PlayerMoveSetTask()
        {
            try
            {
                for (_PlayerMovesIndex = 0; _PlayerMovesIndex < PlayerMoves.Count(); _PlayerMovesIndex++)
                {
                    _Ct.ThrowIfCancellationRequested();
                    await Task.Run(() =>
                    {
                        _Ct.ThrowIfCancellationRequested();
                        PlayerMoveSetAdvanceOne();
                    }, _Ct);

                    await Task.Delay(2000, _Ct);
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        public void InitMoveSet()
        {
            if (_Src != null)
            {
                //cancel atm running player and reset player and arena
                _Src.Cancel();
            }
            PlayerMoveHistoryTiles = new List<ControlTileItem>();
            _MoveSetDict = InitMoveSetDict();
            PlayerMoves = new List<ControlTileItem>();
            AddMoveSet(_MoveSetDict.FirstOrDefault().Key);
            _Src = new CancellationTokenSource();
            _Ct = _Src.Token;
        }

        public void StartMoveSet()
        {
            InitMoveSet();
            var PlayerMovementTask = new Task(async () => await PlayerMoveSetTask(), _Ct);
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
