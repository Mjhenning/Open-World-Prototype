using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver { //Used to make Dictionary serializable
    public ItemObject[] items;
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject> ();  //List of Items and their IDs
    public void OnBeforeSerialize () {
        GetItem = new Dictionary<int, ItemObject> (); //before seialization clears the items out
    }

    public void OnAfterDeserialize () { //after serialization sets item ids
        for (int i = 0; i < items.Length; i++) {
            items[i].data.Id = i;
            GetItem.Add (i, items[i]);
        }
    }
}
