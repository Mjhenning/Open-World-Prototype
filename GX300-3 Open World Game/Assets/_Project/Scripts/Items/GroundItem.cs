using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver { //script used to attatch the item type to a gameobject
    public ItemObject item;
    public void OnBeforeSerialize () {
#if UNITY_EDITOR        
        GetComponent<MeshFilter>().mesh = item.groundItem.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshCollider> ().sharedMesh = item.groundItem.GetComponent<MeshFilter> ().sharedMesh;
        GetComponent<MeshCollider> ().convex = true;
        EditorUtility.SetDirty(GetComponentInChildren<MeshFilter>());
        GetComponent<MeshRenderer>().material = item.groundItem.GetComponent<MeshRenderer>().sharedMaterial;
        EditorUtility.SetDirty(GetComponentInChildren<MeshRenderer>());
#endif        
    }

    public void OnAfterDeserialize () {
    }
}
