using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shell;
using WpfTiles.Common;
using WpfTiles.Model.Notification;

namespace WpfTiles.Model
{
    public enum ENUM_PlayerMovesCollectionChangedType
    {
        ADD,
        REMOVE,
        NORMAL_FORWARD,
        HISTORY_FORWARD,
        HISTORY_BACKWARD,
        RESET,
    }
    public enum ENUM_PlayerGameStatus
    {
        NONE,
        STARTED,
        RUNNING,
        PAUSED, // use with in "one step at the time" too
        END,
    }

    class ModelPlayerController
    {
        private CancellationToken _Ct;
        private CancellationTokenSource _Src;
        private int _PlayerMovesIndex;
        private Dictionary<uint, List<ControlTileItem>> _MoveSetDict;
        private int _HistoryOffsetIndex;
        private ENUM_PlayerGameStatus _PlayerStatus;
        private string _FilePath;

        //backup for resetting when needed
        private List<TileItem> _BackUpMapTiles;
        private PlayerTileItem _BackUpPlayer;
        //cont items in ControlTiles

        public PlayerTileItem Player { get; set; }
        public List<ControlTileItem> ControlTiles { get; set; }
        public List<ControlTileItem> PlayerMoves { get; set; }
        public List<ControlTileItem> PlayerMoveHistoryTiles { get; set; } = new List<ControlTileItem>();
        public ModelScoreBoard ScoreBoard { get; set; }

        public event EventHandler<ProgressUpdatedEventArgs> ProgressUpdated;

        private void UpdateGameProgress(double value, TaskbarItemProgressState state)
        {
            if (state == TaskbarItemProgressState.Paused)
            {
                _PlayerStatus = ENUM_PlayerGameStatus.PAUSED;
            }
            else if (state == TaskbarItemProgressState.Indeterminate)
            {
                _PlayerStatus = ENUM_PlayerGameStatus.RUNNING;
            }
            else if (state == TaskbarItemProgressState.None && value == 0.0)
            {
                //_PlayerStatus = ENUM_PlayerGameStatus.NONE;
            }
            else if ((state == TaskbarItemProgressState.None && value == 1.0 ) || state == TaskbarItemProgressState.Error)
            {
                _PlayerStatus = ENUM_PlayerGameStatus.END;
            }
            EventHandler<ProgressUpdatedEventArgs> handler = ProgressUpdated;
            handler?.Invoke(this, new ProgressUpdatedEventArgs() {
                Value = value,
                State = state,
            });
        }

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
            if (type == ENUM_PlayerMovesCollectionChangedType.RESET)
            {
                handler?.Invoke(this, new PlayerMovesCollectionChangedEventArgs()
                {
                    ChangeType = type,
                });
            }
            else
            {
                handler?.Invoke(this, new PlayerMovesCollectionChangedEventArgs()
                {
                    ChangeType = type,
                    Item = PlayerMoves[_PlayerMovesIndex],
                    Index = index,
                    Id = item.Id,
                });
            }
        }

        public event EventHandler<LevelPassedEventArgs> LevelPassedEventHandler;

        private void PlayerLostLevel()
        {
            var noti = new ModelNotification() { Title = "Lost(title)", Text = "Level Lost(text)" };
            ModelNotificationManager.RaiseNotification(noti);
        }

