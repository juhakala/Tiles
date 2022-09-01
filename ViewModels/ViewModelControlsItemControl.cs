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
    class ViewModelControlsItemControl
    {
        public int CanvasAvailableControlOffsetX { get; set; }
        public int CanvasAvailableControlOffsetY { get; set; }

        public ObservableCollection<TileItem> ObsLst { get; set; }

        private void RemoveOtherSelectionsFromCanvasControlItems(object sender, ControlTileIdEventArgs e)
        {
            var matchItem = (ControlTileItem?)ObsLst.FirstOrDefault(o => o is ControlTileItem co && co.Id == e.Id);
            if (matchItem != null)
            {
                if (string.IsNullOrEmpty(matchItem.Name) && matchItem.Sign == -1) // color selection tile
                {
                    var itemsToClear = ObsLst.ToList().FindAll(o => o is ControlTileItem co && co.Selected && co.Id != e.Id && string.IsNullOrEmpty(co.Name) && co.Sign == -1).Cast<ControlTileItem>().ToList();
                    foreach (var item in itemsToClear)
                    {
                        item.Selected = false;
                    }
                }
                else // sign and name tiles
                {
                    var itemsToClear = ObsLst.ToList().FindAll(o => o is ControlTileItem co && co.Selected && co.Id != e.Id && (!string.IsNullOrEmpty(co.Name) || co.Sign != -1)).Cast<ControlTileItem>().ToList();
                    foreach (var item in itemsToClear)
                    {
                        item.Selected = false;
                    }
                }
            }
        }

        public ViewModelControlsItemControl(ModelGameController cont)
        {
            var lst = new List<TileItem>(cont.AvailableControlTiles);
            var itemsToHook = lst.FindAll(o => o is ControlTileItem co).Cast<ControlTileItem>().ToList();
            foreach (var item in itemsToHook)
            {
                item.TileSelectedHandler += RemoveOtherSelectionsFromCanvasControlItems;
            }
            ObsLst = new ObservableCollection<TileItem>(lst);
            CanvasAvailableControlOffsetX = cont.AvailableControlT.OffsetX;
            CanvasAvailableControlOffsetY = cont.AvailableControlT.OffsetY;

        }
    }
}
