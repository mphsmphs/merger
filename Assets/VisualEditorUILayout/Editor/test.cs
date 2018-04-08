using UnityEngine;
using UnityEditor;
using System.Collections;

public class test : EditorWindow
{
    [MenuItem("Scene/test", false, 2)]
    public static void testwd()
    {
        EditorWindow.GetWindow(typeof(test));
    }
    public void OnGUI()
    {
        
        GUI.Button(new Rect(86, 188, 50, 18), "Btn");
        GUI.Button(new Rect(216, 232, 50, 18), "Btn");
        GUI.Button(new Rect(320, 314, 50, 18), "Btn");
        GUI.Button(new Rect(86, 315, 50, 18), "Btn");
    }
}
