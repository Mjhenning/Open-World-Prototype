using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Food/Fish")]
public class FishObject : FoodObject
{

    public void Awake () {
        foodType = FoodType.Fish;
    }
    
}
