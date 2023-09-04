using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways] //execute certain methods while not in game build
public class LightingManager : MonoBehaviour {
    [SerializeField]Light DirectionalLight;
    [SerializeField] LightingPreset Preset;
    [SerializeField, Range (0, 24)] float TimeOfDay;


    void Update () {
        if (Preset == null) { //checks if preset has been assigned
            return;
        }

        if (Application.isPlaying) { //used to change time of day in the inspector
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; //Clamp between 0-24
            UpdateLighting (TimeOfDay / 24f);
        } else { //if running the game build time of day changes slowly
            TimeOfDay += Time.deltaTime * 0.1f;
            TimeOfDay %= 24; //Clamp between 0-24
            UpdateLighting (TimeOfDay / 24f);
        }
    }

    void UpdateLighting (float timePercent) { //update the lighting based on the time of day
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate (timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate (timePercent);

        if (DirectionalLight != null) {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate (timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler (new Vector3 ((timePercent * 360f) - 90f, 170f, 0));
        }
    }
    
    
    void OnValidate () { //caled whenever inspector changes or script reloads
        if (DirectionalLight != null) { //if Directional light has been assigned return
            return;
        }

        if (RenderSettings.sun != null) { //if a sun has been assigned return
            DirectionalLight = RenderSettings.sun;
        } else { //else find all lights in scene and if the type = directional assign it to DirectionaLight
            Light[] lights = GameObject.FindObjectsOfType<Light> ();
            foreach (Light _light in lights) {
                if (_light.type == LightType.Directional) {
                    DirectionalLight = _light;
                    return;
                }
            }
        }
    }
}
