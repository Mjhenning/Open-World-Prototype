using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject { //script for the inventory

    public string savePath;
    public ItemDatabaseObject database;

    public Inventory Container; //holds a list of the inventory slots with their items and the amount of their items
    public InventorySlot[] GetSlots { get { return Container.Slots; } } //used to grab all Slots on inventory


    public bool AddItem (Item _item, int _amount) { //used to add items to the inventory
        //Check if a stack already exists.
        InventorySlot slot = FindItemOnInventory(_item);
        if(slot != null && database.GetItem[_item.Id].Stackable){
            slot.addAmount(_amount);
            return true;
        }
        if(EmptySlotCount > 0){
            //Place in first empty slot.
            SetEmptySlot(_item, _amount);
            return true;
        }
        else{
            //No way to add item.
            return false;
        }
    }

    public int EmptySlotCount{ //retrieves the amount of empty slots in invetory
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++) {
                if (GetSlots[i].item.Id <= -1) {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory (Item _item) { //checks if slot id matches picked up item id
        for (int i = 0; i < GetSlots.Length; i++) {
            if (GetSlots[i].item.Id == _item.Id) {
                return GetSlots[i];
            }
        }

        return null;
    }

    public InventorySlot SetEmptySlot (Item _item, int _amount) { //used to set empty slots
        for (int i = 0; i < GetSlots.Length; i++) {
            if (GetSlots[i].item.Id <= -1) {
                GetSlots[i].UpdateSlot (_item, _amount);
                return GetSlots[i];
            } 
        }
        //set up functionality for full inventory
        return null;
    }

    public void SwapItems (InventorySlot item1, InventorySlot item2) { //used to swap item 1 with item 2
         InventorySlot temp = new InventorySlot (item2.item, item2.itemamount);
         item2.UpdateSlot (item1.item, item1.itemamount);
         item1.UpdateSlot (temp.item, temp.itemamount);
    }

    public void RemoveItem (Item _item) { //used to completely delete an item from the inventory
        for (int i = 0; i < GetSlots.Length; i++) {
            if (GetSlots[i].item == _item) {
                GetSlots[i].UpdateSlot (null, 0);
            }
        }
    }

    public void Save () { //Save inventory from json file
        string saveData = JsonUtility.ToJson (this, true);
        BinaryFormatter bf = new BinaryFormatter ();
        FileStream file = File.Create (string.Concat (Application.persistentDataPath, savePath));
        bf.Serialize (file, saveData);
        file.Close();
    }

    public void Load () { //Load inventory from json file
        if (File.Exists(String.Concat(Application.persistentDataPath, savePath))) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream file = File.Open (string.Concat (Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite (bf.Deserialize (file).ToString (), this);
            
            for (int i = 0; i < GetSlots.Length; i++) { //force loads
                GetSlots[i].UpdateSlot (GetSlots[i].item, GetSlots[i].itemamount);
            }
            file.Close ();
        }
    }
}

[System.Serializable]
public class Inventory { //hold all the items with their data
    [FormerlySerializedAs ("Items")] public InventorySlot[] Slots = new InventorySlot[49];

    public void Clear () { //used to clear inventory
        for (int i = 0; i < Slots.Length; i++) {
            Slots[i].RemoveItem ();
        }
    }

}

    [System.Serializable]
    public class InventorySlot { //script for each slot within the in venotry
        [System.NonSerialized] //stops this from opening in editor and stops system from trying to save this object
        public Player_Inventory_Interface parentInventroy;
        [System.NonSerialized]
        public GameObject slotDisplay;
        [System.NonSerialized]
        public SlotUpdated OnAfterUpdate;
        [System.NonSerialized]
        public SlotUpdated OnBeforeUpdate;
        
        public Item item; //Item identifier
        public int itemamount; //amount of this item

        public ItemObject itemobject { //makes it a public accesible with only a readonly
            get    
            {
                if (item.Id >= 0) {
                    return parentInventroy.inventory.database.GetItem[item.Id];
                }

                return null;
            }
        }

        public delegate void SlotUpdated (InventorySlot _slot); //used to update slots (used to pass methods/arguments to other methods)
        
        public InventorySlot () { //received item = slot item % recieved amount = amount of item in slot
            UpdateSlot (new Item (), 0);
        }
        public InventorySlot (Item _item, int _amount) { //received item = slot item % recieved amount = amount of item in slot
            UpdateSlot (_item, _amount);
        }
        
        public void UpdateSlot (Item _item, int _amount) { //received item = slot item % recieved amount = amount of item in slot
            if (OnBeforeUpdate != null) {
                OnBeforeUpdate.Invoke (this);
            }
            item = _item;
            itemamount = _amount;

            if (OnAfterUpdate != null) {
                OnAfterUpdate.Invoke (this);
            }
        }

        public void RemoveItem () { //deletes the item
            UpdateSlot (new Item (), 0);
        }

        public void addAmount (int value) { //adds 1 amount of the item to the inventory
            UpdateSlot (item, itemamount += value);
        }
}
