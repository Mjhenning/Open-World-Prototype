using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting_Table : MonoBehaviour
{
    public void OpenCrafting () {
        UI_Manager.instance.OpenCrafting ();
    }

    public void CloseCrafting () {
        UI_Manager.instance.CloseCrafting ();
    }

    public void OnTriggerExit (Collider other) { //auto close crafting inventory if player tries walking away
        if (other.GetComponent<PlayerController>() && UI_Manager.instance.CraftingOpen) {
            CloseCrafting (); //close crafting ui 
        }
    }
}
