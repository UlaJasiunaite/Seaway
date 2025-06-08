// ---------------------------------------------------
// REWORK IN PROGRESS, THIS IS AN OLD UNTOUCHED SCRIPT
// ---------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour
{
    public GameObject Player;
    bool IsPlayerInProximity = false;

    private void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < 15)
        {
            //If player is close to an island, activates delivery button
            if (IsPlayerInProximity == false)
            {
                this.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
                this.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
                IsPlayerInProximity = true;
            }
        }
        else
        {
            //If player is far from an island, deactivates delivery button
            if (IsPlayerInProximity == true)
            {
                this.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                this.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
                IsPlayerInProximity = false;
            }
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
