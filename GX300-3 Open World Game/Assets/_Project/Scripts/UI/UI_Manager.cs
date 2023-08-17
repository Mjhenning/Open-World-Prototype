using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    public static UI_Manager instance;
    
    [SerializeField] GameObject InventoryScreen;
    public bool InventoryOpen = false;
    Player_Input Input;
    
    void Awake () {
        instance = this;
    }

    void Start () { //setup in this manner so that the inventory's slots can be created in onAwake
        InventoryScreen.SetActive (false);
        Input = PlayerController.instance.input;
    }

    void Update()
    {
        if (Input.PlayerActions.Inventory.triggered && InventoryOpen == false) {  //if inventory not open and player presses inventory key set it active and tell the system the inventory is open
            InventoryScreen.SetActive (true);
            InventoryOpen = true;
        } else if ((Input.PlayerActions.Inventory.triggered || Input.PlayerActions.Escape.triggered) && InventoryOpen == true) { //if inventory open and player presses inventory key disable it and tell the system the inventory is closed
            InventoryScreen.SetActive (false);
            InventoryOpen = false;
        }
        
        if (InventoryOpen == true) { //if the inventory is open disable the locked cursor and set it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (InventoryOpen == false) { //if the inventory is closed enable the locked cursor and set it invisible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }
}
