using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BLUEPRINT BEHIND ALL THE FOOD OBJECTS

public enum FoodType{ //Type of the food (if it's a fish or a gruit)
    Fruit,
    Fish
}

public class FoodObject : ItemObject {
    public int RestoreHealthValue;
    public int RestoreSaturationValue;
    public FoodType foodType;
    
}
