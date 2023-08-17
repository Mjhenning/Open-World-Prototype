using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MAIN BLUEPRINT BEHIND ALL THE ITEMS IN THE GAME


public enum ItemType { //the type of the item
    Food,
    Tool,
    Resources
}

public enum ItemState { //the state of the item
    Raw,
    Sliced,
    Cooked
}

public abstract class ItemObject : ScriptableObject {
    public Sprite uiDisplay; //UI prefab
    public bool Stackable;
    public GameObject groundItem;
    public ItemType type;
    public ItemState state;
    [TextArea(15,20)]
    public string description; //item description
    public Item data = new Item ();
}

[System.Serializable]
public class Item {
    public string Name;
    public int Id = -1;

    public Item () {
        Name = "";
        Id = -1;
    }
    
    public Item (ItemObject item) {
        Name = item.name;
        Id = item.data.Id;
    }
}