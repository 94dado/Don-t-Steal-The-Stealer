using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SceneController : MonoBehaviour {

	public GameData data;

	// path to save data
	string dataPath;

	void Awake () {
		// data are stored on this path
		dataPath = Path.Combine(Application.persistentDataPath, "data.json");
		MakePersistent();
	}

	// make the gameobject persistent between scenes
	void MakePersistent() {
		// remove the copy of the scene controller if exist
		GameObject[] objs = GameObject.FindGameObjectsWithTag("SceneManager");
		if (objs.Length > 1) {
			Destroy(this.gameObject);
		}
		// save the scene after the reload
		DontDestroyOnLoad(this.gameObject);
	}

	// called when open the game
	void OnEnable() {
		GameData newData = LoadSceneManager(dataPath);
		// if data to reload exist (used to check the first time we load data)
		if (newData != null) {
			// load data
			data = newData;
		}
	}

	// called when close the game
	void OnDisable() {
		SaveData();
	}

	// load the scene from a path
	GameData LoadSceneManager(string path) {
		// reload all the text of a file if exist
		if(File.Exists(path)) {
			// we return it as scene container
			return JsonUtility.FromJson<GameData>(File.ReadAllText(path));
		}
		return null;
	}

	// save the scene in a path
	void SaveSceneManager(string path, GameData gameData) {
		// create the file if not exist
		File.CreateText(path).Close();
		// create the json from the scene and add to the file created
		File.WriteAllText(path, JsonUtility.ToJson(gameData));
	}

	// used to save new data on json file wherever is necessary
	public void SaveData() {
		// save data on json
		SaveSceneManager(dataPath, data);
	}

	// 
}

[Serializable]
// store the data of all the scenes
public class GameData {
	public Level[] levels;
	public int money;
	public Gadget[] gadgets;
	public Intelligence[] intelligences;
}
