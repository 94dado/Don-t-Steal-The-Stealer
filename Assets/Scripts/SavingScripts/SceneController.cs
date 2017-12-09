using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SceneController : MonoBehaviour {

	public Level[] levels;
	public int money;
	public Gadget[] gadgets;
	public Intelligence[] intelligences;
	// at start he create a new scenes manager
	[HideInInspector]
	public static SceneData data = new SceneData();

	// path to save data
	static string dataPath;

	void Awake () {
		dataPath = Path.Combine(Application.persistentDataPath, "data.json");
		// remove the copy of the scene controller if exist
		GameObject[] objs = GameObject.FindGameObjectsWithTag("SceneManager");
		if (objs.Length > 1) {
			Destroy(this.gameObject);
		}
		// ave the scene after the reload
		DontDestroyOnLoad(this.gameObject);
	}

	// save data
	public void Save() {
		SaveData.Save(dataPath, SaveData.scene);
	}

	// load data
	public void Load() {
		SaveData.Load(dataPath);
	}

	// store data in serializable object
	void StoreData() {
		data.levels = levels;
		data.money = money;
		data.gadgets = gadgets;
		data.intelligences = intelligences;
	}

	// load data from serializable object
	void LoadData() {
		this.levels = data.levels;
		this.money = data.money;
		this.gadgets = data.gadgets;
		this.intelligences = data.intelligences;
	}

	// load data from serializable object
	void ApplyData() {
		SaveData.AddSceneManagerData(data);
	}

	// called when apen the game
	void OnEnable() {
		SaveData.OnLoaded += LoadData;
		// we want store data and after apply them
		SaveData.OnBeforeSave += StoreData;
		SaveData.OnBeforeSave += ApplyData;
	}

	// called when close the game
	void OnDisable() {
		// remove function
		SaveData.OnLoaded -= LoadData;
		// we want store data and after apply them
		SaveData.OnBeforeSave -= StoreData;
		SaveData.OnBeforeSave -= ApplyData;
	}
}

[Serializable]
// store the data of all the scenes
public class SceneData {
	public Level[] levels;
	public int money;
	public Gadget[] gadgets;
	public Intelligence[] intelligences;
}
