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

	[HideInInspector]
	public int objectsScore;
	[HideInInspector]
	public int timeScore;
	[HideInInspector]
	public bool levelCompleted;
	// check the number of the stars reached in this level
	[HideInInspector]
	public int StarsScore {
		get { 
			int i = 0;
			if(levelCompleted) {
				i++;
			}
			// collected all the object
			if(objectsScore == maxObject && i == 1) {
				i++;
			}
			// finished in limit time
			if(timeScore <= timeLimit && i == 2) {
				i++;
			}
			return i;
		}
	}
}
