using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingManager : MonoBehaviour
{
    private MoneyActions _moneyActions;
    private WorldMapSettings _worldMapSettings;
    private NotificationActions _notificationActions;
    
    [Header("Fish Inventory UI:")]
    [SerializeField] private TextMeshProUGUI _fishInventoryCapacityText;
    [SerializeField] private List<Image> _fishInventoryImageList;

    [SerializeField] private int _maxFishCount;
    private int _currentFishCount;
    private bool _isFishBeingSold;

    private void Start()
    {
        _moneyActions = FindFirstObjectByType<MoneyActions>();
        _worldMapSettings = FindFirstObjectByType<WorldMapSettings>();
        _notificationActions = FindFirstObjectByType<NotificationActions>();
        
        DisableAllFishInventoryVisuals();
    }

    private void DisableAllFishInventoryVisuals()
    {
        foreach (var image in _fishInventoryImageList)
        {
            image.gameObject.SetActive(false);
        }
    }
    
    public bool IsFishInventoryFull()
    {
        return _currentFishCount == _maxFishCount;
    }

    public void AddFishToInventory(int fishToAdd)
    {
        if (IsFishInventoryFull()) return;

        _currentFishCount += fishToAdd;

        if (_currentFishCount > _maxFishCount)
            _currentFishCount = _maxFishCount;
        
        _fishInventoryCapacityText.text = _currentFishCount + " / " + _maxFishCount;
        
        UpdateFishInventoryVisuals();
    }

    public void SellFish()
    {
        if (_isFishBeingSold == false)
        {
            _moneyActions.AddMoney(_worldMapSettings.FishPayout * _currentFishCount);
            StartCoroutine(RemoveFishFromInventory());
        }
    }

    private IEnumerator RemoveFishFromInventory()
    {
        _isFishBeingSold = true;

        while (_currentFishCount > 0)
        {
            _currentFishCount--;
            _fishInventoryCapacityText.text = _currentFishCount + " / " + _maxFishCount;
            
            UpdateFishInventoryVisuals();

            yield return new WaitForSeconds(1f);
        }

        _isFishBeingSold = false;
    }
    
    private void UpdateFishInventoryVisuals()
    {
        DisableAllFishInventoryVisuals();
        
        for (var i = 1; i <= _currentFishCount; i++)
        {
            if (i % 2 == 1)
            {
                var imageIndex = (i - 1) / 2;
                if (imageIndex < _fishInventoryImageList.Count)
                {
                    _fishInventoryImageList[imageIndex].gameObject.SetActive(true);
                }
            }
        }
    }
}