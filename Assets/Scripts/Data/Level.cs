using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Data/Level")]
public class Level : ScriptableObject {

	public string levelName;
	public int positionCountInSceneArray;
	public int timeLimit;
	public int maxObject;
	public bool isLocked;
    public int firstStarPrize;
    public int secondStarPrize;
    public int thirdStarPrize;
   

    [TextArea(10,10)]
	public string description;
}
