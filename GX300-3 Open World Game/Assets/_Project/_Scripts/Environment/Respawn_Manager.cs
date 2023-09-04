using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Manager: MonoBehaviour {
    public static Respawn_Manager instance;

    void Awake () {
        instance = this;
    }

    public void RespawnObj (GameObject go, float Time) {
        StartCoroutine (Respawn (go, Time));
    }

    IEnumerator Respawn (GameObject go,float Time) { //respawns whatever go after whatever given time 
        yield return new WaitForSeconds (Time);
        go.SetActive (true);
    }
}
