// ---------------------------------------------------
// REWORK IN PROGRESS, THIS IS AN OLD UNTOUCHED SCRIPT
// ---------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DeliveryObserver : MonoBehaviour
{
    private MoneyActions _moneyActions;
    private WorldMapSettings _worldMapSettings;
    private AudioManager _audioManager;
    private NotificationActions _notificationActions;
    
    public List<GameObject> IslandList = new List<GameObject>();        // Entire island list
    public List<Image> ImageList = new List<Image>();                   // Inventory slots
    public List<Sprite> SpriteList = new List<Sprite>();                // Island images

    public List<int> RandomIslandSelection = new List<int>();
    public List<int> RandomIslandDelivery = new List<int>();            // Deliveries above islands random list

    public int NoFreeSlots;                                             // Global slot checker
    public static bool InventorySlotsMax = false;                       // For LootClaim, so that loot doesn't despawn
    public static string MouseOverIsland;                               // Drag & Drop
    public static bool MouseOverIslandValid;                            // Drag & Drop (Makes sure mouse is over an island and not anywhere else when dropping items)
    public static string ItemSlotName;                                  // Drag & Drop

    int InventorySlotCount;                                             // The amount delivery quest given deliveries
    int HowManyDeliveriesLeft;                                          // Keeps track of main how many deliveries are left
    int DeliveryCount = 3;                                              // How many active deliveries in the map above islands

    bool AreDeliveriesActive = true;
    
    int DeliveryMoneySum = 20;
    int LootMoneySum = 30;

    public GameObject Player;
    public static event Action DeliveryCommendation;
    public static event Action MissionCommendation;

    private void OnEnable()
    {
        // Attaches observer
        AcceptDelivery.OnDeliveryClicked += KickstartDelivery;
        LootClaim.LootDelivery += KickstartLootDelivery;
    }

    private void OnDisable()
    {
        // Detaches observer
        AcceptDelivery.OnDeliveryClicked -= KickstartDelivery;
        LootClaim.LootDelivery -= KickstartLootDelivery;
    }

    private void Start()
    {
        _moneyActions = FindFirstObjectByType<MoneyActions>();
        _worldMapSettings = FindFirstObjectByType<WorldMapSettings>();
        _audioManager = FindFirstObjectByType<AudioManager>();
        _notificationActions = FindFirstObjectByType<NotificationActions>();
        
        //Deactivates all inventory images on launch
        for (int i = 0; i < ImageList.Count; i++)
            ImageList[i].gameObject.SetActive(false);
    }

    private void Update()
    {
        // Connected to ItemDrop script, compares inventory slot names and island names.
        // If the island correctly matches, the item poofs away from inventory.
        if(MouseOverIslandValid == true)
        {
            for(int i=0; i < ImageList.Count; i++)
                if (ImageList[i].name == ItemSlotName) // Finds the current slot that's being dragged
                {
                    // Checks if the slot is turned on (in use), doesn't have a loot icon in the corner, the slot's sprite island name matches the island that the sprite is being dragged onto
                    // FOR NORMAL DELIVERIES
                    if (ImageList[i].gameObject.activeSelf == true && ImageList[i].transform.GetChild(0).gameObject.activeSelf == false && ImageList[i].sprite.name == MouseOverIsland)
                    {
                        //Runs through all islands, finds the one that matches the drop off island name (MouseOverIsLand) and measures it's distance from the player, checks if player's location is close to the island
                        for (int j = 0; j < IslandList.Count; j++) //Delete if what
                            if (MouseOverIsland == IslandList[j].name && Vector3.Distance(IslandList[j].gameObject.transform.position, Player.transform.position) < 13) //Delete if what
                            {
                                ImageList[i].gameObject.SetActive(false);
                                _moneyActions.AddMoney(_worldMapSettings.DeliveryPayout);
                                DeliveryCommendation?.Invoke(); //Adds +1 to delivery commendation

                                HowManyDeliveriesLeft--;
                                NoFreeSlots--;
                                if (HowManyDeliveriesLeft == 0)
                                    AreDeliveriesActive = false;
                            }
                            //If player is not close enough, starts a coroutine for notice display
                            else if (MouseOverIsland == IslandList[j].name && Vector3.Distance(IslandList[j].gameObject.transform.position, Player.transform.position) >= 13)
                                _notificationActions.ShowNotification(NotificationTypes.IslandDistance);


                    }
                    // Checks if the slot is turned on (in use), has a loot icon in the corner, and if the slot's sprite island name matches the island that the sprite is being dragged onto
                    // FOR LOOT DELIVERIES
                    else if (ImageList[i].gameObject.activeSelf == true && ImageList[i].transform.GetChild(0).gameObject.activeSelf == true && ImageList[i].sprite.name == MouseOverIsland)
                    {
                        //Runs through all islands, finds the one that matches the drop off island name (MouseOverIsLand) and measures it's distance from the player, checks if player's location is close to the island
                        for (int j = 0; j < IslandList.Count; j++) //Delete if what
                            if (MouseOverIsland == IslandList[j].name && Vector3.Distance(IslandList[j].gameObject.transform.position, Player.transform.position) < 13) //Delete if what
                            {
                                ImageList[i].transform.GetChild(0).gameObject.SetActive(false);
                                ImageList[i].gameObject.SetActive(false);

                                _moneyActions.AddMoney(_worldMapSettings.LootDeliveryPayout);
                                DeliveryCommendation?.Invoke(); //Adds +1 to delivery commendation

                                NoFreeSlots--;
                            }
                            //If player is not close enough, starts a coroutine for notice display
                            else if (MouseOverIsland == IslandList[j].name && Vector3.Distance(IslandList[j].gameObject.transform.position, Player.transform.position) >= 13)
                                _notificationActions.ShowNotification(NotificationTypes.IslandDistance);
                    }
                }

            MouseOverIslandValid = false;
        }

        if (NoFreeSlots == ImageList.Count)
            InventorySlotsMax = true;
        else if (NoFreeSlots < ImageList.Count)
                InventorySlotsMax = false;

        if (AreDeliveriesActive == false)
            RestartDeliveries();
    }

    public void RestartDeliveries()
    {
        for (int i = 0; i < DeliveryCount; i++)
        {
            int RandomNum = UnityEngine.Random.Range(0, IslandList.Count);
            while (RandomIslandDelivery.Contains(RandomNum) == true)
                RandomNum = UnityEngine.Random.Range(0, IslandList.Count);
            RandomIslandDelivery[i] = RandomNum;
        }

        for (int i = 0; i < IslandList.Count; i++)
        {
            if (RandomIslandDelivery.Contains(i) == true)
            {
                IslandList[i].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        MissionCommendation?.Invoke(); //Adds +1 to mission commendation
        AreDeliveriesActive = true;
    }

    public void KickstartDelivery()
    {
        StartCoroutine(DeliveryStart());
    }

    private IEnumerator DeliveryStart()
    {
        // Prevents icons above islands from switching off in case the inventory is full
        if (NoFreeSlots < ImageList.Count)
        {
            // Deactivates delivery icons above islands
            for (int i = 0; i < IslandList.Count; i++)
                IslandList[i].transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);



            // Picks a random amount of deliveries
            InventorySlotCount = UnityEngine.Random.Range(1, 6); // Up to 5 deliveries can be generated
            Debug.Log("Amount of randomly generated slots: " + InventorySlotCount);

            // Chooses delivery islands numbers from the island list
            for (int i = 0; i < InventorySlotCount; i++)
            {
                int RandomNum = UnityEngine.Random.Range(0, IslandList.Count);
                while (RandomIslandSelection.Contains(RandomNum) == true)
                    RandomNum = UnityEngine.Random.Range(0, IslandList.Count);
                RandomIslandSelection[i] = RandomNum;
            }



            // Even if many deliveries were generated, all of them may not fit in
            int HowManySlotsFitIn = 0;

            // Goes through each randomly generated island delivery
            for (int j = 0; j < InventorySlotCount; j++)
            {
                // Runs through each inventory slot to see if it's occupied or not
                for (int BoxCount = 0; BoxCount < ImageList.Count; BoxCount++)
                    // If there are available slots and the current slot is empty, places the delivery in it
                    if (NoFreeSlots < ImageList.Count && ImageList[BoxCount].gameObject.activeSelf == false)
                    {
                        for (int k = 0; k < IslandList.Count; k++)
                            if (RandomIslandSelection[j] == k)
                            {
                                ImageList[BoxCount].GetComponent<Image>().sprite = SpriteList[k];
                                ImageList[BoxCount].gameObject.SetActive(true);

                                NoFreeSlots++;              // Keeps count of global slot availability
                                HowManySlotsFitIn++;        // Keeps count of how many deliveries managed to fit in the slots
                                BoxCount = ImageList.Count; // Ends the cycle
                                _audioManager.PlayEffect(SoundsList.PopSound);

                                yield return new WaitForSeconds(0.6f);
                            }
                    }
                    else if (NoFreeSlots == ImageList.Count) { BoxCount = ImageList.Count; j = InventorySlotCount; } // Ends all cycles
            }

            // Reupdates the actual delivery count, based on how many slots managed to fit in
            Debug.Log("Amount of total slots filled in the inventory: " + NoFreeSlots);
            HowManyDeliveriesLeft = HowManySlotsFitIn;
            InventorySlotCount = HowManySlotsFitIn;
            Debug.Log("Amount of randomly filled slots after conversion: " + InventorySlotCount);
        }
        else _notificationActions.ShowNotification(NotificationTypes.MaxCargoInventory);
    }

    public void KickstartLootDelivery()
    {
        if (NoFreeSlots < ImageList.Count)
        {
            // Generates one random island
            int RandomNum = UnityEngine.Random.Range(0, IslandList.Count - 1); 

            for (int BoxCount = 0; BoxCount < ImageList.Count; BoxCount++)
                if (ImageList[BoxCount].gameObject.activeSelf == false)
                {
                    for (int k = 0; k < IslandList.Count; k++)
                        if (RandomNum == k)
                        {
                            ImageList[BoxCount].GetComponent<Image>().sprite = SpriteList[k];       // Sets sprite
                            ImageList[BoxCount].transform.GetChild(0).gameObject.SetActive(true);   // Activates boxes icon in the corner
                            _audioManager.PlayEffect(SoundsList.PopSound);
                            ImageList[BoxCount].gameObject.SetActive(true);                         // Turns on the inventory box
                            NoFreeSlots++;
                            BoxCount = ImageList.Count;                                             // Ends the cycle
                        }
                }
        }
        //Calls notice
        else _notificationActions.ShowNotification(NotificationTypes.MaxCargoInventory);
    }
}
