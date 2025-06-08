using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationActions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private CanvasGroup _notificationCanvasGroup;
    [SerializeField] private NotificationManager _notificationManager;

    private void Start()
    {
        _notificationManager = FindFirstObjectByType<NotificationManager>();
    }

    public void ShowNotification(NotificationTypes notificationType)
    {
        _notificationText.text = _notificationManager.GetNotification(notificationType).NotificationText;
        StartCoroutine(FadeNotification());
    }
    
    public void ShowNotification(string notificationText)
    {
        _notificationText.text = notificationText;
        StartCoroutine(FadeNotification());
    }

    private IEnumerator FadeNotification()
    {
        while (_notificationCanvasGroup.alpha < 1f)
        {
            _notificationCanvasGroup.alpha += Time.deltaTime * 2;
            yield return null;
        }
        _notificationCanvasGroup.alpha = 1f;
        
        yield return new WaitForSeconds(1f);
        
        while (_notificationCanvasGroup.alpha > 0f)
        {
            _notificationCanvasGroup.alpha -= Time.deltaTime * 2;
            yield return null;
        }
        _notificationCanvasGroup.alpha = 0f;
    }
}