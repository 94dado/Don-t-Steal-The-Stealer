using UnityEngine;

[CreateAssetMenu(fileName = "New Gadget", menuName = "Data/Gadget")]
public class Gadget : ScriptableObject {

	public string gadgetName;
	public int positionCountInSceneArray;
	public bool isLocked;
	public int price;
	[TextArea(10,10)]
	public string description;
}
