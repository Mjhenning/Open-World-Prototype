using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver { //script used to attatch the item type to a gameobject
    public ItemObject item;
    public ItemState currentstate;
    public GameObject FlamesPrefab;
    public GameObject Flames;
    public bool isOnFire;
    public bool inFire;

    public void OnBeforeSerialize () { //Used to change mesh, material and collider mesh of the ground item based on assigned item.
#if UNITY_EDITOR        
        GetComponent<MeshFilter>().mesh = item.groundItem.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshCollider> ().sharedMesh = item.groundItem.GetComponent<MeshFilter> ().sharedMesh;
        EditorUtility.SetDirty(GetComponent<MeshFilter>());
        GetComponent<MeshRenderer>().material = item.groundItem.GetComponent<MeshRenderer>().sharedMaterial;
        EditorUtility.SetDirty(GetComponent<MeshRenderer>());
        
#endif
    }

    public void OnAfterDeserialize () {
    }

    void OnEnable () {
        OnCreate ();
    }

    public void OnCreate () { //if the item is burnable instantiate the flames prefab on this item and deactivate it
        if (item.Burnable) {
            Flames = Instantiate(FlamesPrefab,transform.position,quaternion.identity,transform);
            Flames.SetActive (false); 
        }

        UpdateState (); //also update the current item's state
    }

    public void UpdateState () { //when the state gets updated
        currentstate = item.state; //update the itemstate enum and all the meshes and colliders
        GetComponent<MeshFilter>().mesh = item.groundItem.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshCollider> ().sharedMesh = item.groundItem.GetComponent<MeshFilter> ().sharedMesh;
        GetComponent<MeshRenderer>().material = item.groundItem.GetComponent<MeshRenderer>().sharedMaterial;
    }
    
    void Update () { //on update
        if (item.Burnable) { //if burnable
            if (isOnFire) { //and currently on fire
                Flames.SetActive (true); //show the flames
            } else if (!isOnFire) { //else if not currently on fire
                Flames.SetActive (false); //hide the flames
            }


            switch (currentstate) { //switch statement to control the state AND to swap out the itemobject when the state changes to the correct item object
                case ItemState.Cooked:
                    if (item is FoodObject _CookedObject) {
                        //for food
                        item = _CookedObject.FoodCooked;
                        UpdateState ();
                    }

                    break;
                case ItemState.Raw:
                    if (item is FoodObject _RawObject) {
                        //for food
                        item = _RawObject.FoodRaw;
                        UpdateState ();
                    } else if (item is ResourceObject _RawResource) {
                        //for resources
                        item = _RawResource.ResourceRaw;
                        UpdateState ();
                    }

                    break;
                case ItemState.Burnt:
                    if (item is FoodObject _BurnObject) {
                        //for food
                        item = _BurnObject.FoodBurnt;
                        UpdateState ();
                    } else if (item is ResourceObject _BurntResource) {
                        //for resources
                        item = _BurntResource.ResourceBurnt;
                        UpdateState ();
                    }

                    break;
            }
        }
    }
    
    
    public void InsideFire() { //when inside fire
        if (item.Burnable) { //and the item is burnable
            inFire = true; //it's inside the fire
            StartFire (); //start the fire and tell the system the object is onfire
            if ((item is FoodObject _CookedObject)) { //if itm is a foodobject have a specific sequence of behavior play
                switch (currentstate) {
                    case ItemState.Raw:
                        StartCoroutine (TimeUntilCooked ());
                        break;
                    case ItemState.Cooked:
                        StartCoroutine (TimeUntilBurnt ());
                        break;
                    case ItemState.Burnt:
                        StartCoroutine (TimeUntilDestroy());
                        break;
                } 
            }
            else if (item is ResourceObject _ResourceObject) { //else if ResourceObject have a different set of behavior play
                switch (currentstate) {
                    case ItemState.Raw:
                        StartCoroutine (TimeUntilBurnt ());
                        break;
                    case ItemState.Burnt:
                        StartCoroutine (TimeUntilDestroy());
                        break;
                } 
            }
        }
    }
    
    public void OutsideFire () { //when outside fire
        if (item.Burnable) { //and this item is burnable
            StopAllCoroutines (); //stop all the state transitions
            StartCoroutine (TimeUntilStopBurning ()); //and start the coroutine to disable the fire
        }
    }

    void StartFire () {
        Flames.SetActive (true);
        isOnFire = true;
    }

    IEnumerator TimeUntilCooked () { //ienumerator to swap state to cooked and start timer until item gets burnt
        if (inFire) {
            yield return new WaitForSeconds (4f);
            currentstate = ItemState.Cooked;
            StartCoroutine (TimeUntilBurnt ());
        }
    }

    IEnumerator TimeUntilBurnt () { //ienumerator to swap state to burnt and start timer until item gets destoryed
        if (inFire) {
            yield return new WaitForSeconds (8f);
            currentstate = ItemState.Burnt;
            StartCoroutine (TimeUntilDestroy());
        }
    }

    IEnumerator TimeUntilDestroy () { //wait for f4 seconds and destroy the gameobject if still inside fire
        if (inFire) {
            yield return new WaitForSeconds (4f);
            Destroy (gameObject);
        }
    }

    IEnumerator TimeUntilStopBurning () { //function to stop an item from burning after 4 seconds when it exits the fire
        yield return new WaitForSeconds (8f);
        StopFire ();
    }
    void StopFire () { //used to disable flames and tell the system the object isn't on fire
        Flames.SetActive (false);
        isOnFire = false;
    }

    public void EnteredWater () {
        StopAllCoroutines (); //stop all the state transitions
        isOnFire = false;
    }
    
}
