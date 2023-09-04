using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Interact : MonoBehaviour {
    public static Pickup_Interact instance;
    
    public Transform DropPoint;
    [SerializeField] GameObject ItemsGroup;
    Crafting_Table CraftingHit;
    public GameObject flamesPrefab;
    public LayerMask playermask;
    public Vector3 DropForce;

    void Awake() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.magenta); //used to visualize raycast
        if (PlayerController.instance.input.PlayerActions.Mine.WasPressedThisFrame()) { //if left click
            if (Physics.Raycast (transform.position, transform.forward, out RaycastHit _hit, 10f, ~playermask)) {
                Debug.Log (_hit.collider.gameObject);
                Destroyable_Obj _tree = _hit.collider.gameObject.GetComponent<Destroyable_Obj> ();
                if (HotbarManager.instance.HotbarInventory.Container.Slots[HotbarManager.instance.CurrentSlot].item.Name == "Axe") { //hits the tree
                    HitObject (_tree);
                }
            }
        }

        if (PlayerController.instance.input.PlayerActions.Interact.WasPressedThisFrame()) { //if f was pressed this frame and it hit a crafting table call the opencrafting function on that crafting table and store CraftingHit (so we know what crafting table was last interacted with)
            Physics.Raycast (transform.position, transform.forward, out RaycastHit hit, 10f, ~playermask);
            if (hit.transform.GetComponent<Crafting_Table>()) {
                CraftingHit = hit.transform.GetComponent<Crafting_Table> ();
                hit.transform.GetComponent<Crafting_Table> ().OpenCrafting();
            }
        }

        if (PlayerController.instance.input.PlayerActions.Escape.WasPressedThisFrame()) { //if player presses escape while crafting close down the crafting menu of the previously hit crafting table
            if (UI_Manager.instance.CraftingOpen) {
                CraftingHit.transform.GetComponent<Crafting_Table> ().CloseCrafting ();
            }
        }

    }

    public void Pickup () {
        if (Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, 10f, ~playermask)) { //if something is hit on the itemmask
            Debug.Log ("Hit " + hit.collider.gameObject); //debug what was hit
            if (hit.collider.gameObject.GetComponent<GroundItem>()) { //if the hit item has a grounditem
                
                GroundItem _item = hit.collider.gameObject.GetComponent<GroundItem> (); //grab the item off of the item on the ground
                if (_item) { //if there is an item
                    Item item = new Item (_item.item); //create an item that can be added into the inventory
                    if (PlayerController.instance.Inventory.AddItem(item, 1)) { //add the item to the inventory and only if it was added
                        Water_Behavior.instance.ItemsRb.Remove (_item.GetComponent<Rigidbody> ());
                        Destroy (_item.gameObject);//destory the item on the ground
                    }   
                }
            }
        }
    }

    public void Drop (ItemObject item) {  //used to set the dropped item properties and to actually drop the item infront of the player
        
        Transform player = GetComponent<CameraController> ().player;
        
        GameObject _item = Instantiate (item.groundItem, DropPoint.position, transform.rotation, ItemsGroup.transform);
        _item.name = item.data.Name;
        if (!_item.GetComponent<GroundItem>()) { //if it doesn't have a grounditem script
            GroundItem _itemScript = _item.AddComponent<GroundItem> ();
            _itemScript.enabled = true;
            _itemScript.item = item;
            _itemScript.FlamesPrefab = flamesPrefab;
            _itemScript.currentstate = item.state;
            _itemScript.OnCreate ();
            Rigidbody itemrb = _item.GetComponent<Rigidbody> ();
            itemrb.isKinematic = false;
            itemrb.AddRelativeForce (DropForce, ForceMode.Impulse);
        } else if (_item.GetComponent<GroundItem>()) { //if it has a grounditem script
            GroundItem _itemScript = _item.GetComponent<GroundItem> ();
            _itemScript.enabled = true;
            _itemScript.item = item;
            _itemScript.FlamesPrefab = flamesPrefab;
            _itemScript.currentstate = item.state;
            _itemScript.OnCreate ();
            Rigidbody itemrb = _item.GetComponent<Rigidbody> ();
            itemrb.isKinematic = false;
            itemrb.AddRelativeForce (DropForce, ForceMode.Impulse);
        }
    }
    
    public void Drop (ItemObject item, Transform droppoint) {  //used to set the dropped item properties and to actually drop the item 
        GameObject _item = Instantiate (item.groundItem, droppoint.position, Quaternion.identity, ItemsGroup.transform);
        _item.name = item.data.Name;
        if (!_item.GetComponent<GroundItem>()) { //if it doesn't have a grounditem script
            GroundItem _itemScript = _item.AddComponent<GroundItem> ();
            _itemScript.enabled = true;
            _itemScript.item = item;
            _itemScript.FlamesPrefab = flamesPrefab;
            _itemScript.currentstate = item.state;
            _itemScript.OnCreate ();
            Rigidbody itemrb = _item.GetComponent<Rigidbody> ();
            itemrb.isKinematic = false;
        } else if (_item.GetComponent<GroundItem>()) { //if it has a grounditem script
            GroundItem _itemScript = _item.GetComponent<GroundItem> ();
            _itemScript.enabled = true;
            _itemScript.item = item;
            _itemScript.FlamesPrefab = flamesPrefab;
            _itemScript.currentstate = item.state;
            _itemScript.OnCreate ();
            Rigidbody itemrb = _item.GetComponent<Rigidbody> ();
            itemrb.isKinematic = false;
        }
    }

    public void HitObject (Destroyable_Obj _item) { //used to count up the amount of hits on a tree and if 3 call the destroy obj script

        if (_item != null) {
            Debug.Log ("Hit");
            _item.hit_amount++;
            
            if (_item.hit_amount== 3) {
                _item.DestroyObj ();
            }
        }
    }
    
}
