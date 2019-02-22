using UnityEngine;
using UnityEditor;

//Editor for spline creation
[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor{

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    
    private const float directionScale = 10f;
    //Editor Inspector GUI
    public override void OnInspectorGUI(){
        //Target spline
        spline = target as BezierSpline;
        //Draw default GUI
        DrawDefaultInspector();
        //Add curve button
        if(GUILayout.Button("Add Curve")){
            //Undo functionality
            Undo.RecordObject(spline,"Add Curve");
            spline.AddCurve();
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
        for(int i=1;i<spline.points.Length;i+=3){
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i+1);
            Vector3 p3 = ShowPoint(i+2);
            //Draw bezier lines
            Handles.color = Color.white;
            Handles.DrawLine(p0,p1);
            Handles.DrawLine(p2,p3);
            Handles.DrawBezier(p0,p3,p1,p2,new Color(0f,1f,.75f,.5f), null, directionScale);
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
        Vector3 point = handleTransform.TransformPoint(spline.points[index]);
        //Read scene editor handle size
        float size = HandleUtility.GetHandleSize(point);
        //Set handle color
        Handles.color=new Color(0f,1f,.75f,.5f);
        //Draw vertex
        if(Handles.Button(point,handleRotation,size*handleSize,size*pickSize,Handles.SphereHandleCap)){
            selectedIndex=index;
        }
        //Draw handle
        if(selectedIndex==index){
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point,handleRotation);
            if(EditorGUI.EndChangeCheck()){
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.points[index] = handleTransform.InverseTransformPoint(point);
            }
        }
        //Return world position
        return point;
    }
}