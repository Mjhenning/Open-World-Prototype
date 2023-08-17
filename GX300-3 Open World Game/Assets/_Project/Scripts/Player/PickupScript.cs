using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour {

    public static PickupScript instance;
    [SerializeField] LayerMask itemMask;

    void Awake() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.magenta); //used to visualize raycast
    }

    public void Pickup () {
        if (Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, 10f, itemMask)) { //if something is hit on the itemmask
            Debug.Log ("Hit " + hit.collider.gameObject); //debug what was hit
            if (hit.collider.gameObject.GetComponent<GroundItem>()) { //if the hit item has a grounditem
                
                GroundItem _item = hit.collider.gameObject.GetComponent<GroundItem> (); //grab the item off of the item on thee ground
                if (_item) { //if there is an item
                    Item item = new Item (_item.item); //create an item that can be added into the inventory
                    if (PlayerController.instance.Inventory.AddItem(item, 1)) { //add the item to the inventory and only if it was added
                        Destroy (_item.gameObject);//destory the item on the ground
                    }   
                }
            }
        }
        
    }
}
