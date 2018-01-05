using UnityEngine;
using UnityEditor;
using System.IO;

public class DataEditor {
	[MenuItem("Tools/RemoveDataFile",false)]
	public static void OpenSpriteBakerEditor() {
		string dataPath = Path.Combine(Application.persistentDataPath,"data.json");
        Debug.Log(dataPath);
		File.Delete(dataPath);
	}
}
