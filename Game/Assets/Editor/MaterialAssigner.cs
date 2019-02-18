using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

class MaterialAssigner : EditorWindow{
    [MenuItem("Tools/Ozego/MaterialAssigner")]
    public static void ShowWindow(){
        EditorWindow.GetWindow(typeof(MaterialAssigner));
    }

    string searchQuery = "Sphere";
    Material material;
    Editor materialEditor;

    void OnGUI(){

        GUILayout.Label("Material Assigner", EditorStyles.boldLabel);
        searchQuery = EditorGUILayout.TextField("Prefab name:", searchQuery);

        EditorGUI.BeginChangeCheck();
        material = EditorGUILayout.ObjectField("Material:",material, typeof(Material),true) as Material;
        if (EditorGUI.EndChangeCheck()){
            materialEditor = Editor.CreateEditor(material);
        }
        if (material != null){
            if (materialEditor == null){
                materialEditor = Editor.CreateEditor(material);
            }
            materialEditor.OnPreviewGUI(GUILayoutUtility.GetRect(300, 300), EditorStyles.toolbar);
        }

        if (GUILayout.Button("Replace Materials")){
            GameObject[] gameObjects = FindObjectsByName(searchQuery);
            if (gameObjects == null) {
                EditorUtility.DisplayDialog("No GameObjects found","Try another search term","OK");
            } 
            else if (material == null){
                EditorUtility.DisplayDialog("No Material Selected","Please select a material to be assigned","OK");
            } 
            else {  
                foreach (var o in gameObjects){
                    var mr = o.GetComponent<MeshRenderer>();
                    if(mr!=null){
                        mr.material = material;
                        //TODO: Undo functionality
                    }
                }
                
            }
        }
    }
    private static GameObject[] FindObjectsByName(string SearchQuery){
        return GameObject.FindObjectsOfType<GameObject>().Where(o => o.name == SearchQuery).ToArray();
    }
}
