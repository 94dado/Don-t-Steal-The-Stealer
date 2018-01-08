using UnityEngine;

[CreateAssetMenu(fileName = "New Intelligence", menuName = "Data/Intelligence")]
public class Intelligence : ScriptableObject {

	public Level[] levels;
    public string intelligenceName;
    public int[] unlockedLevels;
	public int positionCountInSceneArray;
	public int price;
    public bool isLocked;
    public Sprite image;
    [TextArea(10,10)]
	public string description;
}
