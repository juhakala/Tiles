using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Model;

namespace WpfTiles.ViewModels
{
    class ViewModelControlsItemControl
    {
        public int CanvasAvailableControlOffsetX { get; set; }
        public int CanvasAvailableControlOffsetY { get; set; }

        public ObservableCollection<TileItem> ObsLst { get; set; }

        public ViewModelControlsItemControl(ModelGameController cont)
        {
            var lst = new List<TileItem>(cont.AvailableControlTiles);
            ObsLst = new ObservableCollection<TileItem>(lst);
            CanvasAvailableControlOffsetX = cont.AvailableControlT.OffsetX;
            CanvasAvailableControlOffsetY = cont.AvailableControlT.OffsetY;

        }
    }
}
