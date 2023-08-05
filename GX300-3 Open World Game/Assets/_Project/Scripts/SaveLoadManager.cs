using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour {
    
    [System.Serializable]
    public class PlayerInfo {
        public Vector3 PlayerPos;
        public Quaternion PlayerRotation;
    }
    
    [System.Serializable]
    public class ScreenshotInfo {
        public string ScreenshotPath;
        public string SaveDate_Time;
    }
    [System.Serializable]
    public class SaveData {
        public bool SaveSlotUsed;
        public PlayerInfo playerInfo;
        public ScreenshotInfo screenshotInfo;
    }

    public static SaveLoadManager instance;
    public int SaveSlotCount = 3; //amount of saveslots
    public bool[] SavedSlotUsed; //used to monitor which slots have been used
    

    public SaveData[] tempSaveData = new SaveData[3];

    void Awake () {
        if (instance != null) { //used to destroy this manager if one exists in scene (put in place to stop duplicates)
             Debug.Log ("Found more than one Save/Load Manager in the scene. Destroying the newest one.");
             Destroy (this.gameObject);
             return;
        } else if (instance == null){ //else create a new one
             Debug.Log ("Creating a new Save/Load Manager");
             instance = this;
        }

        CallLoad ();

        DontDestroyOnLoad (gameObject); //tells the system to not destroy this manager
    }

    public void CallLoad () {
        for (int i = 0; i < SaveSlotCount; i++) {
            tempSaveData[i] = JsonUtility.FromJson <SaveData> (PlayerPrefs.GetString("SaveData " + i));
        }
        
        SavedSlotUsed = new bool[tempSaveData.Length];

        for (int i = 0; i < SavedSlotUsed.Length; i++) {
            if (tempSaveData[i] != null) {
                SavedSlotUsed[i] = tempSaveData[i].SaveSlotUsed;
            }
        }
    }

    public void LoadGame (int Saveslot) {
        
        if (SavedSlotUsed[Saveslot] & tempSaveData[Saveslot] != null) {
            
            PlayerController.instance.currenPlayerPos = tempSaveData[Saveslot].playerInfo.PlayerPos;
            PlayerController.instance.CallOnLoad ();

            CameraController.instance.CurrentPlayerViewRotation = tempSaveData[Saveslot].playerInfo.PlayerRotation;
            CameraController.instance.CallOnLoad ();
            
            PlayerController.instance.Inventory.Load ();
        }
    }

    public string GetScreenshotPath (int Saveslot) { //used to grab the correct screenshotpath (when loading screenshots)
        if (tempSaveData[Saveslot] != null && SavedSlotUsed[Saveslot]) {
            string screenshotPath = tempSaveData[Saveslot].screenshotInfo.ScreenshotPath;
            return screenshotPath;    
        } else {
            return null;
        }
    }

    public void SaveGame (int Saveslot) { //save all variables of player and move able objects

        //if (!SavedSlotUsed[Saveslot]) { //this check works?
            Debug.Log ("Saving Game");
            
         tempSaveData[Saveslot]= new SaveData();
        
        //Player Save
        
        tempSaveData[Saveslot].playerInfo = new PlayerInfo ();
        
        tempSaveData[Saveslot].playerInfo.PlayerPos = PlayerController.instance.gameObject.transform.position;
        tempSaveData[Saveslot].playerInfo.PlayerRotation = CameraController.instance.CurrentPlayerViewRotation;

        PlayerController.instance.Inventory.Save ();

        //Screenshot Save
        
        tempSaveData[Saveslot].screenshotInfo = new ScreenshotInfo ();
        
         string screenshotFileName = "screenshot_" + (Saveslot+1)+ ".png"; // Generate a unique filename for the screenshot
         tempSaveData[Saveslot].screenshotInfo.ScreenshotPath = Path.Combine(Application.persistentDataPath, screenshotFileName); // Construct the full save path for the screenshot
         ScreenCapture.CaptureScreenshot(tempSaveData[Saveslot].screenshotInfo.ScreenshotPath); // Take a screenshot and save it to the specified path

         tempSaveData[Saveslot].screenshotInfo.SaveDate_Time = DateTime.Now.ToString ("F"); //used to save date and time when game was saved (String displayed on saveslots)
         
         //Save slot used?
         tempSaveData[Saveslot].SaveSlotUsed = true;
         
         PlayerPrefs.SetString ("SaveData " + Saveslot.ToString(), JsonUtility.ToJson (tempSaveData[Saveslot]));
         PlayerPrefs.Save ();    
        }
    //}
}



