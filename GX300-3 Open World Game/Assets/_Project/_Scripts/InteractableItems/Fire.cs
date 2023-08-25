using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    public List<GroundItem> ItemsInFire;
    GroundItem currentItemAdded;
    GroundItem currentItemExit;
    void OnTriggerEnter (Collider other) { //if something enters the trigger
        if (other.GetComponent<PlayerController>()) { //and it has the player's script on it
            UI_Manager.instance.PlayerInFire (); //tell the system to make the player take damage
        }
        currentItemAdded = other.GetComponent<GroundItem> (); //if it has a ground item
        currentItemAdded.InsideFire (); //tell that grounditem it's inside the fire
        ItemsInFire.Add (currentItemAdded); //add it to the list of objects inside the fire
    }

    void OnTriggerExit (Collider other) { //if something exits the trigger
        if (other.GetComponent<PlayerController>()) { //and it has the player's script on it
            UI_Manager.instance.PlayerLeftFire (); //tell the system to stop the player from taking damage
        }
        currentItemExit = other.GetComponent<GroundItem> (); //if it has a grounditem
        currentItemExit.OutsideFire ();//tell that item's script it's outside the fire
        ItemsInFire.Remove (currentItemExit); //and remove it from the list
    }
    
}
