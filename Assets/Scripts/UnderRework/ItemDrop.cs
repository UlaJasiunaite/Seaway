// ---------------------------------------------------
// REWORK IN PROGRESS, THIS IS AN OLD UNTOUCHED SCRIPT
// ---------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour, IDropHandler
{
    public AudioSource Pop;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel,Input.mousePosition))
        {
            //Checks if the cargo was indeed dropped on an island
            if(DeliveryObserver.MouseOverIsland != null)
            {
                //Debug.Log(DeliveryObserver.MouseOverIsland);
                //Assigns the name of the inventory slot
                DeliveryObserver.ItemSlotName = gameObject.transform.GetChild(1).gameObject.name;
                Debug.Log("Item Slot Name: " + DeliveryObserver.ItemSlotName);
                //Runs update in DeliveryObserver code, to see if correct cargo was dropped on a correct island
                DeliveryObserver.MouseOverIslandValid = true;
            }
            //Pop.Play();
        }
    }
}
