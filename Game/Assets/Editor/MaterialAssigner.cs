using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

class MaterialAssigner : EditorWindow {
    [MenuItem("Tools/Ozego/MaterialAssigner")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(MaterialAssigner));
    }

    string searchQuery = "Sphere";
    public string[] Strings = { "Larry", "Curly", "Moe" };
    Material[] materials;
    
    void OnGUI () {
        GUILayout.Label ("Material Assigner", EditorStyles.boldLabel);
        searchQuery = EditorGUILayout.TextField("Prefab name:", searchQuery);
        
        // "target" can be any class derrived from ScriptableObject 
         // (could be EditorWindow, MonoBehaviour, etc)
         ScriptableObject target = this;
         SerializedObject so = new SerializedObject(target);
         SerializedProperty stringsProperty = so.FindProperty("Strings");
 
         EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
         so.ApplyModifiedProperties(); // Remember to apply modified properties
        
    }
    private static GameObject[] FindObjectsByName(string SearchQuery){
        return (GameObject[])Resources.FindObjectsOfTypeAll<GameObject>().Where(o => o.name == SearchQuery);
    }
}
