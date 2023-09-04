using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Behavior : MonoBehaviour {
    public static Water_Behavior instance;

    public List<Rigidbody> ItemsRb;

    void Awake () {
        instance = this;
    }

    void Start () {
        ItemsRb = new List<Rigidbody>();
    }

    void OnTriggerEnter (Collider other) {
        if (other.GetComponent<PlayerController>()) {
            other.GetComponent<Rigidbody> ().useGravity = false;
        }

        if (other.GetComponent<GroundItem>() && other.GetComponent<GroundItem>().isOnFire) {
            other.GetComponent<GroundItem> ().EnteredWater ();
        }

        if (other.GetComponent<GroundItem>() && other.GetComponent<Rigidbody>().isKinematic != true) {
            ItemsRb.Add (other.attachedRigidbody);
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.GetComponent<PlayerController>()) {
            other.GetComponent<Rigidbody> ().useGravity = true;
        }
        if (other.GetComponent<GroundItem>()) {
            ItemsRb.Remove(other.attachedRigidbody);
        }
    }

    void OnTriggerStay (Collider other) {
        foreach (Rigidbody item in ItemsRb) {
            item.AddForce (0, 1.05f, 0, ForceMode.Force);
        }
        
    }
}
