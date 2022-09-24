using System.Windows.Shell;
using WpfTiles.Common;

namespace WpfTiles.Model
{
    class TaskBarController : NotifyPropertyChangedBase
    {
        private TaskbarItemProgressState _ProgressState;
        public TaskbarItemProgressState ProgressState
        {
            get { return _ProgressState; }
            set
            {
                if (value != _ProgressState)
                {
                    _ProgressState = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private double _ProgressValue;
        public double ProgressValue
        {
            get { return _ProgressValue; }
            set
            {
                if (value != _ProgressValue)
                {
                    _ProgressValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public TaskBarController(ModelGameController cont)
        {
            cont.PlayerController.ProgressUpdated += UpdateProgress;
        }
        public void UpdateProgress(object sender, ProgressUpdatedEventArgs e)
        {
            ProgressValue = e.Value;
            ProgressState = e.State;
        }
    }
}
