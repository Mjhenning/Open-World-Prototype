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
    public GameObject prefab; //UI prefab
    public ItemType type;
    public ItemState state;
    [TextArea(15,20)]
    public string description; //item description

}
