using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Simplified bezier system derived from tutorial by Catlike Coding.
[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor{

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    
    private const int lineSteps = 16;
    private const float directionScale = 10f;

    private void OnSceneGUI(){
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation==PivotRotation.Local?handleTransform.rotation:Quaternion.identity;
        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);
        Handles.color = Color.white;
        Handles.DrawLine(p0,p1);
        Handles.DrawLine(p2,p3);
        Handles.DrawBezier(p0,p3,p1,p2,new Color(0f,1f,.5f,.5f), null, directionScale);
    }
    private Vector3 ShowPoint(int index){
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point,handleRotation);
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}