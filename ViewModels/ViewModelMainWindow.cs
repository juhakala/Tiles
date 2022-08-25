using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WpfTiles.Common;
using WpfTiles.Model;

namespace WpfTiles.ViewModels
{
    class ViewModelMainWindow : ViewModelBase
    {
        public int CanvasMapAreaWidth { get; set; }
        public int CanvasMapAreaHeight { get; set; }
        public int CanvasMapOffsetX { get; set; }
        public int CanvasMapOffsetY { get; set; }
        public ObservableCollection<TileItem> CanvasMapItems { get; set; }

        public int CanvasControlAreaWidth { get; set; }
        public int CanvasControlAreaHeight { get; set; }
        public int CanvasControlOffsetX { get; set; }
        public int CanvasControlOffsetY { get; set; }
        public ObservableCollection<TileItem> CanvasControlItems { get; set; }

        public ViewModelControlsItemControl AvailableControlsControlWM { get; set; }
        public ViewModelPlayerController PlayerControllerWM { get; set; }
        public ViewModelLevelSelectorController LevelSelectorControllerWM { get; set; }
        public ViewModelStepsController StepsControllerVM { get; set; }


        public ICommand SetSelectedCommand => new RelayCommand(o => SetSelectedMethod());

        private void SetSelectedMethod()
        {
            var availableSelections = AvailableControlsControlWM.ObsLst.Cast<ControlTileItem>().ToList().FindAll(o => o.Selected);
            var signsAndNames = availableSelections.FindAll(o => o.Sign != -1 || !string.IsNullOrEmpty(o.Name));
            var colors = availableSelections.FindAll(o => string.IsNullOrEmpty(o.Name) && o.Sign == -1);
            if (signsAndNames.Count() > 1 || colors.Count() > 1) { return; }
            var target = CanvasControlItems.ToList().FindAll(o => o is ControlTileItem co && co.Selected).Cast<ControlTileItem>();
            if (target.Count() != 1) { return; }
            if (colors.Count == 1)
                target.First().Color = colors.First().Color;
            if (signsAndNames.Count == 1)
            {
                target.First().Sign = signsAndNames.First().Sign;
                target.First().Name = signsAndNames.First().Name;
            }
        }

        public void LoadMapFromContEvent(object sender, LevelLoaderEvent e)
        {
            LoadMapFromCont(e.mCont);
            NotifyPropertyChanged(nameof(CanvasMapOffsetX));
            NotifyPropertyChanged(nameof(CanvasMapOffsetY));
            NotifyPropertyChanged(nameof(CanvasMapItems));

            NotifyPropertyChanged(nameof(CanvasControlOffsetX));
            NotifyPropertyChanged(nameof(CanvasControlOffsetY));
            NotifyPropertyChanged(nameof(CanvasControlItems));

            NotifyPropertyChanged(nameof(AvailableControlsControlWM));
            NotifyPropertyChanged(nameof(PlayerControllerWM));
            NotifyPropertyChanged(nameof(LevelSelectorControllerWM));
            NotifyPropertyChanged(nameof(StepsControllerVM));
        }
        private void LoadMapFromCont(ModelGameController cont)
        {
            CanvasMapOffsetX = cont.Map.OffsetX;
            CanvasMapOffsetY = cont.Map.OffsetY;
            CanvasMapItems = PopulateCanvasMapItems(cont.MapTiles, cont.PlayerTile);

            CanvasControlOffsetX = cont.Control.OffsetX;
            CanvasControlOffsetY = cont.Control.OffsetY;
            CanvasControlItems = PopulateCanvasControlItems(cont.ControlTiles, cont.NameTiles);

            AvailableControlsControlWM = new ViewModelControlsItemControl(cont);
            PlayerControllerWM = new ViewModelPlayerController(cont);
            LevelSelectorControllerWM = new ViewModelLevelSelectorController(cont);
            StepsControllerVM = new ViewModelStepsController(cont);
        }

        public ViewModelMainWindow(ModelGameController cont)
        {
            CanvasMapAreaWidth = cont.Map.MapAreaWidth;
            CanvasMapAreaHeight = cont.Map.MapAreaHeight;

            CanvasControlAreaWidth = cont.Control.MapAreaWidth;
            CanvasControlAreaHeight = cont.Control.MapAreaHeight;

            LoadMapFromCont(cont);
        }
        private ObservableCollection<TileItem> PopulateCanvasMapItems(List<TileItem> tiles, PlayerTileItem player)
        {
            var res = new ObservableCollection<TileItem>(tiles);
            res.Add(player);
            return res;
        }

        private ObservableCollection<TileItem> PopulateCanvasControlItems(List<ControlTileItem> controlTiles, List<NameTileItem> nameTiles)
        {
            var lst = new List<TileItem>();
            lst.AddRange(nameTiles);
            lst.AddRange(controlTiles);
            var res = new ObservableCollection<TileItem>(lst);
            return res;
        }
    }
}
