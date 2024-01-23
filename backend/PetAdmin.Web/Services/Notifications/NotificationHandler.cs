using System.Collections.Generic;
using System.Linq;

namespace PetAdmin.Web.Services
{
    public class NotificationHandler
    {
        private List<string> _notificationList;

        public NotificationHandler()
        {
            _notificationList = new List<string>();
        }

        public void Raise(string notificacao)
        {
            _notificationList.Add(notificacao);
        }

        public List<NotificationModel> GetNotifications()
        {
            var notificationModelList = new List<NotificationModel>();

            foreach (var notification in _notificationList)
            {
                notificationModelList.Add(new NotificationModel("Erro", notification));
            }

            return notificationModelList;
        }

        public bool HasNotification()
        {
            return _notificationList.Any();
        }
    }

    public class NotificationModel
    {
        public NotificationModel(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; }
        public string Message { get; }
    }
}
