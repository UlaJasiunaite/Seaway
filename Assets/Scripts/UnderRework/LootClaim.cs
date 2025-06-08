// ---------------------------------------------------
// REWORK IN PROGRESS, THIS IS AN OLD UNTOUCHED SCRIPT
// ---------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LootClaim : MonoBehaviour
{
    private DeliveryObserver _deliveryObserver;
    private FishingManager _fishingManager;
    
    public Canvas LootCanvas;
    public GameObject Player;
    bool IsPlayerInProximity = false;
    public static bool CanDeleteLoot = false;
    int LootMoneySum = 15;

    private RandomEventManager _randomEventManager;

    public static event Action LootDelivery;
    public static event Action LootMoneyPopUp;
    public static event Action LootFish;
    public static event Action LootCommendation;
    int RandomEvent;
    bool LootActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _randomEventManager = FindFirstObjectByType<RandomEventManager>();
        _fishingManager = FindFirstObjectByType<FishingManager>();
        Player = FindFirstObjectByType<ShipMovementActions>().gameObject;
        //Picks a random event for this loot
        RandomEvent = UnityEngine.Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        //Checks distance between player and this loot and turns the UI on accordingly
        if (Vector3.Distance(transform.position, Player.transform.position) < 4)
        {
            //Starts UI fade in
            if (IsPlayerInProximity == false)
            {
                //In case of player moving in and out of the area, stops the previous coroutine
                StopCoroutine(FadeUI(true));
                StartCoroutine(FadeUI(false));
                IsPlayerInProximity = true;
            }

            //Executes random event reward, loot active makes sure the player can't extort the key during destroy
            if (Input.GetKeyDown(KeyCode.E) && LootActive == false)
            {
                if (RandomEvent == 1)
                {
                    LootActive = true;
                    //MoneyActions.MoneyAdditionFactor = LootMoneySum;
                    LootMoneyPopUp?.Invoke();
                    DeletionProcess();
                }
                else if (RandomEvent == 2)
                {
                    LootDelivery?.Invoke();
                    if (DeliveryObserver.InventorySlotsMax == false)
                    {
                        LootActive = true;
                        DeletionProcess();
                    }
                }
                else if (RandomEvent == 3)
                {
                    var randomFishNum = UnityEngine.Random.Range(2, 5);
                    _fishingManager.AddFishToInventory(randomFishNum);
                    if(_fishingManager.IsFishInventoryFull() == false)
                    {
                        LootActive = true;
                        DeletionProcess();
                    }
                }
            }
        }
        else
        {
            //Starts UI fade out
            if (IsPlayerInProximity == true)
            {
                //In case of player moving in and out of the area, stops the previous coroutine
                StopCoroutine(FadeUI(false));
                StartCoroutine(FadeUI(true));
                IsPlayerInProximity = false;
            }
        }
    }

    void DeletionProcess()
    {
        LootCommendation?.Invoke(); //Adds +1 to loot commendation
        //Sets deleted event's position to zero
        _randomEventManager.DestroyRandomEvent(gameObject);
    }

    private IEnumerator FadeUI(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime * 4)
            {
                LootCanvas.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
            //Ensure that the UI is fully off
            LootCanvas.GetComponent<CanvasGroup>().alpha = 0;
            yield return null;
        }
        // fade from transparent to opaque
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime * 4)
            {
                LootCanvas.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
            //Ensure that UI is fully on
            LootCanvas.GetComponent<CanvasGroup>().alpha = 1;
            yield return null;
        }
    }
}
