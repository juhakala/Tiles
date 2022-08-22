using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Model;

namespace WpfTiles.ViewModels
{
    class ViewModelPlayerController : ViewModelBase
    {
        public ObservableCollection<ControlTileItem> PlayerMovesCollection { get; set; }
        public int ControlHalfWay { get; set; }
        public ViewModelPlayerController(ModelGameController cont)
        {
            ControlHalfWay = 200; //calculate player tile move colletion X offset and update so that current move is in middle 
            PlayerMovesCollection = new ObservableCollection<ControlTileItem>();
            cont.PlayerController.PlayerMovesCollectionChanged += UpdatePlayerMovesCollection;
        }
        private void UpdatePlayerMovesCollection(object sender, PlayerMovesCollectionChangedEventArgs e)
        {
            if (e.ChangeType == ENUM_PlayerMovesCollectionChangedType.ADD)
            {
                PlayerMovesCollection.Add(e.Item.Clone());
            }
            else if (e.ChangeType == ENUM_PlayerMovesCollectionChangedType.REMOVE)
            {
                //PlayerMovesCollection.Remove(e.Item.Clone()); // need some indexat or something to determine what object to remove?
            }
            else
            {
                throw new NotImplementedException($"ViewModelPlayerController.UpdatePlayerMovesCollection:'{e.ChangeType}'");
            }
        }
    }
}
