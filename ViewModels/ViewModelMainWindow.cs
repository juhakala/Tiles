using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WpfTiles.Common;
using WpfTiles.Model;
using WpfTiles.Model.Notification;

namespace WpfTiles.ViewModels
{
    class ViewModelMainWindow : ViewModelBase
    {
        public ObservableCollection<ModelNotification> NotificationCollection { get; set; } = new ObservableCollection<ModelNotification>();
        public bool IsNotifications { get { return NotificationCollection.Count > 0; } }

        public int CanvasMapAreaWidth { get; set; }
        public int CanvasMapAreaHeight { get; set; }
        public int CanvasMapOffsetX { get; set; } //move to own viewmodelCanvasMap later ??
        public int CanvasMapOffsetY { get; set; } //move to own viewmodelCanvasMap later ??
        public ObservableCollection<TileItem> CanvasMapItems { get; set; } //move to own viewmodelCanvasMap later !!
        public ModelScoreBoard ScoreBoard {get;set; } //move to own viewmodelCanvasMap later !!

        public int CanvasControlAreaWidth { get; set; }
        public int CanvasControlAreaHeight { get; set; }
        public int CanvasControlOffsetX { get; set; }
        public int CanvasControlOffsetY { get; set; }
        public ObservableCollection<TileItem> CanvasControlItems { get; set; }

        public ViewModelControlsItemControl AvailableControlsControlWM { get; set; }
        public ViewModelPlayerController PlayerControllerWM { get; set; }
        public ViewModelLevelSelectorController LevelSelectorControllerWM { get; set; }
        public ViewModelStepsController StepsControllerVM { get; set; }
        public TaskBarController TaskBar { get; set; }

        public ICommand SetSelectedCommand => new RelayCommand(o => SetSelectedMethod());

        private void RemoveOtherSelectionsFromCanvasControlItems(object sender, ControlTileIdEventArgs e)
        {
            var itemsToClear = CanvasControlItems.ToList().FindAll(o => o is ControlTileItem co && co.Selected && co.Id != e.Id).Cast<ControlTileItem>().ToList();
            foreach (var item in itemsToClear)
            {
                item.Selected = false;
            }
        }

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

            NotifyPropertyChanged(nameof(ScoreBoard));
        }
        private void LoadMapFromCont(ModelGameController cont)
        {
            CanvasMapOffsetX = cont.Map.OffsetX;
            CanvasMapOffsetY = cont.Map.OffsetY;
            CanvasMapItems = PopulateCanvasMapItems(cont.MapTiles, cont.PlayerTile);
            ScoreBoard = cont.PlayerController.ScoreBoard;

            CanvasControlOffsetX = cont.Control.OffsetX;
            CanvasControlOffsetY = cont.Control.OffsetY;
            CanvasControlItems = PopulateCanvasControlItems(cont.ControlTiles, cont.NameTiles);

            AvailableControlsControlWM = new ViewModelControlsItemControl(cont);
            PlayerControllerWM = new ViewModelPlayerController(cont);
            LevelSelectorControllerWM = new ViewModelLevelSelectorController(cont);
            StepsControllerVM = new ViewModelStepsController(cont);

            TaskBar = new TaskBarController(cont);
        }

        private void NotificationReceiver(object sender, NotificationEventArgs e)
        {
            NotificationCollection.Add(e.Notification);
            NotifyPropertyChanged(nameof(IsNotifications));
        }

        public ViewModelMainWindow(ModelGameController cont)
        {
            ModelNotificationManager.NotificationHandler += NotificationReceiver;

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
            var itemsToHook = lst.FindAll(o => o is ControlTileItem co).Cast<ControlTileItem>().ToList();
            foreach (var item in itemsToHook)
            {
                item.TileSelectedHandler += RemoveOtherSelectionsFromCanvasControlItems;
            }
            var res = new ObservableCollection<TileItem>(lst);

            return res;
        }
    }
}
