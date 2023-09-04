using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour {

    [Header("Hotbar Info")]
    public static HotbarManager instance;
    public InventoryObject HotbarInventory;
    public List<Button> HotbarSlots = new List<Button> ();
    
    [Header("Held Item Info")]
    public int selectedItemId;
    public int CurrentSlot;
    public int previousSlot;
    public GameObject HeldItem;

    public Color Selected;
    public Color Default;

    Player_Input Input;

    void Awake () {
        instance = this;
    }


    void Start () { 

        Input = PlayerController.instance.input; //sets input to the same the playercontroller uses

        //assigns hotbars 1-7 to their respected inputs
        Input.HotbarActions.Hotbar1.started += Hotbar1;
        Input.HotbarActions.Hotbar2.started += Hotbar2;
        Input.HotbarActions.Hotbar3.started += Hotbar3;
        Input.HotbarActions.Hotbar4.started += Hotbar4;
        Input.HotbarActions.Hotbar5.started += Hotbar5;
        Input.HotbarActions.Hotbar6.started += Hotbar6;
        Input.HotbarActions.Hotbar7.started += Hotbar7;
        
        foreach (GameObject slot in GetComponent<Static_Player_Interface>().slots) { //grabs all of the slots from the hotbar interface and reads them into my own list
            HotbarSlots.Add (slot.GetComponent<Button> ());
        }
        
        //updates the selected slot to slot 0 to highlight the correct slot and to show the correct item in-hand
        UpdateSelectedSlot ();
    }
    
    //Sets the selection to the corresponding hotbar selection based on which key was pressed
    void Hotbar1 (InputAction.CallbackContext obj) {
        SetSelection (0);
    }
    void Hotbar2 (InputAction.CallbackContext obj) {
        SetSelection (1);
    }
    void Hotbar3 (InputAction.CallbackContext obj) {
        SetSelection (2);
    }
    void Hotbar4 (InputAction.CallbackContext obj) {
        SetSelection (3);
    }
    void Hotbar5 (InputAction.CallbackContext obj) {
        SetSelection (4);
    }
    void Hotbar6 (InputAction.CallbackContext obj) {
        SetSelection (5);
    }
    void Hotbar7 (InputAction.CallbackContext obj) {
        SetSelection (6);
    }

    public void SetSelection (int slot) { //sets the selected slot then calls the UpdateSelectedSlot() function to update it visually
        SetPreviousSlot (CurrentSlot);
        CurrentSlot = slot;
        UpdateSelectedSlot ();
    }

    public void SetPreviousSlot (int slot) { //sets the previous slot that was selected
        previousSlot = slot;
    }

    public void UpdateSelectedSlot () {
        HotbarSlots[CurrentSlot].targetGraphic.color = Selected; //used to highlight the corresponding slot
        HotbarSlots[previousSlot].targetGraphic.color = Default; //used to change the color of the previous slot
        
        selectedItemId = HotbarInventory.Container.Slots[CurrentSlot].item.Id; //grabs id of selected object

        if (HeldItem.activeSelf == false && selectedItemId >= 0) { //only show the selected item if it has any ID besides the empty slot id (which is -1)
            HeldItem.SetActive (true);
        } else if (selectedItemId < 0) {
            HeldItem.SetActive (false);
        }

        if (selectedItemId >= 0) { //only change if item != nothing
            //changes the mesh of the selected item
            HeldItem.GetComponent<MeshFilter>().mesh = HotbarInventory.database.Items[selectedItemId].groundItem.GetComponent<MeshFilter>().sharedMesh;
            HeldItem.GetComponent<MeshRenderer>().material = HotbarInventory.database.Items[selectedItemId].groundItem.GetComponent<MeshRenderer>().sharedMaterial;
        }
    }
    
    public void Use (ItemObject item) { //script to use an item when right clicked (specifically food items at this stage)
        if (item is FoodObject _foodItem) { //if the itemobject is a foodobject
            if (UI_Manager.instance.Health < UI_Manager.instance.maxHealth || UI_Manager.instance.Saturation < UI_Manager.instance.maxSaturation || _foodItem.RestoreHealthValue < 0 || _foodItem.RestoreSaturationValue < 0) { //and health isn't max, saturation isn't max or it restores a negative value
                _foodItem.OnUse (); //use the item
                if (HotbarInventory.Container.Slots[CurrentSlot].itemamount >1) { //if more than ine item only take one away
                    HotbarInventory.Container.Slots[CurrentSlot].UpdateSlot (item.data, HotbarInventory.Container.Slots[CurrentSlot].itemamount -= 1);
                } else if (HotbarInventory.Container.Slots[CurrentSlot].itemamount == 1) { //else delete the item out of the inventory
                    HotbarInventory.Container.Slots[CurrentSlot].RemoveItem ();
                }
            }
        }

        UpdateSelectedSlot (); //update the held item and the inventory
    }
}
