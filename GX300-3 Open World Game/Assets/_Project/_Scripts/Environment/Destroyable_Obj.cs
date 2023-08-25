using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable_Obj : MonoBehaviour {
    public Transform[] Drop_point;
    public ItemObject[] DropItems;
    public int hit_amount = 0;
    public void DestroyObj () { //when destoryed deactivate gameobject and for both drop points spawn their appropriate items 3 times
        gameObject.SetActive (false);
        for (int x = 0; x < 3; x++) {
            for (int i = 0; i <Drop_point.Length; i++) {
                Pickup_Interact.instance.Drop (DropItems[i], Drop_point[i]);
            }   
        }

        //StartCoroutine (Respawn ());
    }

    // IEnumerator Respawn () {
    //     yield return new WaitForSeconds (120f);
    //     gameObject.SetActive (true);
    //     hit_amount = 0;
    // }
}
