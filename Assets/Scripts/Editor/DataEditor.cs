using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class DataEditor {
	[MenuItem("Tools/RemoveDataFile",false)]
	public static void OpenSpriteBakerEditor() {
		string dataPath = Path.Combine(Application.persistentDataPath,"data.json");
		File.Delete(dataPath);
	}
}
