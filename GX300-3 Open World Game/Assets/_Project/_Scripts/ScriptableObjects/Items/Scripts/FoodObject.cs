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
    public ItemObject FoodRaw;
    public ItemObject FoodCooked;
    public ItemObject FoodBurnt;
    public int RestoreHealthValue;
    public int RestoreSaturationValue;

    public void OnUse () {
            UI_Manager.instance.Saturation = Mathf.Clamp (UI_Manager.instance.Saturation, 0, UI_Manager.instance.maxSaturation);
            UI_Manager.instance.Health =Mathf.Clamp (UI_Manager.instance.Health, 0, UI_Manager.instance.maxHealth);
            UI_Manager.instance.GainSaturation(RestoreSaturationValue);
            UI_Manager.instance.HealHealth(RestoreHealthValue);
    }
}
