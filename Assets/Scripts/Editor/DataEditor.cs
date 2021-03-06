﻿using UnityEngine;
using UnityEditor;
using System.IO;

public class DataEditor {
	[MenuItem("Tools/RemoveDataFile",false)]
	public static void OpenSpriteBakerEditor() {
		string dataPath = Path.Combine(Application.persistentDataPath,"data.json");
        Debug.Log(dataPath);
		File.Delete(dataPath);
        DataManager dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        
        foreach (PersistentGadget g in dataManager.Gadgets)
        {
            g.isLocked = true;
        }

        foreach (PersistentLevel l in dataManager.Levels)
        {
            l.isLocked = true;
            l.levelCompleted = false;
            l.objectsScore = 0;
            l.timeScore = 0;

        }

        foreach (PersistentIntelligence i in dataManager.Intelligence)
        {
            i.isLocked = true;
        }
        //dataManager.AlreadyStarted = false;
    }
}
