using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Common;
using WpfTiles.Model;

namespace WpfTiles.ViewModels
{
    class ViewModelPlayerController : ViewModelBase
    {
        private int _Offset;
        public ObservableCollection<PlaybackTileItem> PlayerMovesCollection { get; set; }
        public int ControlHalfWay { get; set; }
        public ViewModelPlayerController(ModelGameController cont)
        {
            ControlHalfWay = 200; //calculate player tile move colletion X offset and update so that current move is in middle 
            PlayerMovesCollection = new ObservableCollection<PlaybackTileItem>();
            cont.PlayerController.PlayerMovesCollectionChanged += UpdatePlayerMovesCollection;
            cont.PlayerController.PlayerMoveMadeEventHandler += PlayerMoveMade;
        }
        private void UpdatePlayerMovesCollection(object sender, PlayerMovesCollectionChangedEventArgs e)
        {
            if (e.ChangeType == ENUM_PlayerMovesCollectionChangedType.ADD)
            {
                var tmpItem = e.Item.Clone<PlaybackTileItem>();
                tmpItem.X = 0;
                if (e.Index == -1)
                    tmpItem.VideoOffset = PlayerMovesCollection.Count - _Offset; //add to end
                else
                {
                    tmpItem.VideoOffset = e.Index - _Offset; //add to middle from f[int]call
                    foreach (var item in PlayerMovesCollection)
                    {
                        if (item.VideoOffset >= tmpItem.VideoOffset)
                            item.VideoOffset++; //move later items to right
                    }
                }
                PlayerMovesCollection.Add(tmpItem);
            }
            else if (e.ChangeType == ENUM_PlayerMovesCollectionChangedType.REMOVE)
            {
                if (e.Index == -1)
                {
                    PlayerMovesCollection.RemoveAt(PlayerMovesCollection.Count - 1); // from end, not used atm since will be handled with index place
                }
                else if (e.Index < PlayerMovesCollection.Count)
                {
                    var tmpItem = PlayerMovesCollection.FirstOrDefault(o => o.Id == e.Id);
                    if (tmpItem == null)
                    {
                        throw new InvalidOperationException($"ViewModelPlayerController.UpdatePlayerMovesCollection => tmpItem:{tmpItem}");
                    }
                    foreach (var item in PlayerMovesCollection)
                    {
                        if (item.VideoOffset > tmpItem.VideoOffset)
                            item.VideoOffset--;
                    }
                    PlayerMovesCollection.Remove(tmpItem); //from middle, should be fcall first name tile replacement

                }
                else
                {
                    //error should not be possible?
                    throw new InvalidOperationException($"ViewModelPlayerController.UpdatePlayerMovesCollection => e.type:{e.ChangeType}, e.index:{e.Index}, PlayermovesCollection.count:{PlayerMovesCollection.Count}");
                }
                //PlayerMovesCollection.Remove(e.Item.Clone()); // need some indexat or something to determine what object to remove
            }
            else if (e.ChangeType == ENUM_PlayerMovesCollectionChangedType.HISTORY_FORWARD)
            {
                throw new NotImplementedException($"ViewModelPlayerController.UpdatePlayerMovesCollection:'{e.ChangeType}'");
            }
            else if (e.ChangeType == ENUM_PlayerMovesCollectionChangedType.HISTORY_BACKWARD)
            {
                throw new NotImplementedException($"ViewModelPlayerController.UpdatePlayerMovesCollection:'{e.ChangeType}'");
            }
            else
            {
                throw new NotImplementedException($"ViewModelPlayerController.UpdatePlayerMovesCollection:'{e.ChangeType}'");
            }
        }
        private void PlayerMoveMade(object sender, PlayerMovesCollectionChangedEventArgs e)
        {
            if (ENUM_PlayerMovesCollectionChangedType.NORMAL_FORWARD == e.ChangeType) // && ENUM_PlayerMovesCollectionChangedType.HISTORY_FORWARD == e.ChangeType
            {
                _Offset++;
                foreach (var item in PlayerMovesCollection)
                {
                    item.VideoOffset -= 1;
                }
            }
            
        }
    }
}
