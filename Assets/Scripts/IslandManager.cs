using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IslandManager : MonoBehaviour
{
    public List<Island> IslandList;
    private WorldMapSettings _worldMapSettings;

    int DeliveryPortCount = 3;
    int FishPortCount = 2;
    int RandomEventCountMax = 8;

    private void Awake()
    {
        _worldMapSettings = FindFirstObjectByType<WorldMapSettings>();
        
        AssignDeliveryPorts();
        AssignFishPorts();
        SetupIslandPlacements();
    }
    
    private void AssignDeliveryPorts()
    {
        for (var i = 0; i < _worldMapSettings.DeliveryPortCount; i++)
        {
            var randomPortNumber = Random.Range(0, IslandList.Count);
            
            while (IslandList[randomPortNumber].IsDeliveryPort || IslandList[randomPortNumber].IsFishPort)
                randomPortNumber = Random.Range(0, IslandList.Count);

            IslandList[randomPortNumber].IsDeliveryPort = true;
            
            IslandList[i].IslandObject.transform.Find("Canvas").GetChild(0).gameObject.SetActive(true);
        }
    }

    private void AssignFishPorts()
    {
        for (var i = 0; i < _worldMapSettings.FishPortCount; i++)
        {
            var randomFishPortNumber = Random.Range(0, IslandList.Count);
            
            while (IslandList[randomFishPortNumber].IsDeliveryPort || IslandList[randomFishPortNumber].IsDeliveryPort)
                randomFishPortNumber = Random.Range(0, IslandList.Count);

            IslandList[randomFishPortNumber].IsFishPort = true;
            
            IslandList[i].IslandObject.transform.Find("Canvas").GetChild(1).gameObject.SetActive(true);
        }
    }

    private List<GameObject> GetAllPlacedIslands()
    {
        var placedIslands = new List<GameObject>();
        
        foreach (var island in IslandList)
        {
            if (island.IsPlaced)
                placedIslands.Add(island.IslandObject);
        }

        return placedIslands;
    }

    private Vector3 GetRandomIslandLocation()
    {
        var islandPlacementZone = _worldMapSettings.ObjectPlacementZone;
        return new Vector3(
            Random.Range(islandPlacementZone.x,
            islandPlacementZone.y), -0.55f, 
            Random.Range(islandPlacementZone.x,islandPlacementZone.y));
    }
    
    private void SetupIslandPlacements()
    {
        foreach (var island in IslandList)
        {
            var randomLocation = GetRandomIslandLocation();
            bool locationIsValid;
            
            do
            {
                locationIsValid = true;
                
                if (Vector3.Distance(randomLocation, _worldMapSettings.WorldMapCenter) <= _worldMapSettings.ObjectDistance)
                {
                    locationIsValid = false;
                    randomLocation = GetRandomIslandLocation();
                }
            } while (!locationIsValid);

            if (GetAllPlacedIslands().Count > 0)
            {
                do
                {
                    locationIsValid = true;
                    
                    foreach (var placedIsland in GetAllPlacedIslands())
                    {
                        if (Vector3.Distance(randomLocation, placedIsland.transform.position) <= _worldMapSettings.ObjectDistance)
                        {
                            locationIsValid = false;
                            randomLocation = GetRandomIslandLocation();
                            break;
                        }
                    }
                }
                while (!locationIsValid);
            }

            island.IslandObject.transform.position = randomLocation;
            island.IsPlaced = true;
        }
    }
}

[Serializable]
public class Island
{
    public GameObject IslandObject;
    public bool IsDeliveryPort;
    public bool IsFishPort;
    public bool IsPlaced;
}