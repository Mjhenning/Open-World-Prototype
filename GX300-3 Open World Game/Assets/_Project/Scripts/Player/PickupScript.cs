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
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.magenta);
    }

    public void Pickup () {
        if (Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, 10f, itemMask)) {
            Debug.Log ("Hit " + hit.collider.gameObject);
            if (hit.collider.gameObject.GetComponent<GroundItem>()) {
                
                GroundItem _item = hit.collider.gameObject.GetComponent<GroundItem> ();
                if (_item) {
                    Item item = new Item (_item.item);
                    if (PlayerController.instance.Inventory.AddItem(item, 1)) {
                        Destroy (_item.gameObject);  
                    }   
                }
            }
        }
        
    }
}
