// ---------------------------------------------------
// REWORK IN PROGRESS, THIS IS AN OLD UNTOUCHED SCRIPT
// ---------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DeliveryObserver.InventorySlotsMax = false;
        LootClaim.CanDeleteLoot = false;
    }
}
