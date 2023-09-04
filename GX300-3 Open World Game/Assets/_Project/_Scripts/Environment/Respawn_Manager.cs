using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Manager: MonoBehaviour {
    public static Respawn_Manager instance;

    void Awake () {
        instance = this;
    }

    public void RespawnTree (GameObject tree, float Time) {
        StartCoroutine (Respawn (tree, Time));
        tree.GetComponent<Destroyable_Obj> ().hit_amount = 0;
    }

    IEnumerator Respawn (GameObject go,float Time) {
        yield return new WaitForSeconds (Time);
        go.SetActive (true);
    }
}
