using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataManager : MonoBehaviour {

	// started game data
	[SerializeField]
	GameData data;
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
		LoadData();
	}

	// called when close the game
	void OnDisable() {
		SaveData();
	}

	// load the scene from a path
	void LoadData() {
		// reload all the text of a json file if exist
		if(File.Exists(dataPath)) {
			// we return it as game data
			data = JsonUtility.FromJson<GameData>(File.ReadAllText(dataPath));
		}
	}

	// save the scene in a path
	void SaveData() {
		// create the file if not exist
		File.CreateText(dataPath).Close();
		// create the json from the scene and add to the file created
		File.WriteAllText(dataPath, JsonUtility.ToJson(data));
	}

	// get or set the levels of game
	public Level[] Levels {
		get {
			return data.levels;
		}
		set { 
			data.levels = value;
			SaveData();
		}
	}

	// get or set the levels of game
	public Gadget[] Gadgets {
		get {
			return data.gadgets;
		}
		set { 
			data.gadgets = value;
			SaveData();
		}
	}

	// get or set the intelligences of game
	public Intelligence[] Intelligence {
		get {
			return data.intelligence;
		}
		set { 
			data.intelligence = value;
			SaveData();
		}
	}

	// get or set the money of game
	public int MoneyData {
		get { 
			return data.money;
		}
		set {
			data.money = value;
			SaveData();
		}
	}
}

[Serializable]
// store the data of all the scenes
public class GameData {
	public int money;
	public Level[] levels;
	public Gadget[] gadgets;
	public Intelligence[] intelligence;
}
