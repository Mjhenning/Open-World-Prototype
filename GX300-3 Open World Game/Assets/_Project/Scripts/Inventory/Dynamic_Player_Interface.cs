using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dynamic_Player_Interface : Player_Inventory_Interface
{
    public GameObject inventoryPrefab;
    
    [Header("UI layout specifics")]
    public int x_strt;
    public int y_strt;
    public int x_gap;
    public int Num_Of_Columns;
    public int y_gap;
    
    public override void CreateSlots () { //used to create a inventory display based on the items inside of the player's inventory;
        DisplayedSlots = new Dictionary<GameObject, InventorySlot> ();
        for (int i = 0; i < inventory.GetSlots.Length; i++) {
            var obj = Instantiate (inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform> ().localPosition = GetPos (i);

            AddEvent (obj, EventTriggerType.PointerEnter, delegate { OnEnter (obj); });
            AddEvent (obj, EventTriggerType.PointerExit, delegate { OnExit (obj); });
            AddEvent (obj, EventTriggerType.BeginDrag, delegate { OnDragStrt (obj); });
            AddEvent (obj, EventTriggerType.EndDrag, delegate { OnDragEnd (obj); });
            AddEvent (obj, EventTriggerType.Drag, delegate { OnDrag (obj); });
            
            inventory.GetSlots[i].slotDisplay = obj; //
            
            DisplayedSlots.Add (obj, inventory.GetSlots[i]);
        }
    }
    
    Vector3 GetPos (int i) { //used to get the positions to create each item's display
        return new Vector3 (x_strt +(x_gap * (i % Num_Of_Columns)),y_strt + (-y_gap * (i/Num_Of_Columns)), 0f);
    }
}
