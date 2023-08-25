using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwapTrees : MonoBehaviour {

    public static SwapTrees instance;
    
    public GameObject[] TreePrefabs;
    public Transform TreesParent;
    

    // Use this for initialization
    void Start () {
        // Grab the island's terrain data
        TerrainData theIsland;
        theIsland = GetComponent<Terrain> ().terrainData;

        Terrain.activeTerrain.treeDistance = 0; //used to disable trees from terraindata
        // For every tree on the island
        foreach (TreeInstance tree in theIsland.treeInstances) {

            int randomTree = Random.Range (0, TreePrefabs.Length);
            // Find its local position scaled by the terrain size (to find the real world position)
            Vector3 worldTreePos = Vector3.Scale (tree.position, theIsland.size) + Terrain.activeTerrain.transform.position;
            Instantiate (TreePrefabs[randomTree], worldTreePos, Quaternion.identity, TreesParent); // Create a prefab tree on its pos
        }
    }
}