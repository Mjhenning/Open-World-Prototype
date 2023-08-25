using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {
    
    [Header("Player Specifics")]
    public static CameraController instance;
    public Transform player;
    public float sensitivity;
    public float height = 10f;
    
    Quaternion currentRotation;

    [SerializeField] public Quaternion CurrentPlayerViewRotation;
    void Awake () {
        instance = this;
    }

    public void CallOnLoad () {
        currentRotation = CurrentPlayerViewRotation;
    }
    
    void Start () { //on start disable cursor and change lockstate
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }
    
    void LateUpdate() { //Used to follow player mouse position to determine rotation of first person view
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 lookInput = PlayerController.instance.input.PlayerActions.LookAround.ReadValue<Vector2>();
            float mouseX = lookInput.x * sensitivity;
            float mouseY = lookInput.y * sensitivity;
        
            currentRotation.x += mouseX;
            currentRotation.y += mouseY;
            currentRotation.y = Mathf.Clamp(currentRotation.y, -90f, 90f);

            Quaternion rotation = Quaternion.Euler(-currentRotation.y, currentRotation.x, 0f);
            transform.rotation = rotation;
            CurrentPlayerViewRotation = currentRotation;
        }
        
        player.rotation = Quaternion.Euler (0, currentRotation.x, 0);
        
        Vector3 position = player.position; //Allows the camera to move around accordingly to player movement.
        position.y = player.position.y + height; //Allows camera to be the correct height instead of player's feat
        transform.position = position; //Changes camera position
    }
    
    void Update () { //used to update the sensitivity
             sensitivity = PlayerPrefs.GetFloat ("MouseSens", 1f);
    }
}