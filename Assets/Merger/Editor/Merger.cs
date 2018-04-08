using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;



public class Merger : EditorWindow {
	public  Merger mInstance = null;
	public Texture txout= null;
	public  int istart = 0;
	public Texture tx;
	public  List<Texture> txlist=new List<Texture>();
	private List<Texture> txinpanel=new List<Texture>();
	//[MenuItem ("Scene/Mergea", false, 2)]

	public void Merge ()
	{
//		
//		if (txlist == null)
//			return;

		if (mInstance == null) {
			mInstance = (Merger)EditorWindow.GetWindow (typeof(Merger));
			mInstance.titleContent.text = "merge objs";
			mInstance.minSize = new Vector2 (700, 500);
			mInstance.maxSize = new Vector2 (5200, 5000);
			istart = 0;
			mInstance.Show ();
		}
	}
	void OnGUI()
	{
		GUIStyle gs = GUI.skin.button;
		Rect wr = new Rect (200, 200, 700, 500);
		if (mInstance != null) {
			wr = mInstance.position;
			//Debug.Log ("here");
		}

		int ww = (int)wr.width;
		int wh = (int)wr.height;
        
		int vlen =ww - 200;
		if (vlen > wh)
			vlen = wh;

		wr.x = 10;
		wr.y = 10;
		wr.height =vlen- 20;
		wr.width = wr.height;
		GUI.Box(wr, "--------");
		wr.x += wr.width + 5;
		wr.width = ww - vlen - 10;
		wr.height -= 30;


		GUI.Box(wr, "Texture list");
		Rect wrilist = wr;
		int itemlen = 100;
		wrilist.width = itemlen;
		wrilist.height = itemlen;
		wrilist.x += 5;
		wrilist.y += 25;
		if (istart > 0) {
			Rect wrt = wr;
			wrt.x = wrt.x+5;
			wrt.height = 20;
			wrt.width = (wr.width - 120) / 2;
			if (GUI.Button (wrt, "<<<<<<")) {
				
				int xcount = Mathf.FloorToInt (wr.width / (itemlen + 3));
				int ycount = Mathf.FloorToInt (wr.height / (itemlen + 5));

				istart -= xcount * ycount;
				if (istart < 0)
					istart = 0;

			}
		}
		for (int i = istart;i < txlist.Count; i++) {
			//GUI.Box (wrilist, i.ToString ());
			if (GUI.Toggle (wrilist, false, txlist [i], gs)) {
				txinpanel.Add (txlist [i]);
				txlist.RemoveAt (i);
			}
			wrilist.x += itemlen + 3;
			if (wrilist.x > ww - itemlen - 18) {
				wrilist.x = wr.x + 5;
				wrilist.y += itemlen + 5;
				if (wrilist.y >vlen-itemlen-30&&i<29) {
					Rect wrt = wr;
					wrt.x = wrt.x +wr.width- (wr.width - 120) / 2;
					wrt.height = 20;
					wrt.width = (wr.width - 120) / 2;
					if(GUI.Button(wrt,">>>>>>")){
						istart=i+1;

					}
					break;	

				}
			}
		}
		for (int i = 0; i < txinpanel.Count; i++) {
			Debug.Log ("inpanel");
			Rect wrpanel = wr;
			wrpanel.width = itemlen;
			wrpanel.height = itemlen;
			wrpanel.x = 10;
			wrpanel.y = 25;
			GUI.Toggle (wrpanel, true, txinpanel[i],gs);
		}

		wr.y += wr.height + 5;
		wr.width = wr.width / 2;
		wr.height = 30;
		GUI.Button (wr, "OK");
		wr.x += wr.width;
		GUI.Button (wr, "cancle");
	}
}
