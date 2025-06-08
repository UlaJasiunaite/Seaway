using UnityEngine;

public class PlayerStatistics : MonoBehaviour, IDataPersistence
{
    public int DeliveriesCompleted;
    public int MissionsCompleted;
    public int LootFound;
    public int FishCaught;
    public int MoneyOwned;

    private void Awake()
    {
        if (!PersistenceManager.Instance.SaveFileExists())
        {
            PersistenceManager.Instance.CreateNewGame();
            PersistenceManager.Instance.LoadGameDefaults();
        }
        else
        {
            PersistenceManager.Instance.LoadGame();
        }
    }

    public void LoadData(PersistenceData data)
    {
        DeliveriesCompleted = data.DeliveriesCompleted;
        MissionsCompleted = data.MissionsCompleted;
        LootFound = data.LootFound;
        FishCaught = data.FishCaught;
        MoneyOwned = data.MoneyOwned;
    }
    
    public void SaveData(ref PersistenceData data)
    {

        data.DeliveriesCompleted = DeliveriesCompleted;
        data.MissionsCompleted = MissionsCompleted;
        data.LootFound = LootFound;
        data.FishCaught = FishCaught;
        data.MoneyOwned = MoneyOwned;
    }
}
