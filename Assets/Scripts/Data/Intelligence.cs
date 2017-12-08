using UnityEngine;

[CreateAssetMenu(fileName = "New Intelligence", menuName = "Data/Intelligence")]
public class Intelligence : ScriptableObject {

	public Level[] levels;
	public int price;
	[TextArea(10,10)]
	public string description;

	// buy the information and unlock the information
	public virtual void Buy() {
		for(int i = 0;i < levels.Length;i++) {
			levels[i].isLocked = false;
		}
	}
}
