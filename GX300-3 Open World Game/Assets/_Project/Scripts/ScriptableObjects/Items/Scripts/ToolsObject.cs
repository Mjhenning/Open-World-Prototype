using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolsObject : ItemObject
{
    public void Awake () {
        type = ItemType.Tool;
    }
}