        private void PlayerWonLevel()
        {
            var noti = new ModelNotification() { Title="Level Won Title", Text="Level Won Text"};
            ModelNotificationManager.RaiseNotification(noti);
            EventHandler<LevelPassedEventArgs> handler = LevelPassedEventHandler;
            handler?.Invoke(this, new LevelPassedEventArgs() { FilePath = _FilePath });
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
            if (_PlayerStatus == ENUM_PlayerGameStatus.NONE)
            {
                InitMoveSet();
            }
            else if (_PlayerStatus == ENUM_PlayerGameStatus.END)
            {
                ResetCurrentMapInstance();
                InitMoveSet();
                //throw new NotImplementedException($"ModelPlayerController.PlayerMoveStepForwardOne => _PlayerStatus:{_PlayerStatus}");
            }
            else
            {
                try
                {
                    //_Src.Cancel(); //for if already running? or prevent it totally
                    var task = new Task(async () => await PlayerMoveSetAdvanceOneTask(), _Ct);
                    task.Start();
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        private async Task PlayerMoveSetAdvanceOneTask()
        {
            try
            {
                _Ct.ThrowIfCancellationRequested();
                await Task.Run(() =>
                {
                    if (_HistoryOffsetIndex == 0) // make normal move
                    {
                        if (_PlayerMovesIndex < PlayerMoves.Count)
                        {
                            PlayerMoveSetAdvanceOne();
                            _PlayerMovesIndex++;
                            _Ct.ThrowIfCancellationRequested();
                            UpdateGameProgress(0.5, TaskbarItemProgressState.Paused);
                        }
                        if (_PlayerMovesIndex >= PlayerMoves.Count)// check if no more moves at bank
                        {
                            if (ScoreBoard.CheckIfPickedAll())//player won
                            {
                                UpdateGameProgress(1.0, TaskbarItemProgressState.None); //might happen if no moves added and still won 
                                PlayerWonLevel();
                            }
                            else if (true) //player lost
                            {
                                //show error, and reset/get ready to reset
                                UpdateGameProgress(0.5, TaskbarItemProgressState.Error);
                                PlayerLostLevel();
                                //throw new NotImplementedException($"ModelPlayerController.PlayerMoveSetAdvanceOneTask, 'else if' => _PlayerMovesIndex:{_PlayerMovesIndex}, PlayerMoves.Count:{PlayerMoves.Count}");
                            }
                            else 
                            {
                                throw new NotImplementedException($"ModelPlayerController.PlayerMoveSetAdvanceOneTask, 'else' => _PlayerMovesIndex:{_PlayerMovesIndex}, PlayerMoves.Count:{PlayerMoves.Count}");
                            }
                        }
                    
                    }
                    else //make history move
                    {
                        //and advance from history tiles (with last?)
                        //think is need to add fcalls too to history tiles since coloring will be there?
                        _HistoryOffsetIndex++;
                    }
                }, _Ct);
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (OperationCanceledException)
            {
                return;
            }
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
            if (ScoreBoard.CheckIfPickedAll())//player won
            {
                _Src.Cancel();
                UpdateGameProgress(1.0, TaskbarItemProgressState.None);
                PlayerWonLevel();
            }
        }

        private async Task PlayerMoveSetTask()
        {
            try
            {
                UpdateGameProgress(0, TaskbarItemProgressState.Indeterminate);
                await Task.Delay(1000, _Ct);
                for (; _PlayerMovesIndex < PlayerMoves.Count(); _PlayerMovesIndex++)
                {
                    _Ct.ThrowIfCancellationRequested();
                    await Task.Run(() =>
                    {
                        _Ct.ThrowIfCancellationRequested();
                        PlayerMoveSetAdvanceOne();
                    }, _Ct);

                    await Task.Delay(1000, _Ct);
                }
                UpdateGameProgress(0.5, TaskbarItemProgressState.Error);//if gets here => lost
                PlayerLostLevel();
            }
            catch (TaskCanceledException)
            {
                //UpdateGameProgress(0.5, TaskbarItemProgressState.Paused); //whi cancels => tells state
                return;
            }
        }

        public void InitMoveSet()
        {
            _PlayerStatus = ENUM_PlayerGameStatus.STARTED;
            PlayerMoveMadeEventMethod(ENUM_PlayerMovesCollectionChangedType.RESET, new ControlTileItem(), 0);
            if (_Src != null)
            {
                //cancel atm running/at end player and reset player and arena
                _Src.Cancel();
            }
            _PlayerMovesIndex = 0;
            PlayerMoveHistoryTiles = new List<ControlTileItem>();
            _MoveSetDict = InitMoveSetDict();
            PlayerMoves = new List<ControlTileItem>();
            AddMoveSet(_MoveSetDict.FirstOrDefault().Key);
            _Src = new CancellationTokenSource();
            _Ct = _Src.Token;
        }

        public void StartMoveSet()
        {
            if (_PlayerStatus == ENUM_PlayerGameStatus.NONE) // if playermoving not yet started
            {
                InitMoveSet();
                var PlayerMovementTask = new Task(async () => await PlayerMoveSetTask(), _Ct);
                //PlayerMovementTask.ContinueWith(); //check result
                PlayerMovementTask.Start();
            }
            else if (_PlayerStatus == ENUM_PlayerGameStatus.END) // if playermoving is at the end
            {
                //we'll go again since victoryscreen or error message shown?
                ResetCurrentMapInstance();
                InitMoveSet();
                StartMoveSet();
                //throw new NotImplementedException($"ModelPlayerController.StartMoveSet => _PlayerStatus:{_PlayerStatus}");
            }
            else if (_PlayerStatus == ENUM_PlayerGameStatus.RUNNING) // no need to do anything if already running? maybe disable button to do this
            {
                //throw new NotImplementedException($"ModelPlayerController.StartMoveSet => _PlayerStatus:{_PlayerStatus}");
            }
            else if (_PlayerStatus == ENUM_PlayerGameStatus.STARTED || _PlayerStatus == ENUM_PlayerGameStatus.PAUSED)// if playermoving ongoing
            {
                var PlayerMovementTask = new Task(async () => await PlayerMoveSetTask(), _Ct);
                //PlayerMovementTask.ContinueWith(); //check result
                PlayerMovementTask.Start();
            }
            else
            {
                throw new NotImplementedException($"ModelPlayerController.StartMoveSet => _PlayerStatus:{_PlayerStatus}");
            }
        }

        private void ResetCurrentMapInstance()
        {
            foreach (var item in Player.MapTiles)
            {
                var foundItems = _BackUpMapTiles.FindAll(o => o.X == item.X && o.Y == item.Y);
                if (foundItems.Count != 1)
                    throw new NotImplementedException($"ModelPlayerController.ResetCurrentMapInstance => foundItems.count:{foundItems.Count}");
                item.Color = foundItems.First().Color;
                item.Star = foundItems.First().Star;
            }
            Player.X = _BackUpPlayer.X;
            Player.Y = _BackUpPlayer.Y;
            Player.Direction = _BackUpPlayer.Direction;
            ScoreBoard.ResetScoreBoard();
            PlayerMoves = new List<ControlTileItem>();
            UpdateGameProgress(0, TaskbarItemProgressState.None);
        }

        public ModelPlayerController(PlayerTileItem player, List<ControlTileItem> contItems, List<TileItem> mapTiles, string filePath)
        {
            //save/clone values to bank * for resetting just this stage and progress => player, maptiles, 
            _FilePath = filePath;
            _BackUpPlayer = new PlayerTileItem() { X=player.X, Y=player.Y, Direction=player.Direction };
            _BackUpMapTiles = new List<TileItem>();
            foreach (var item in mapTiles)
            {
                _BackUpMapTiles.Add(new TileItem() { X=item.X, Y=item.Y, Color=item.Color, Star=item.Star });
            }

            Player = player;
            ControlTiles = contItems;
            PlayerMoves = new List<ControlTileItem>();
            ScoreBoard = new ModelScoreBoard(mapTiles.FindAll(o => o.Star == true).Count);
            Player.StarPicketEventHandler += ScoreBoard.IncreaseScore;
        }
    }
}
