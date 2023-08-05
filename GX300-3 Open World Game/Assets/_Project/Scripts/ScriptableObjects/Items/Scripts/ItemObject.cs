using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Food,
    Tool,
    Resources
}

public enum ItemState {
    Raw,
    Sliced,
    Cooked
}

public abstract class ItemObject : ScriptableObject {
    public GameObject prefab;
    public ItemType type;
    public ItemState state;
    [TextArea(15,20)]
    public string description;

}
