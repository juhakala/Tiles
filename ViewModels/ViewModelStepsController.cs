using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfTiles.Common;
using WpfTiles.Model;
using WpfTiles.Model.Parser;

namespace WpfTiles.ViewModels
{
    class ViewModelStepsController
    {
        public int Width { get; set; } = (int)ENUM_TileSizes.MapBackground;
        public ICommand StopPlayerAnimCommand => new RelayCommand(o => NotImplementedYet(), o => NotImplementedYetCan());
        public ICommand PausePlayerAnimCommand => new RelayCommand(o => NotImplementedYet(), o => NotImplementedYetCan());
        public ICommand StepBackPlayerAnimCommand => new RelayCommand(o => NotImplementedYet(), o => NotImplementedYetCan());
        public ICommand StepForwardPlayerAnimCommand => new RelayCommand(o => StepForwardPlayerAnimMethod());
        public event EventHandler StepForwardPlayerAnimEvent;
        private void StepForwardPlayerAnimMethod()
        {
            EventHandler handler = StepForwardPlayerAnimEvent;
            handler?.Invoke(this, new EventArgs());
        }

        public ICommand StartPlayerAnimCommand => new RelayCommand(o => StartPlayerAnimMethod());
        public event EventHandler StartPlayerAnimEvent;
        private void StartPlayerAnimMethod()
        {
            EventHandler handler = StartPlayerAnimEvent;
            handler?.Invoke(this, new EventArgs());
        }

        public ICommand InstantPlayerAnimCommand => new RelayCommand(o => NotImplementedYet(), o => NotImplementedYetCan());

        private void NotImplementedYet()
        {
            throw new NotImplementedException();
        }
        private bool NotImplementedYetCan()
        {
            return false;
        }
        public ViewModelStepsController(ModelGameController cont)
        {
            StartPlayerAnimEvent += cont.StartPlayerAnimEvent;
            StepForwardPlayerAnimEvent += cont.PlayerController.PlayerMoveStepForwardOne;
        }
    }
}
