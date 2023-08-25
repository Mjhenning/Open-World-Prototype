using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public abstract class Player_Inventory_Interface : MonoBehaviour {
    
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> DisplayedSlots = new Dictionary<GameObject, InventorySlot> ();

    void Awake () {
        
        
        for (int i = 0; i < inventory.GetSlots.Length; i++) { //whenever a interface is created loops through all items and links them all as the parent
            inventory.GetSlots[i].parentInventroy = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        
        CreateSlots ();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface (gameObject);});
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface (gameObject);});
    }

    void OnSlotUpdate (InventorySlot _slot) { //new code to update slots
        if (_slot.item.Id >= 0) {
            _slot.slotDisplay.transform.GetChild (0).GetComponentInChildren<Image> ().sprite = _slot.itemobject.uiDisplay; //updates the sprite to the item's sprite
            _slot.slotDisplay.transform.GetChild (0).GetComponentInChildren<Image> ().color = new Color (1, 1, 1, 1); //makes the color fully white (because null item doesn't currently have a sprite)
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI> ().text = _slot.itemamount == 1 ? "" : _slot.itemamount.ToString ("n0"); //actually sets the item amount only if there is more than one
        } else {
            _slot.slotDisplay.transform.GetChild (0).GetComponentInChildren<Image> ().sprite = null;
            _slot.slotDisplay.transform.GetChild (0).GetComponentInChildren<Image> ().color = new Color (1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI> ().text = "";
        }
    }

    // Update is called once per frame //old code to update Displays
    // void Update() {
    //     DisplayedSlots.UpdateSlotDisplay();
    // }

    public abstract void CreateSlots ();
    

    protected void AddEvent (GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) { //adds new events to the ui buttons (a way to automate add events onto each button when they are instantiated the moment the inventory loads)
        EventTrigger _trigger = obj.GetComponent<EventTrigger> ();
        var eventTrigger = new EventTrigger.Entry (); //new entry to event triggers
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener (action);
        _trigger.triggers.Add (eventTrigger);
    }

    public void OnEnter (GameObject obj) {
        MouseData.slotHoveredOver = obj; //adds object hovered over to the mousedata
    }
    
    public void OnExit (GameObject obj) {
        MouseData.slotHoveredOver = null; //remove object hovered over to the mousedata
    }
    
    public void OnEnterInterface (GameObject obj) { //put in place to stop the item from being deleted when inside interface
        MouseData.InterfaceUnderMouse = obj.GetComponent<Player_Inventory_Interface> ();
    }
    
    public void OnExitInterface (GameObject obj) { //tels mousedata that 
        MouseData.InterfaceUnderMouse  = null;
    }
    
    public void OnDragStrt (GameObject obj) {
        
        MouseData.tempItemDrag = CreateTmepItem(obj);
    }

    public GameObject CreateTmepItem (GameObject obj) {
        GameObject tempitem = null;
        if (DisplayedSlots[obj].item.Id >= 0) { //if item exists on inventory
            
            tempitem = new GameObject ();
            var rt = tempitem.AddComponent<RectTransform> ();
            rt.sizeDelta = new Vector2 (65, 65);
            tempitem.transform.SetParent (transform.parent);
        
            var img = tempitem.AddComponent<Image> ();
            img.sprite = DisplayedSlots[obj].itemobject.uiDisplay; //grabs the item being dragged's sprite
            img.raycastTarget = false; //put in place to stop the mouse's raycast from hitting the sprite iteself   
        }

        return tempitem;
    }
    
    public void OnDragEnd (GameObject obj) {
        Destroy(MouseData.tempItemDrag); //destroy the item picked up by mouse

        if (MouseData.InterfaceUnderMouse == null) { //if no interface under object delete the object
            DisplayedSlots[obj].DropItem ();
            return;
        }

        if (MouseData.slotHoveredOver && MouseData.slotHoveredOver.GetComponent<Button>().interactable) { //if mouse is currently hovering over an item and drag stops && slot it hovers over is interactable (set in place for crafting inventories)
            InventorySlot mouseHoverSlotData = MouseData.InterfaceUnderMouse.DisplayedSlots[MouseData.slotHoveredOver];
            inventory.SwapItems (DisplayedSlots[obj], mouseHoverSlotData);
        }

        HotbarManager.instance.UpdateSelectedSlot (); //just to update slot 0 on hotbar if item is dropped in there
    }
    public void OnDrag (GameObject obj) { //change item sprite position to mouse pos
        if (MouseData.tempItemDrag != null) {
            MouseData.tempItemDrag.GetComponent<RectTransform> ().position = Input.mousePosition; //follow mouse
        }
        
    }
    
}

    public static class MouseData { //class that stores the obj in the inventory and the item currently being dragged
        public static Player_Inventory_Interface InterfaceUnderMouse;
        public static GameObject tempItemDrag;
        public static GameObject slotHoveredOver;
    }

public static class ExtensionMethods {
    public static void UpdateSlotDisplay (this Dictionary<GameObject, InventorySlot> _slotsOnInterface) { //used to update the displayed slots on the UI (old code)
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface) {
            if (_slot.Value.item.Id >= 0) {
                _slot.Key.transform.GetChild (0).GetComponentInChildren<Image> ().sprite = _slot.Value.itemobject.uiDisplay; //updates the sprite to the item's sprite
                _slot.Key.transform.GetChild (0).GetComponentInChildren<Image> ().color = new Color (1, 1, 1, 1); //makes the color fully white (because null item doesn't currently have a sprite)
                _slot.Key.GetComponentInChildren<TextMeshProUGUI> ().text = _slot.Value.itemamount == 1 ? "" : _slot.Value.itemamount.ToString ("n0"); //actually sets the item amount only if there is more than one
            } else {
                _slot.Key.transform.GetChild (0).GetComponentInChildren<Image> ().sprite = null;
                _slot.Key.transform.GetChild (0).GetComponentInChildren<Image> ().color = new Color (1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI> ().text = "";
            }
        }
    }
}
