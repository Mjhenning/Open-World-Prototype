using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;

    public InventoryObject Inventory;
    
    public float speed;
    public float jumpforce;
    public float DownForce;
    Rigidbody rb;
    public LayerMask groundMask;

    public bool isGrounded;
    public bool isJumping;

    public Player_Input input; //used to access player_input actionmap
    
    void Awake () {
        instance = this;
        input = new Player_Input ();
    }
    

    void Start () {

        groundMask = LayerMask.GetMask ("Terrain");
        rb = GetComponent<Rigidbody> ();

        input.Enable (); //Enables the Player_input actions.
    }
    
    void FixedUpdate () {
        Vector2 MoveInput = input.PlayerActions.Movement.ReadValue<Vector2>();
        
        float zMovement = MoveInput.y * speed; // creates a float variable holding the value from pressing W/S * speed
        float xMovement = MoveInput.x * speed; // creates a float variable holding the value from pressing A/D * speed
        
        Vector3 forwardMovement = transform.forward * zMovement; //Creates a new vector 3 holding the transformation of the forward movement according to the rotation of the object
        Vector3 sidewaysMovement = transform.right * xMovement; //Creates a new vector 3 holding the transformation of the sideways movement according to the rotation of the object

        Vector3 movement = forwardMovement + sidewaysMovement;
        rb.velocity = new Vector3 (movement.x, rb.velocity.y, movement.z);
        
    }

    void Update () {

        isGrounded = Physics.Raycast(CameraController.instance.transform.position, Vector3.down, 11f, groundMask);
        Debug.DrawRay(CameraController.instance.transform.position, Vector3.down * 11f, Color.red);
        
        if (isGrounded && input.PlayerActions.Jump.triggered) {

            isJumping = true;
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse); 
        }

        if (!isGrounded && isJumping) {
            isGrounded = false;
            rb.AddForce(Vector3.down * DownForce, ForceMode.Force);
        }
        
    }

    public void OnTriggerEnter (Collider other) {
        var item = other.GetComponent<Item> ();

        if (item) {
            Inventory.AddItem (item.item, 1);
            Destroy (other.gameObject);
        }
    }

    private void OnApplicationQuit () {
        Inventory.Container.Clear ();
    }
}
    

