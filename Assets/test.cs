using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	static YieldInstruction wait420ms = new WaitForSeconds(0.42f);
	private bool firstStarted = false;
	IEnumerator Start()
	{
		Debug.Log ("start");
		yield return wait420ms;
		Debug.Log ("afterwait420ms");
		this.BCstart();
	}

	void BCstart () {
		Debug.Log ("BCstart");
		if (true == firstStarted)
			return;
		firstStarted = true;
	}
	// Update is called once per frame
	void LateUpdate () {
		Debug.Log ("lateupdate");
		if (false == firstStarted)
			return;
		Debug.Log ("------------cal");
	}
}
