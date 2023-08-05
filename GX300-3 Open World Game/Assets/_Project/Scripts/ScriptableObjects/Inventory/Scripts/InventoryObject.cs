using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver { //script for the inventory

    public string savePath;
    ItemDatabaseObject database;
    
    public List<InventorySlot> Container = new List<InventorySlot> (); //holds a list of the inventory slots with their items and the amount of their items

    void OnEnable () {
#if UNITY_EDITOR //Do below if in unity editor (because of serialization issues)
        database = (ItemDatabaseObject) AssetDatabase.LoadAssetAtPath ("Assets/_Project/Resources/Database.asset", typeof(ItemDatabaseObject)); //used to load from the database made instead of overwriting it each time
#else //else load from resources folder
        database = Resources.Load<ItemDatabaseObject> ("Database");
#endif
    }

    public void AddItem (ItemObject _item, int _amount) {
        
        for (int i = 0; i < Container.Count; i++) {
            
            if (Container[i].item == _item) { // if item exist increase it's amount
                Container[i].addAmount (_amount);
                return; //and return
            }
        }
        Container.Add (new InventorySlot (database.GetID[_item],_item, _amount)); //else run this code
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
            file.Close ();
        }
    }
    public void OnBeforeSerialize () {
    }

    public void OnAfterDeserialize () { //as soon as anything changes on sciptable object so that unity has to change this object (such as scene reloads), we look through all the items and repopulate each slot
        for (int i = 0; i < Container.Count; i++) {
            Container[i].item = database.GetItem[Container[i].ID]; //adds items back
        }
    }
}

    [System.Serializable]
    public class InventorySlot { //script for each slot within the in venotry

        public int ID; //item ID
        public ItemObject item; //Item identifier
        public int itemamount; //amount of this item

        public InventorySlot (int _id,ItemObject _item, int _amount) { //received item = slot item % recieved amount = amount of item in slot
            ID = _id;
            item = _item;
            itemamount = _amount;
        }

        public void addAmount (int value) { //adds 1 amount of the item to the inventory
            itemamount += value;
        }
}
