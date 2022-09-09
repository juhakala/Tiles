using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Model;
using WpfTiles.Model.Notification;
using WpfTiles.Common;
using System.Windows.Input;

namespace WpfTiles.ViewModels
{
    class ViewModelNotificationsControl : NotifyPropertyChangedBase
    {
        public ObservableCollection<ModelNotification> NotificationCollection { get; set; } = new ObservableCollection<ModelNotification>();
        public bool IsNotifications { get { return NotificationCollection.Count > 0; } }

        public ModelNotification SelectedNotification
        {
            get { return NotificationCollection.Count > 0 ? NotificationCollection[0] : new ModelNotification() { Text="empty", Title="EMPTY" }; }
        }
        public ICommand OkCommand => new RelayCommand(o => OkCommandMethod());

        private void OkCommandMethod()
        {
            if (NotificationCollection.Count > 0)
                NotificationCollection.RemoveAt(0);
            NotifyPropertyChanged(nameof(IsNotifications));
            NotifyPropertyChanged(nameof(SelectedNotification));
        }

        private void NotificationReceiver(object sender, NotificationEventArgs e)
        {
            NotificationCollection.Add(e.Notification); //title, text
            NotifyPropertyChanged(nameof(SelectedNotification));
            NotifyPropertyChanged(nameof(IsNotifications));
        }
        public ViewModelNotificationsControl(ModelGameController cont)
        {
            ModelNotificationManager.NotificationHandler += NotificationReceiver;
        }
    }
}
