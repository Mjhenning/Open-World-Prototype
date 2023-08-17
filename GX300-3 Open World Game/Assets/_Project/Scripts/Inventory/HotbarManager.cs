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
    [SerializeField]InventoryObject HotbarInventory;
    [FormerlySerializedAs ("HotbarSlotButtons")] public List<Button> HotbarSlots = new List<Button> ();
    
    [Header("Held Item Info")]
    public int selectedItemId;
    public int CurrentSlot;
    public GameObject HeldItem;

    Player_Input Input;

    void Awake () {
        instance = this;
    }


    void Start () { 

        Input = PlayerController.instance.input; //sets input to the same the playercontroller uses

        //assigns hotbars 1-7 to their respected inputs
        Input.HotbarActions.Hotbar1.performed += Hotbar1;
        Input.HotbarActions.Hotbar2.performed += Hotbar2;
        Input.HotbarActions.Hotbar3.performed += Hotbar3;
        Input.HotbarActions.Hotbar4.performed += Hotbar4;
        Input.HotbarActions.Hotbar5.performed += Hotbar5;
        Input.HotbarActions.Hotbar6.performed += Hotbar6;
        Input.HotbarActions.Hotbar7.performed += Hotbar7;
        
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
        CurrentSlot = slot;
        UpdateSelectedSlot ();
    }

    public void UpdateSelectedSlot () {
        selectedItemId = HotbarInventory.Container.Slots[CurrentSlot].item.Id; //grabs id of selected object

        if (HeldItem.activeSelf == false && selectedItemId >= 0) { //only show the selected item if it has any ID besides the empty slot id (which is -1)
            HeldItem.SetActive (true);
        } else if (selectedItemId < 0) {
            HeldItem.SetActive (false);
        }

        HotbarSlots[CurrentSlot].Select (); //used to highlight the corresponding slot

        if (selectedItemId >= 0) { //only change if item != nothing
            //changes the mesh of the selected item
            HeldItem.GetComponent<MeshFilter>().mesh = HotbarInventory.database.items[selectedItemId].groundItem.GetComponent<MeshFilter>().sharedMesh;
            HeldItem.GetComponent<MeshRenderer>().material = HotbarInventory.database.items[selectedItemId].groundItem.GetComponent<MeshRenderer>().sharedMaterial;   
        }
    }
}
