using UnityEngine;
using System.Collections;
using UnityEditor;

public class MergerSetup : EditorWindow {
	public static MergerSetup mInstance = null;
	public int tis = 0;



	[MenuItem ("Scene/Merge", false, 2)]
	public static void Merge ()
	{
		

		if (mInstance == null) {
			mInstance = (MergerSetup)EditorWindow.GetWindow (typeof(MergerSetup));
			mInstance.titleContent.text = "merge objs";
			mInstance.minSize = new Vector2 (520,350);
			mInstance.maxSize = new Vector2 (520,350);

			mInstance.Show ();
		}
	}
	void OnGUI()
	{
		int xposa=10;
		int xposb=20;
		int xposc=30;
		int ypos=10;
		int heia=30;
		int heib=200;
		int lena = 10;
		int lenb = 20;
		int lenc = 50;
		int lend = 100;
		int lene = 200;
		int lenf = 500;



		Rect wr = new Rect (xposa, ypos, lenf, heia);
		tis=GUI.Toolbar(wr,tis,new string[]{"merge into a new image","merge into a exist image"});

		if (tis == 0) {
			wr.x = xposb;
			wr.y += heia + lenb;
			wr.width = lene;
			wr.height = heia;
			GUI.Label (wr, "width:");
			wr.x += lene+lenb;
			GUI.Label (wr, "height:");
			wr.x = xposb;
			wr.y += heia;
			wr.width = lene;
			wr.height = heia;
			GUI.HorizontalSlider (wr, 0f, 0f, 512f );

			wr.x += lene+lenb;

			GUI.HorizontalSlider(wr, 0f, 0f, 512f);
			wr.x = xposb;
			wr.y += heia;
			wr.height = heia;
			wr.width =2*lene;
			GUI.Button (wr, "choose path");
			wr.x += wr.width;
			wr.width = lenc+lenb;
			GUI.Button (wr, "continue");
		} else {
			//wr.x +=heib;
			wr.x = xposb+lenb;
			wr.y += heia + lenb;
			wr.width = lene;
			wr.height = lene;
			GUI.Box (wr,"");
			wr.x = xposb;
			wr.y += heia+lene;
			wr.height = heia;
			wr.width =2*lene;
			GUI.Button (wr, "choose path");
			wr.x += wr.width;
			wr.width = lenc+lenb;
			if (GUI.Button (wr, "continue")) {
				Debug.Log (Selection.gameObjects.Length);
				Merger mg = new Merger ();
				Debug.Log (mg.txlist);
				foreach (GameObject o in Selection.gameObjects) {
					MeshRenderer mrtmp = o.GetComponent<MeshRenderer> ();

					if (mrtmp != null) {
						Texture tx = mrtmp.sharedMaterial.mainTexture;


						mg.txlist.Add (tx);
					}
				}
				Debug.Log (mg.txlist);
				mInstance.Close ();
				mg.Merge ();

				}
			}
		}
	}

