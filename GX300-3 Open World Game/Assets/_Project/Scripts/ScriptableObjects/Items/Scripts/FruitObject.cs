using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Food/Fruit")]

//BLUEPRINT BEHIND ALL THE FRUIT OBJECTS

public class FruitObject : FoodObject
{

    public void Awake () {
        foodType = FoodType.Fruit;
    }
    
}
