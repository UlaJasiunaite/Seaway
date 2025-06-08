using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class PersistenceFileHandler
{
    private readonly string _jsonFileDirectoryPath;
    private readonly string _jsonFileName;

    public PersistenceFileHandler(string jsonFileDirectoryPath, string jsonFileName)
    {
        _jsonFileDirectoryPath = jsonFileDirectoryPath;
        _jsonFileName = jsonFileName;
    }

    public PersistenceData LoadData()
    {
        var fullPath = GetFullFilePath();

        PersistenceData loadedData = null;
        if (File.Exists(fullPath))
        {
            using var stream = new FileStream(fullPath, FileMode.Open);
            using var reader = new StreamReader(stream);
            var dataToLoad = reader.ReadToEnd();

            loadedData = JsonUtility.FromJson<PersistenceData>(dataToLoad);
        }

        return loadedData;
    }

    public void SaveData(PersistenceData data)
    {
        var fullPath = GetFullFilePath();

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        var dataToStore = JsonUtility.ToJson(data, true);

        using var stream = new FileStream(fullPath, FileMode.Create);
        using var writer = new StreamWriter(stream);
        writer.Write(dataToStore);
    }

    public string GetFullFilePath()
    {
        return Path.Combine(_jsonFileDirectoryPath, _jsonFileName);
    }
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> _keys = new List<TKey>();
    [SerializeField] private List<TValue> _values = new List<TValue>();

    private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var pair in _dictionary)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        _dictionary.Clear();

        if (_keys.Count != _values.Count)
        {
            return;
        }

        for (var i = 0; i < _keys.Count; i++)
        {
            _dictionary.Add(_keys[i], _values[i]);
        }
    }

    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return _dictionary.Remove(key);
    }
}