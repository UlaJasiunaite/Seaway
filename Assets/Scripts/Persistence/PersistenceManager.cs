using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance { get; private set; }
    private List<IDataPersistence> _dataPersistenceObjects;
    private PersistenceFileHandler _persistenceFileHandler;
    private PersistenceData _persistenceData;
    [SerializeField] private string _jsonFileName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        
        _persistenceFileHandler = new PersistenceFileHandler(Application.dataPath, _jsonFileName);
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    
    public void LoadGame()
    {
        LoadFromJsonFile();

        foreach (var persistence in _dataPersistenceObjects)
        {
            persistence.LoadData(_persistenceData);
        }
    }
    
    public void LoadGameDefaults()
    {
        foreach (var persistence in _dataPersistenceObjects)
        {
            persistence.LoadData(_persistenceData);
        }
    }

    public void SaveGame()
    {
        foreach (var persistence in _dataPersistenceObjects)
        {
            persistence.SaveData(ref _persistenceData);
        }

        SaveToJsonFile();
    }

    private void SaveToJsonFile()
    {
        _persistenceFileHandler.SaveData(_persistenceData);
    }

    private void LoadFromJsonFile()
    {
        _persistenceData = _persistenceFileHandler.LoadData();
    }

    public void CreateNewGame()
    {
        _persistenceData = new PersistenceData();
    }

    public bool SaveFileExists()
    {
        return File.Exists(_persistenceFileHandler.GetFullFilePath());
    }

    private static List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        var dataPersistenceObjects = 
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(component => component is IDataPersistence)
                .Cast<IDataPersistence>();
    
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}