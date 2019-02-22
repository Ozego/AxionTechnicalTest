using System.Linq;
using UnityEngine;
using UnityEditor;

class MaterialAssigner : EditorWindow{
    //Tool path
    [MenuItem("Tools/Ozego/MaterialAssigner")]
    public static void ShowWindow(){
        EditorWindow.GetWindow(typeof(MaterialAssigner));
    }
    //Tool parameters
    string searchQuery = "Sphere";
    Material material;
    Editor materialEditor;
    
    void OnGUI(){
        //Title
        GUILayout.Label("Material Assigner", EditorStyles.boldLabel);
        //Searchbar
        searchQuery = EditorGUILayout.TextField("Prefab name:", searchQuery);
        //Material Selector
        EditorGUI.BeginChangeCheck();
        material = EditorGUILayout.ObjectField("Material:",material, typeof(Material),true) as Material;
        //Draw material preview with selection conditions
        if (EditorGUI.EndChangeCheck()){
            materialEditor = Editor.CreateEditor(material);
        }
        if (material != null){
            if (materialEditor == null){
                materialEditor = Editor.CreateEditor(material);
            }
            materialEditor.OnPreviewGUI(GUILayoutUtility.GetRect(300, 300), EditorStyles.toolbar);
        }
        //Execution button and functionality
        if (GUILayout.Button("Replace Materials")){
            //Search for GameObjects
            GameObject[] gameObjects = FindObjectsByName(searchQuery);
            //Check for GameObjects with searched name
            if (gameObjects == null) {
                EditorUtility.DisplayDialog("No GameObjects found","Try another search term","OK");
            } 
            //Check for assigned Material
            else if (material == null){
                EditorUtility.DisplayDialog("No Material Selected","Please select a material to be assigned","OK");
            } 
            //Apply Material to all GameObjects
            else {  
                foreach (var o in gameObjects){
                    var mr = o.GetComponent<MeshRenderer>();
                    if(mr!=null){
                        //Undo fuctionality
                        Undo.RecordObject(mr,"Change Material");
                        mr.material = material;
                        EditorUtility.SetDirty(mr);
                    }
                }
            }
        }
    }
    //Search function using Linq
    private static GameObject[] FindObjectsByName(string SearchQuery){
        return GameObject.FindObjectsOfType<GameObject>().Where(o => o.name == SearchQuery).ToArray();
    }
}
