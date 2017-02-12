using UnityEngine;
using UnityEditor;
using System.Collections;

class Open : EditorWindow
{
    [MenuItem("Window/Gamfari Settings")]

    public static void OpenObject()
    {
        AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/Gamfari/Resources/Gamfari");
       
    }

    void OnGUI()
    {
        // The actual window code goes here
    }
}
