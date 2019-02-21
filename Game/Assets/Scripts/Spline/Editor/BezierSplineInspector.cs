using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Simplified bezier system derived from tutorial by Catlike Coding.
[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor{

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    
    private const float directionScale = 10f;
    
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        spline = target as BezierSpline;
        if(GUILayout.Button("Add Curve")){
            
            Undo.RecordObject(spline,"Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }
    private void OnSceneGUI(){
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation==PivotRotation.Local?handleTransform.rotation:Quaternion.identity;
        Vector3 p0 = ShowPoint(0);
        for(int i=1;i<spline.points.Length;i+=3){
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i+1);
            Vector3 p3 = ShowPoint(i+2);
            Handles.color = Color.white;
            Handles.DrawLine(p0,p1);
            Handles.DrawLine(p2,p3);
            Handles.DrawBezier(p0,p3,p1,p2,new Color(0f,1f,.75f,.5f), null, directionScale);
            p0=p3;
        }
    }
    private const float handleSize=0.15f;
    private const float pickSize=0.1f;
    private int selectedIndex=-1;
    private Vector3 ShowPoint(int index){
        Vector3 point = handleTransform.TransformPoint(spline.points[index]);
        float size = HandleUtility.GetHandleSize(point);
        Handles.color=new Color(0f,1f,.75f,.5f);
        if(Handles.Button(point,handleRotation,size*handleSize,size*pickSize,Handles.SphereHandleCap)){
            selectedIndex=index;
        }
        if(selectedIndex==index){
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point,handleRotation);
            if(EditorGUI.EndChangeCheck()){
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.points[index] = handleTransform.InverseTransformPoint(point);
            }
        }
        return point;
    }
}