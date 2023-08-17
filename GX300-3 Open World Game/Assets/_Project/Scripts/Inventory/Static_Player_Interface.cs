using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Static_Player_Interface : Player_Inventory_Interface {
    
    public GameObject[] slots;
    
    public override void CreateSlots () { //ued to create events on slots and to update their displays
        DisplayedSlots = new Dictionary<GameObject, InventorySlot> ();
        for (int i = 0; i < inventory.GetSlots.Length; i++) {
            var obj = slots[i];
            
            AddEvent (obj, EventTriggerType.PointerEnter, delegate { OnEnter (obj); });
            AddEvent (obj, EventTriggerType.PointerExit, delegate { OnExit (obj); });
            AddEvent (obj, EventTriggerType.BeginDrag, delegate { OnDragStrt (obj); });
            AddEvent (obj, EventTriggerType.EndDrag, delegate { OnDragEnd (obj); });
            AddEvent (obj, EventTriggerType.Drag, delegate { OnDrag (obj); });
            
            inventory.GetSlots[i].slotDisplay = obj;
            DisplayedSlots.Add (obj, inventory.GetSlots[i]);
        }
    }
}
