using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BezierSpline))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SkinnedMeshSpline : MonoBehaviour{
    
    public SkinnedMeshRenderer skin;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BezierSpline spline;

    // Start is called before the first frame update
    void Start(){
        meshFilter = GetComponent(typeof(MeshFilter)) as MeshFilter;
        meshRenderer = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        spline = GetComponent(typeof(BezierSpline)) as BezierSpline;
        Mesh bakedMesh = new Mesh();
        for (int i = 0; i < skin.bones.Length; i++){
            // skin.bones[i].position=spline.GetPoint(.01f*(float)i);
            // skin.bones[i].rotation=Quaternion.LookRotation( spline.GetDirection(.01f*(float)i));
        }
        bakedMesh.name="bakedMesh";
        skin.BakeMesh(bakedMesh);
        meshFilter.mesh=bakedMesh;

        // skin.bones[0].position=Vector3.zero;
    }
    // Awake is called on fist frame
    void Awake(){

    }

    // Update is called once per frame
    void Update(){
        
    }
}
