using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BezierSpline))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SkinnedMeshSpline : MonoBehaviour{
    
    public SkinnedMeshRenderer Skin;
    public float SegmentLenght = 1f;
    private int WallSegments{
        get{
            float fractionalSegments = spline.EuclideanLenght/SegmentLenght;
            return (int)fractionalSegments;
        }
    }

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BezierSpline spline;
    private Mesh[] segmentMeshes;
    private CombineInstance[] combineInstances;
    // Start is called before the first frame update
    void Start()
    {

        meshFilter = GetComponent(typeof(MeshFilter)) as MeshFilter;
        meshRenderer = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        spline = GetComponent(typeof(BezierSpline)) as BezierSpline;
        //Make mesh to recieve data
        Mesh bakedMeshes = new Mesh();
        segmentMeshes = new Mesh[WallSegments];
        combineInstances = new CombineInstance[WallSegments];
        for (int i = 0; i < WallSegments; i++)
        {
            combineInstances[i] = new CombineInstance();
            combineInstances[i].mesh=MakeMeshSegment(i);
            combineInstances[i].transform=Matrix4x4.identity;
        };
        bakedMeshes.CombineMeshes(combineInstances);
        meshFilter.mesh=bakedMeshes;
    }

    private Mesh MakeMeshSegment(int index)
    {
        Mesh bakedMesh = new Mesh();
        bakedMesh.name = "bakedMesh";
        Skin.transform.position=transform.position;
        Skin.transform.rotation=transform.rotation;
        int boneSegments = WallSegments*2;
        
        //Left bone to be aligned to adjecet wall segments right bone
        BoneTransformer(Skin.bones[1],index*2-1,boneSegments); 
        //Root bone to be aligned to wall segment position
        BoneTransformer(Skin.bones[0],index*2  ,boneSegments); 
        //Right bone to be aligned to adjecet wall segments left bone
        BoneTransformer(Skin.bones[2],index*2+1,boneSegments); 

        Skin.BakeMesh(bakedMesh);
        return bakedMesh;
    }
    //Places bones
    private void BoneTransformer(Transform bone,int index, int boneSegments){
        bone.position=spline.GetPoint((float)(index)/(float)boneSegments);
        bone.LookAt(bone.position+spline.GetDirection((float)(index)/(float)boneSegments));
        bone.Rotate(0f,-90f,0f);
    }

    // Awake is called on fist frame
    void Awake(){

    }

    // Update is called once per frame
    void Update(){
        
    }
}
