// ---------------------------------------------------
// REWORK IN PROGRESS, THIS IS AN OLD UNTOUCHED SCRIPT
// ---------------------------------------------------


using UnityEngine;

public class MouseOver : MonoBehaviour
{
    //Makes sure that the player's mouse is on an island when submitting cargo
    private void OnMouseEnter()
    {
        DeliveryObserver.MouseOverIsland = gameObject.name;
    }

    private void OnMouseExit()
    {
        DeliveryObserver.MouseOverIsland = null;
    }
}
