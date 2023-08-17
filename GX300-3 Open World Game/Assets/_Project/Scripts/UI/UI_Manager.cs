using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    public static UI_Manager instance;
    
    [SerializeField] GameObject InventoryScreen;
    public bool InventoryOpen = false;
    
    void Awake () {
        instance = this;
    }

    void Start () {
        InventoryScreen.SetActive (false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && InventoryOpen == false) {
            InventoryScreen.SetActive (true);
            InventoryOpen = true;
        } else if (Input.GetKeyDown(KeyCode.I) && InventoryOpen == true) {
            InventoryScreen.SetActive (false);
            InventoryOpen = false;
        }
        
        if (InventoryOpen == true) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (InventoryOpen == false) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }
}
