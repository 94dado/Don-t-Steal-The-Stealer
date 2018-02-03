using System.Collections;
using UnityEngine;

public class Ghoster : MonoBehaviour {

    public GameObject[] disappearingList;
    public int waitTime;
    private bool start = false;
	
	// Update is called once per frame
	void Update () {
		//when press enter, start coroutine that every "waitTime" remove an object. When finished, kill coroutine
        if(!start && Input.GetKeyUp(KeyCode.Return)) {
            start = true;
            Debug.Log("Start!");
            StartCoroutine("Killer");
        }

	}

    private IEnumerator Killer() {
        yield return new WaitForSeconds(waitTime);
        int i = 0;
        while(i < disappearingList.Length) {
            GameObject obj = disappearingList[i];
            Destroy(obj);
            i++;
            yield return new WaitForSeconds(waitTime);
        }
        Debug.Log("Finished!");
    }
}
