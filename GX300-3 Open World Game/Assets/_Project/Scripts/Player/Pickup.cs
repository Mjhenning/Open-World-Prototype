using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.magenta);
        
        if (Physics.Raycast(transform.position, transform.forward,out var hit, 10f)) {
            if (hit.collider.gameObject.GetComponent<Item>() && PlayerController.instance.input.PlayerActions.Interact.triggered) {
                Item _item = hit.collider.gameObject.GetComponent<Item> ();
                PlayerController.instance.Inventory.AddItem (_item.item, 1);
                Destroy (_item.gameObject);
            }
        }
    }
}
