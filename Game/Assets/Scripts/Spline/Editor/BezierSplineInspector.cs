﻿using UnityEngine;
using UnityEditor;
using System;

//Editor for spline creation
[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor{
    //Target spline and its corrosponding transform and rotation
    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    
    private const float directionScale = 10f;
    //Editor Inspector GUI
    public override void OnInspectorGUI(){
        //Target spline
        spline = target as BezierSpline;
        if(selectedIndex>=0&&selectedIndex<spline.ControlPointCount){
            DrawSelectedPointInspector();
        }

        //Add curve button
        if(GUILayout.Button("Add Curve")){
            //Undo functionality
            Undo.RecordObject(spline,"Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }
    //Editor Inspector GUI for selected vertex control point
    private void DrawSelectedPointInspector(){
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position",spline.GetControlPoint(selectedIndex));
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(spline,"Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex,point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode",spline.GetControlPointMode(selectedIndex));
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(spline,"Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }

    //Scene handle tools
    private void OnSceneGUI(){
        //Target spline
        spline = target as BezierSpline;
        //Read target transform
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation==PivotRotation.Local?handleTransform.rotation:Quaternion.identity;
        //Draw bezier vertecies and handles
        Vector3 p0 = ShowPoint(0);
        for(int i=1;i<spline.ControlPointCount;i+=3){
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i+1);
            Vector3 p3 = ShowPoint(i+2);
            //Draw bezier lines
            Handles.color = new Color(0f,1f,.75f,.5f);
            Handles.DrawLine(p0,p1);
            Handles.DrawLine(p2,p3);
            Handles.DrawBezier(p0,p3,p1,p2,Color.white, null, directionScale);
            p0=p3;
        }
    }
    //Handle parameters
    private const float handleSize=0.15f;
    private const float pickSize=0.1f;
    private int selectedIndex=-1;
    //Handle scene editor draw function
    private Vector3 ShowPoint(int index){
        //Target vertex
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        //Read scene editor handle size
        float size = HandleUtility.GetHandleSize(point);
        //Set handle color
        Handles.color=new Color(0f,1f,.75f,.5f);
        //Draw vertex
        if(Handles.Button(point,handleRotation,size*handleSize,size*pickSize,Handles.SphereHandleCap)){
            selectedIndex=index;
            //Update Inspector GUI
            Repaint();
        }
        //Draw handle
        if(selectedIndex==index){
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point,handleRotation);
            if(EditorGUI.EndChangeCheck()){
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        //Return world position
        return point;
    }
}