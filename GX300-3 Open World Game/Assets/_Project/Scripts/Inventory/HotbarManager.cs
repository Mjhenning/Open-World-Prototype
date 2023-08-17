using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour {

    public static HotbarManager instance;

    [SerializeField]InventoryObject HotbarInventory;
    public int selectedItemId;
    public int CurrentSlot;
    [FormerlySerializedAs ("HotbarSlotButtons")] public List<Button> HotbarSlots = new List<Button> ();

    public Player_Input Input;
    public GameObject HeldItem;

    void Awake () {
        instance = this;
    }


    void Start () {

        Input = PlayerController.instance.input;

        Input.HotbarActions.Hotbar1.performed += Hotbar1;
        Input.HotbarActions.Hotbar2.performed += Hotbar2;
        Input.HotbarActions.Hotbar3.performed += Hotbar3;
        Input.HotbarActions.Hotbar4.performed += Hotbar4;
        Input.HotbarActions.Hotbar5.performed += Hotbar5;
        Input.HotbarActions.Hotbar6.performed += Hotbar6;
        Input.HotbarActions.Hotbar7.performed += Hotbar7;
        
        foreach (GameObject slot in GetComponent<Static_Player_Interface>().slots) {
            HotbarSlots.Add (slot.GetComponent<Button> ());
        }
        
        UpdateSelectedSlot ();
    }
    
    void Hotbar1 (InputAction.CallbackContext obj) {
        Debug.Log ("Received Input");
        SetSelection (0);
    }
    void Hotbar2 (InputAction.CallbackContext obj) {
        Debug.Log ("Received Input");
        SetSelection (1);
    }
    void Hotbar3 (InputAction.CallbackContext obj) {
        Debug.Log ("Received Input");
        SetSelection (2);
    }
    void Hotbar4 (InputAction.CallbackContext obj) {
        Debug.Log ("Received Input");
        SetSelection (3);
    }
    void Hotbar5 (InputAction.CallbackContext obj) {
        Debug.Log ("Received Input");
        SetSelection (4);
    }
    void Hotbar6 (InputAction.CallbackContext obj) {
        Debug.Log ("Received Input");
        SetSelection (5);
    }
    void Hotbar7 (InputAction.CallbackContext obj) {
        Debug.Log ("Received Input");
        SetSelection (6);
    }

    public void SetSelection (int slot) {
        CurrentSlot = slot;
        UpdateSelectedSlot ();
    }

    public void UpdateSelectedSlot () {
        selectedItemId = HotbarInventory.Container.Slots[CurrentSlot].item.Id; //grabs id of selected object

        if (HeldItem.activeSelf == false && selectedItemId >= 0) {
            HeldItem.SetActive (true);
        } else if (selectedItemId < 0) {
            HeldItem.SetActive (false);
        }

        HotbarSlots[CurrentSlot].Select ();

        if (selectedItemId >= 0) { //only change if item != nothing
            //changes the mesh of the selected item
            HeldItem.GetComponent<MeshFilter>().mesh = HotbarInventory.database.items[selectedItemId].groundItem.GetComponent<MeshFilter>().sharedMesh;
            HeldItem.GetComponent<MeshRenderer>().material = HotbarInventory.database.items[selectedItemId].groundItem.GetComponent<MeshRenderer>().sharedMaterial;   
        }
    }
}
