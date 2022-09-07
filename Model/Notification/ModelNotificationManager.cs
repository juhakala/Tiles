using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Common;

namespace WpfTiles.Model.Notification
{
    static class ModelNotificationManager
    {
        static public event EventHandler<NotificationEventArgs> NotificationHandler;
        
        static public void RaiseNotification(ModelNotification notification)
        {
            EventHandler<NotificationEventArgs> handler = NotificationHandler;
            handler?.Invoke(new object(), new NotificationEventArgs() { Notification=notification });
        }
    }
}
