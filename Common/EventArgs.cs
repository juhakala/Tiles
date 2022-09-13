using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;
using WpfTiles.Model;
using WpfTiles.Model.Notification;

namespace WpfTiles.Common
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
    class LevelLoaderEvent : EventArgs
    {
        public ModelGameController mCont { get; set; }
        public LevelLoaderEvent(ModelGameController cont)
        {
            mCont = cont;
        }
    }
    class PlayerMovesCollectionChangedEventArgs : EventArgs
    {
        public ENUM_PlayerMovesCollectionChangedType ChangeType { get; set; }
        public ControlTileItem Item { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
    }

    public class ControlTileIdEventArgs : EventArgs
    {
        public int Id { get; set; }
    }
    public class PlayerPicketStarEventArgs : EventArgs
    {

    }
    public class ProgressUpdatedEventArgs : EventArgs
    {
        public double Value { get; set; }
        public TaskbarItemProgressState State { get; set; }
    }
    public class NotificationEventArgs : EventArgs
    {
        public ModelNotification Notification { get; set; }
    }
    public class LevelPassedEventArgs : EventArgs
    {
        public string FilePath { get; set; }
    }
    public class ChangeMapToEventArgs : EventArgs
    {
        public string FilePath { get; set; }
        public ChangeMapToEventArgs(string path)
        {
            FilePath = path;
        }
    }
}
