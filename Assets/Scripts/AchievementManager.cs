using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }
    public List<Achievement> Achievements;
    private PlayerStatistics _playerStatistics;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        _playerStatistics = FindFirstObjectByType<PlayerStatistics>();
        
        CheckForAchievementCompletions();
    }
    
    public void CheckForAchievementCompletions()
    {
        foreach (var achievement in Achievements)
        {
            if (!achievement.IsCompleted)
            {
                var currentValue = GetAchievementCurrentValue(achievement.Type);
                if (currentValue >= achievement.TargetValue)
                {
                    achievement.IsCompleted = true;
                }
            }
        }
    }
    
    public int GetAchievementCurrentValue(AchievementType type)
    {
        if (_playerStatistics == null) return 0;

        switch (type)
        {
            case AchievementType.DeliveriesCompleted:
                return _playerStatistics.DeliveriesCompleted;
            case AchievementType.MissionsCompleted:
                return _playerStatistics.MissionsCompleted;
            case AchievementType.LootFound:
                return _playerStatistics.LootFound;
            case AchievementType.FishCaught:
                return _playerStatistics.FishCaught;
            default:
                return 0;
        }
    }
}

[Serializable]
public class Achievement
{
    public AchievementType Type;
    public Sprite Icon;
    public string TitleText;
    public int TargetValue;
    public bool IsCompleted;
}

public enum AchievementType
{
    None,
    DeliveriesCompleted,
    MissionsCompleted,
    LootFound,
    FishCaught,
}