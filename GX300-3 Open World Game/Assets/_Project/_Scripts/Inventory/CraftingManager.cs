using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour {
    InventoryObject CraftingInventory;
    Static_Player_Interface InterfaceScript;
    public ItemObject[] CraftableItems;
    public Button CraftedSlot;

    void Awake () {
        InterfaceScript = GetComponent<Static_Player_Interface> ();
        CraftingInventory = InterfaceScript.inventory;
    }

    void Start () {
        
        InvokeRepeating ("CheckCraft",0f,2f);
    }

    void CheckCraft () { //checks if a crafting recipe has succeeded
        
        CraftAxe ();
        
        if (CraftingInventory.Container.Slots[3].item.Id >=0) { //if the output slots a item allow it to be interacted with (so the player can drag out the crafted item)
            CraftedSlot.interactable = true;
        } else if (CraftingInventory.Container.Slots[3].item.Id <= -1) { //else if the output slots has a null item stop it's interactability (to stop players from putting items in the output slot
            CraftedSlot.interactable = false;
        }
    }
    
    void CraftAxe () { //if wood, seaweed, rock in squence & craftedslot id is set to  a null item id then craft and update crafted slot
        if (CraftingInventory.Container.Slots[0].item.Name == "Wood" && CraftingInventory.Container.Slots[1].item.Name == "Seaweed" && CraftingInventory.Container.Slots[2].item.Name == "Rock" && CraftingInventory.Container.Slots[3].item.Id <=-1) {
            for (int i = 0; i < CraftingInventory.Container.Slots.Length; i++) { 
                if (CraftingInventory.Container.Slots[i].itemamount == 1) { //if item amount is excactly one
                    CraftingInventory.Container.Slots[i].RemoveItem (); //remove that item completely from the inventory
                } else if (CraftingInventory.Container.Slots[i].itemamount > 1) { //else if more than one
                    CraftingInventory.Container.Slots[i].UpdateSlot (CraftingInventory.Container.Slots[i].item, --CraftingInventory.Container.Slots[i].itemamount); //decrease the amount and leave the rest behind
                }
            }
            
            CraftingInventory.Container.Slots[3].UpdateSlot (CraftableItems[0].data,1); //update the output slot with the crafted item
        }
    }
}
