using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private List<Notification> _notifications;
    
    public Notification GetNotification(NotificationTypes notificationType)
    {
        return _notifications.FirstOrDefault(notification => notification.NotificationType == notificationType);
    }
}

[Serializable]
public class Notification
{
    public NotificationTypes NotificationType;
    public string NotificationText;
}

public enum NotificationTypes
{
    None,
    MaxFishInventory,
    MaxCargoInventory,
    IslandDistance,
}