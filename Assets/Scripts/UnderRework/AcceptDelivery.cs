// ---------------------------------------------------
// REWORK IN PROGRESS, THIS IS AN OLD UNTOUCHED SCRIPT
// ---------------------------------------------------


using UnityEngine;
using System;

public class AcceptDelivery : MonoBehaviour
{
    public static event Action OnDeliveryClicked;
    public static event Action OnFishingPortClicked;

    private FishingManager _fishingManager;

    private void Start()
    {
        _fishingManager = FindFirstObjectByType<FishingManager>();
    }

    public void OnDeliveryClick()
    {
        OnDeliveryClicked?.Invoke();
    }

    public void OnFishingPortClick()
    {
        _fishingManager.SellFish();
    }
}
