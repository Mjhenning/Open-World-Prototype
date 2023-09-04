using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    public static UI_Manager instance;
    
    [Header("Player Inventories")]
    public GameObject InventoryScreen;
    public bool InventoryOpen = false;
    Player_Input Input;

    [Header ("Crafting Inventories")]
    public GameObject CraftingScreen;
    public bool CraftingOpen;

    [Header ("GameOver & Escape")]
    public GameObject EscapeMenu;
    public GameObject GameOverMenu;
    public bool GamePaused = false;
    
    [Header ("Crosshair")]
    public GameObject crosshair;

    [Header ("Inventory Description Box")]
    public GameObject Description_Box;
    public TextMeshProUGUI Description_Text;

    [Header ("Saturation + Health")]
    public float Health;
    public float maxHealth;
    public float Saturation;
    public float maxSaturation;

    public Image HealthBar;
    public Image SaturationBar;
    
    void Awake () {
        instance = this;
    }

    void Start () { //setup in this manner so that the inventory's slots can be created in onAwake
        Time.timeScale = 1;
        Health = maxHealth;
        Saturation = maxSaturation;
        InventoryScreen.SetActive (false);
        Input = PlayerController.instance.input;
        InvokeRepeating ("LoseSaturation", 1f, 1f);
    }

    void Update()
    {
        
        if (Input.PlayerActions.Escape.triggered && CraftingOpen == false && InventoryOpen == false) { //if I press escape and no inventory based screens are open pause the game
            GamePaused = true;
            Time.timeScale = 0f;
            EscapeMenu.SetActive (true);
        }
        
        
        if (Input.PlayerActions.Inventory.triggered && InventoryOpen == false) {  //if inventory not open and player presses inventory key set it active and tell the system the inventory is open
            OpenInventory ();
        } else if ((Input.PlayerActions.Inventory.triggered || Input.PlayerActions.Escape.triggered) && InventoryOpen == true) { //if inventory open and player presses inventory key disable it and tell the system the inventory is closed
            CloseInventory ();
        }

        if (InventoryOpen == true || CraftingOpen == true || GamePaused == true) { //if the inventory is open disable the locked cursor and set it visible
            crosshair.SetActive (false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (InventoryOpen == false || CraftingOpen == false || GamePaused == false) { //if the inventory is closed enable the locked cursor and set it invisible
            crosshair.SetActive (true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Health <= 0) {
            GamePaused = true;
            Time.timeScale = 0;
            GameOverMenu.SetActive (true);
        }
    }

    public void LoseSaturation () { //passively lose saturation
        Saturation -= 1f;
        UpdatedBars();
    }

    public void LoseHealth () { //passively lose health in a fire
        Health -= 8f;
        UpdatedBars ();
    }

    public void GainSaturation (float amount) { //gain/lose saturation from food items that are used
        Saturation += amount;
        UpdatedBars ();
    }

    public void HealHealth (float amount) { //gain/lose health from food items that are used
        Health += amount;
        UpdatedBars();
    }
    

    public void OpenInventory () { //opens the invetory ui
        InventoryScreen.SetActive (true);
        InventoryOpen = true;
    }

    public void CloseInventory () { //closes the invetory ui
        InventoryScreen.SetActive (false);
        InventoryOpen = false;

        Description_Box.SetActive (false);
        Description_Text.text = null;
    }
    
    public void OpenCrafting () { //opens the crafting ui
        InventoryScreen.SetActive (true);
        CraftingScreen.SetActive (true);
        CraftingOpen = true;
    }
    
    public void CloseCrafting () { //closes the crafting ui
        InventoryScreen.SetActive (false);
        CraftingScreen.SetActive (false);
        CraftingOpen = false;
    }

    public void UpdatedBars () { //updates the hunger and saturation bars
        SaturationBar.fillAmount = Saturation / 100f;
        HealthBar.fillAmount = Health / 100f;
    }

    public void PlayerInFire () { //if player in fire start losing health
        InvokeRepeating ("LoseHealth", 0, 2f);
    }

    public void PlayerLeftFire () { //if player left the fire stop losing health
        CancelInvoke ( nameof (LoseHealth));
    }

    public void OnRespawn () { //logic for respawn button
        Time.timeScale = 1;
        GameOverMenu.SetActive (false);
        EscapeMenu.SetActive (false);
        GamePaused = false;
        SceneManager.LoadScene ("Main_Scene");
    }

    public void Quit () { //logic for quit button
        Application.Quit ();
    }

    public void OnResume () { //logic for resume button
        crosshair.SetActive (true);
        Time.timeScale = 1f;
        EscapeMenu.SetActive (false);
        GamePaused = false;
    }
    
}
