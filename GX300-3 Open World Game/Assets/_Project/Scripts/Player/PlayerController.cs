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
    }

    void OnInteract (InputAction.CallbackContext obj) {
        PickupScript.instance.Pickup (); //fire pickup logic
    }

    void FixedUpdate () {
        if (!UI_Manager.instance.InventoryOpen) { //player can only move if the inventory isn't currently open
            Vector2 MoveInput = input.PlayerActions.Movement.ReadValue<Vector2>();
        
            float zMovement = MoveInput.y * speed; // creates a float variable holding the value from pressing W/S * speed
            float xMovement = MoveInput.x * speed; // creates a float variable holding the value from pressing A/D * speed
        
            Vector3 forwardMovement = transform.forward * zMovement; //Creates a new vector 3 holding the transformation of the forward movement according to the rotation of the object
            Vector3 sidewaysMovement = transform.right * xMovement; //Creates a new vector 3 holding the transformation of the sideways movement according to the rotation of the object

            Vector3 movement = forwardMovement + sidewaysMovement;
            rb.velocity = new Vector3 (movement.x, rb.velocity.y, movement.z);  
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


        if (Input.GetKeyDown(KeyCode.K)) { //Testing the saving of game
            SaveLoadManager.instance.SaveGame (0);
        }

        if (Input.GetKeyDown(KeyCode.L)) { //Testing the loading of game
            SaveLoadManager.instance.LoadGame (0);
        }
        
    }
    

    private void OnApplicationQuit () { //when player quits the application clear both the hotbar and inventory lists
        Inventory.Container.Clear();
        Hotbar.Container.Clear();
    }
}
    

