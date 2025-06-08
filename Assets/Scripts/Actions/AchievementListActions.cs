using UnityEngine;

public class AchievementListActions : MonoBehaviour
{
    [SerializeField] private GameObject _listContainer;
    [SerializeField] private GameObject _achievementEntryPrefab;

    private void Start()
    {
        PopulateAchievementsList();
    }
    
    /*private void OnEnable()
    {
        // Attaches observer
        DeliveryObserver.DeliveryCommendation += KickstartDeliveryAddition;
        DeliveryObserver.MissionCommendation += KickstartMissionAddition;
        LootClaim.LootCommendation += KickstartLootAddition;
        FishCapture.FishCommendation += KickstartFishAddition;
    }

    private void OnDisable()
    {
        // Detaches observer
        DeliveryObserver.DeliveryCommendation -= KickstartDeliveryAddition;
        DeliveryObserver.MissionCommendation -= KickstartMissionAddition;
        LootClaim.LootCommendation -= KickstartLootAddition;
        FishCapture.FishCommendation -= KickstartFishAddition;
    }*/
    
    private void PopulateAchievementsList()
    {
        foreach (var achievement in AchievementManager.Instance.Achievements)
        {
            var instantiatedAchievement = Instantiate(_achievementEntryPrefab, _listContainer.transform);
            instantiatedAchievement.GetComponent<AchievementListEntryActions>().SetupAchievementListEntry(achievement);
        }
    }
}