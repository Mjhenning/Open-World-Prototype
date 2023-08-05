using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver { //Used to make Dictionary serializable
    public ItemObject[] items;
    public Dictionary<ItemObject, int> GetID = new Dictionary<ItemObject, int> (); //List of Items and their amounts stored as a Dictionary 
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject> ();  //List of Items and their IDs
    public void OnBeforeSerialize () {
    }

    public void OnAfterDeserialize () {
        GetID = new Dictionary<ItemObject, int> ();
        GetItem = new Dictionary<int, ItemObject> ();
        
        for (int i = 0; i < items.Length; i++) {
            GetID.Add(items[i], i);
            GetItem.Add (i, items[i]);
        }
    }
}
