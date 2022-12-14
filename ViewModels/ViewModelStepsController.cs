using System;
using System.Windows.Input;
using WpfTiles.Common;
using WpfTiles.Model;

namespace WpfTiles.ViewModels
{
    class ViewModelStepsController
    {
        public ICommand StopPlayerAnimCommand => new RelayCommand(o => StopPlayerAnimMethod());
        public event EventHandler StopPlayerAnimEvent;
        private void StopPlayerAnimMethod()
        {
            EventHandler handler = StopPlayerAnimEvent;
            handler?.Invoke(this, new EventArgs());
        }

        public ICommand PausePlayerAnimCommand => new RelayCommand(o => PausePlayerAnimMethod());
        public event EventHandler PausePlayerAnimEvent;
        private void PausePlayerAnimMethod()
        {
            EventHandler handler = PausePlayerAnimEvent;
            handler?.Invoke(this, new EventArgs());
        }

        public ICommand StepBackPlayerAnimCommand => new RelayCommand(o => NotImplementedYet(), o => NotImplementedYetCan());
        public ICommand StepForwardPlayerAnimCommand => new RelayCommand(o => StepForwardPlayerAnimMethod());
        public event EventHandler StepForwardPlayerAnimEvent;
        private void StepForwardPlayerAnimMethod()
        {
            EventHandler handler = StepForwardPlayerAnimEvent;
            handler?.Invoke(this, new EventArgs());
        }

        public ICommand StartPlayerAnimCommand => new RelayCommand(o => StartPlayerAnimMethod()); //disable, if ModelPlayerController._PlayerStatus == running?
        public event EventHandler StartPlayerAnimEvent;
        private void StartPlayerAnimMethod()
        {
            EventHandler handler = StartPlayerAnimEvent;
            handler?.Invoke(this, new EventArgs());
        }

        public ICommand InstantPlayerAnimCommand => new RelayCommand(o => InstantPlayerAnimMethod(), o => NotImplementedYetCan());
        public event EventHandler InstantPlayerAnimEvent;
        private void InstantPlayerAnimMethod()
        {
            EventHandler handler = InstantPlayerAnimEvent;
            handler?.Invoke(this, new EventArgs());
        }

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
            PausePlayerAnimEvent += cont.PlayerController.PausePlayerMoveStepsEvent;
            StopPlayerAnimEvent += cont.PlayerController.StopPlayerMoveStepsEvent;
            InstantPlayerAnimEvent += cont.PlayerController.InstantPlayerMoveStepsEvent;
        }
    }
}
