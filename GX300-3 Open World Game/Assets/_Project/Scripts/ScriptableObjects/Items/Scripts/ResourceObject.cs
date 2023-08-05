using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Resource")]
public class ResourceObject : ItemObject {
    public bool Tool;
    
    public void Awake () {
        type = ItemType.Resources;
    }
}
