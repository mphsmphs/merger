using UnityEngine;
using System.Collections;
using System.Threading;
public class taa : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("=======startmain=====");
		StartCoroutine (dodo());
	}

	IEnumerator dodo(){
		Debug.Log ("++++++++dodo+++++++++");
		yield return new WaitForSeconds (2);
		Debug.Log ("+++++++++dodowaitdone++++++++++");
		for (int i = 0; i < 100; i++) {
			Debug.Log ("||||||||||||||||||||||||||" + i.ToString ());
			Thread.Sleep (30);
		}

	}
	// Update is called once per frame
	void Update () {
	
	}
}
