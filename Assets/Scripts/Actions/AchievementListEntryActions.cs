using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementListEntryActions : MonoBehaviour
{
    [SerializeField] private Image _achievementImage;
    [SerializeField] private TextMeshProUGUI _achievementTitle;
    [SerializeField] private TextMeshProUGUI _achievementTargetValue;
    [SerializeField] private GameObject _achievementCompletionOverlay;

    public void SetupAchievementListEntry(Achievement achievement)
    {
        _achievementImage.sprite = achievement.Icon;
        _achievementTitle.text = achievement.TitleText;
        
        var currentAchievementValue = AchievementManager.Instance.GetAchievementCurrentValue(achievement.Type);
        _achievementTargetValue.text = currentAchievementValue + " / " + achievement.TargetValue;
        
        if (achievement.IsCompleted)
            _achievementCompletionOverlay.SetActive(true);
    }
}
