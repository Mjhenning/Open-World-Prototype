using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType{
    Fruit,
    Fish
}

public class FoodObject : ItemObject {
    public int RestoreHealthValue;
    public int RestoreSaturationValue;
    public FoodType foodType;
    
    public void Awake () {
        type = ItemType.Food;
    }
}
