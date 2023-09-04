using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;

    [Header("Player Inventories")]
    public InventoryObject Inventory;
    public InventoryObject Hotbar;
    InventoryObject useItem;

    [Header ("Crafting Inventories")]
    public InventoryObject Crafting_table;
    
    [Header("Player Specifications")]
    public float speed;
    public float jumpforce;
    public float DownForce;
    Rigidbody rb;

    [Header("Jump / Ground checks")]
    public bool isGrounded;
    public bool isJumping;
    public LayerMask groundMask;
    
    [Header("Player Info")]
    public Vector3 currenPlayerPos;
    public Player_Input input; //used to access player_input actionmap
    
    void Awake () {
        instance = this;
        input = new Player_Input ();
    }

    public void CallOnLoad () {
        transform.position = currenPlayerPos; //Used to load back player saved pos
    }
    

    void Start () {

        groundMask = LayerMask.GetMask ("Terrain");
        rb = GetComponent<Rigidbody> ();

        input.Enable (); //Enables the Player_input actions.

        input.PlayerActions.Interact.started += OnInteract; //when key is pressed
        input.PlayerActions.Use.started += OnUse;
    }

    void OnUse (InputAction.CallbackContext obj) { //on use call the Hotbarmanager's use function to use the current item in the selected slot
        if (HotbarManager.instance.HotbarInventory.Container.Slots[HotbarManager.instance.CurrentSlot].itemobject is FoodObject _foodObject) {
            HotbarManager.instance.Use (HotbarManager.instance.HotbarInventory.Container.Slots[HotbarManager.instance.CurrentSlot].itemobject);
        } ;
    }

    void OnInteract (InputAction.CallbackContext obj) { //on interact call the pickup interact script's pcikup funtion
        Pickup_Interact.instance.Pickup (); //fire pickup logic
    }

    void FixedUpdate () {
        
        rb.AddForce(Physics.gravity * rb.mass);
        
        if (!UI_Manager.instance.InventoryOpen || !UI_Manager.instance.CraftingOpen || !UI_Manager.instance.GamePaused) { //player can only move if the inventory isn't currently open
            rb.freezeRotation = false;
            Vector2 MoveInput = input.PlayerActions.Movement.ReadValue<Vector2>();
        
            float zMovement = MoveInput.y * speed; // creates a float variable holding the value from pressing W/S * speed
            float xMovement = MoveInput.x * speed; // creates a float variable holding the value from pressing A/D * speed
        
            Vector3 forwardMovement = transform.forward * zMovement; //Creates a new vector 3 holding the transformation of the forward movement according to the rotation of the object
            Vector3 sidewaysMovement = transform.right * xMovement; //Creates a new vector 3 holding the transformation of the sideways movement according to the rotation of the object

            Vector3 movement = forwardMovement + sidewaysMovement;
            rb.velocity = new Vector3 (movement.x, rb.velocity.y, movement.z);  
        } else if (UI_Manager.instance.InventoryOpen || UI_Manager.instance.CraftingOpen || UI_Manager.instance.GamePaused) {
            rb.freezeRotation = true; //used to stop the player from falling over when menus are open
        }
    }

    void Update () {

        currenPlayerPos = transform.position;
        
        isGrounded = Physics.Raycast(CameraController.instance.transform.position, Vector3.down, 11f, groundMask);
        Debug.DrawRay(CameraController.instance.transform.position, Vector3.down * 11f, Color.red);
        
        if (isGrounded && input.PlayerActions.Jump.triggered) { //adds a jumpforce upwards

            isJumping = true;
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse); 
        }

        if (!isGrounded && isJumping) { //adds a gravity downforce
            isGrounded = false;
            rb.AddForce(Vector3.down * DownForce, ForceMode.Force);
        }


        // if (Input.GetKeyDown(KeyCode.K)) { //Testing the saving of game
        //     SaveLoadManager.instance.SaveGame (0);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.L)) { //Testing the loading of game
        //     SaveLoadManager.instance.LoadGame (0);
        // }
        
    }
    

    void OnApplicationQuit () { //when player quits the application clear both the hotbar and inventory lists
        Inventory.Container.Clear();
        Hotbar.Container.Clear();
    }
}
    

