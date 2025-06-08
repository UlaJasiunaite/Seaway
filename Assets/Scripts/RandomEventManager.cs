using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomEventManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _currentRandomEventsInWorld;
    [SerializeField] private GameObject _randomEventContainer;
    [SerializeField] private List<RandomEvent> _randomEvents;
    private int _currentRandomEventCount;
    private WorldMapSettings _worldMapSettings;
    private IslandManager _islandManager;

    private void Start()
    {
        _worldMapSettings = FindFirstObjectByType<WorldMapSettings>();
        _islandManager = FindFirstObjectByType<IslandManager>();
        
        InstantiateInitialEvents();
        InvokeRepeating(nameof(AutoInstantiateRandomEvent), 120f, 120f);
    }
    
    private Vector3 GetRandomEventLocation()
    {
        var eventPlacementZone = _worldMapSettings.ObjectPlacementZone;
        return new Vector3(
            Random.Range(eventPlacementZone.x,
                eventPlacementZone.y), 0f, 
            Random.Range(eventPlacementZone.x,eventPlacementZone.y));
    }

    private void InstantiateInitialEvents()
    {
        _currentRandomEventCount = Random.Range(_worldMapSettings.MinRandomEventCountOnStart, _worldMapSettings.MaxRandomEventCount);
        Debug.LogError("Event Sum: " + _currentRandomEventCount);

        for (var i = 0; i < _currentRandomEventCount; i++)
        {
            InstantiateRandomEvent();
        }
    }

    private void InstantiateRandomEvent()
    {
        var maxAttempts = 100; // Add a maximum number of attempts
        var attempts = 0;
        bool locationIsValid;
        Vector3 randomLocation;
    
        do
        {
            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogError("Failed to find valid location for random event after " + maxAttempts + " attempts");
                return;
            }

            randomLocation = GetRandomEventLocation();
            locationIsValid = true;

            if (Vector3.Distance(randomLocation, _worldMapSettings.WorldMapCenter) <= _worldMapSettings.ObjectDistance)
            {
                locationIsValid = false;
                continue;
            }
                    
            foreach (var island in _islandManager.IslandList)
            {
                if (Vector3.Distance(randomLocation, island.IslandObject.transform.position) <= _worldMapSettings.ObjectDistance)
                {
                    locationIsValid = false;
                    break;
                }
            }
        
            if (!locationIsValid) continue;
        
            // Check distance from other events
            foreach (var randomEventInWorld in _currentRandomEventsInWorld)
            {
                if (Vector3.Distance(randomLocation, randomEventInWorld.transform.position) <= _worldMapSettings.ObjectDistance)
                {
                    locationIsValid = false;
                    break;
                }
            }
        }
        while (!locationIsValid);
    
        var randomEventNumber = Random.Range(0, _randomEvents.Count);
        var instantiatedEvent = Instantiate(_randomEvents[randomEventNumber].EventPrefab, randomLocation, Quaternion.identity, _randomEventContainer.transform);
        _currentRandomEventsInWorld.Add(instantiatedEvent);
    }

    private void AutoInstantiateRandomEvent()
    {
        if (_currentRandomEventCount < _worldMapSettings.MaxRandomEventCount)
        {
            InstantiateRandomEvent();
        }
    }

    public void DestroyRandomEvent(GameObject eventToDestroy)
    {
        _currentRandomEventsInWorld.Remove(eventToDestroy);
        
        Destroy(eventToDestroy);
    }
}

[Serializable]
public class RandomEvent
{
    public RandomEventType EventType;
    public GameObject EventPrefab;
}

public enum RandomEventType
{
    None,
    FishEvent,
    LootEvent,
}