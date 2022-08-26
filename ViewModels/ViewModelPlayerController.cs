﻿using System;
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
                _Offset++;
                var tmpItem = e.Item.Clone<PlaybackTileItem>();
                tmpItem.X = 0;
                if (e.Index == -1)
                    tmpItem.VideoOffset = PlayerMovesCollection.Count - _Offset;
                else
                    tmpItem.VideoOffset = e.Index - _Offset;
                PlayerMovesCollection.Add(tmpItem);
            }
            else if (e.ChangeType == ENUM_PlayerMovesCollectionChangedType.REMOVE)
            {
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
        private void PlayerMoveMade(object sender, PlayerMoveMadeEventArgs e)
        {
            foreach (var item in PlayerMovesCollection)
            {
                item.VideoOffset -= 1;
            }
        }
    }
}
