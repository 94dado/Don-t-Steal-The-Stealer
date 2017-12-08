using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData {
	// we just need one actor container
	public static SceneController scene = new SceneController();
	// use delegate to comunicate between script the load and the store of the data
	public delegate void SerializeAction();
	// happened after loading
	public static event SerializeAction OnLoaded;
	// happened before save
	public static event SerializeAction OnBeforeSave;

	// what we want to load and where
	public static void Load(string path) {
		scene = LoadSceneManager(path);
		OnLoaded();
	}

	public static void Save(string path, SceneController sceneToSave) {
		// throw up everything is stored in this function
		OnBeforeSave();
		SaveSceneManager(path, sceneToSave);
		SceneController.data = null;
	}

	// add scene to the container
	public static void AddSceneManagerData(SceneData data) {
		SceneController.data = data;
	}

	// load the scene from a path
	static SceneController LoadSceneManager(string path) {
		// reload all the text of a file
		// we assume is json
		// we return it as scene container
		return JsonUtility.FromJson<SceneController>(File.ReadAllText(path));
	}

	// save the scene in a path
	static void SaveSceneManager(string path, SceneController sceneToSave) {
		// create the file if not exist
		StreamWriter sw = File.CreateText(path);
		sw.Close();
		// create the json from the scene and add to the file created
		File.WriteAllText(path, JsonUtility.ToJson(sceneToSave));
	}
}
