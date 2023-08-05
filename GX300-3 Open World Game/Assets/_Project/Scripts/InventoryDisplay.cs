using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class InventoryDisplay : MonoBehaviour {
    public InventoryObject inventory;

    public int x_strt;
    public int y_strt;
    
    public int x_gap;
    public int Num_Of_Columns;
    public int y_gap;
    Dictionary<InventorySlot, GameObject> itemsDsiplayed = new Dictionary<InventorySlot, GameObject> ();

    // Start is called before the first frame update
    void Start() {
        CreateDisplay ();
    }

    // Update is called once per frame
    void Update() {
        UpdateDisplay();
    }

    public void CreateDisplay () { //used to create a inventory display based on the items inside of the player's inventory;
        for (int i = 0; i < inventory.Container.Count; i++) {
            var obj = Instantiate (inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform> ().localPosition = GetPos (i);
            obj.GetComponentInChildren<TextMeshProUGUI> ().text = inventory.Container[i].itemamount.ToString ("n0");
            itemsDsiplayed.Add (inventory.Container[i], obj);
        }  
    }

    public Vector3 GetPos (int i) { //used to get the positions to create each item's display
        return new Vector3 (x_strt +(x_gap * (i % Num_Of_Columns)),y_strt + (-y_gap * (i/Num_Of_Columns)), 0f);
    }

    public void UpdateDisplay () {
        for (int i = 0; i < inventory.Container.Count; i++) {
            if (itemsDsiplayed.ContainsKey(inventory.Container[i])) {
                itemsDsiplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI> ().text = inventory.Container[i].itemamount.ToString ("n0");
            } else {
                var obj = Instantiate (inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform> ().localPosition = GetPos (i);
                obj.GetComponentInChildren<TextMeshProUGUI> ().text = inventory.Container[i].itemamount.ToString ("n0");
                itemsDsiplayed.Add (inventory.Container[i], obj);
            }
        }
    }
}
