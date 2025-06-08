[System.Serializable]
public class PersistenceData
{
    public int DeliveriesCompleted, MissionsCompleted, LootFound, FishCaught, MoneyOwned; 
    
    public PersistenceData()
    {
        DeliveriesCompleted = 0;
        MissionsCompleted = 0;
        LootFound = 0;
        FishCaught = 0;
        MoneyOwned = 0;
    }
}

public interface IDataPersistence
{
    void LoadData(PersistenceData data);
    void SaveData(ref PersistenceData data);
}