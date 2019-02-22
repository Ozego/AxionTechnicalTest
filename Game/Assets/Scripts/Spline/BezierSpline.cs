using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour{
    //Control point vertex array
    [SerializeField]
    private Vector3[] points;
    //Read control point array lenght
    public int ControlPointCount{
        get{
            return points.Length;
        }
    }
    //Read control point vertex
    public Vector3 GetControlPoint(int index){
        return points[index];
    }
    //Set control point vertex
    public void SetControlPoint(int index, Vector3 point){
        points[index]=point;
        EnforceMode(index);
    }
    //Control point constraint mode array
    [SerializeField]
    private BezierControlPointMode[] modes;
    //Read control point mode of control point
    public BezierControlPointMode GetControlPointMode(int index){
        return modes[(index+1)/3];
    }
    //Set control point mode of control point
    public void SetControlPointMode(int index, BezierControlPointMode mode){
        modes[(index+1)/3]=mode;
        EnforceMode(index);
    }
    //Enforce Constraints on control points
    private void EnforceMode(int index){
        int modeIndex = (index+1)/3;
        BezierControlPointMode mode = modes[modeIndex];
        //if free mode or not constructed enforce no constraints
        if(mode==BezierControlPointMode.Free||modeIndex==0||modeIndex==modes.Length-1){
            return;
        }
        //sort index of bezier control points to be constrained
        int middleIndex = modeIndex*3;
        int fixedIndex, enforcedIndex;
        if(index<=middleIndex){
            fixedIndex=middleIndex-1;
            enforcedIndex=middleIndex+1;
        }
        else{
            fixedIndex=middleIndex+1;
            enforcedIndex=middleIndex-1;
        }
        //Middle control point vertex position
        Vector3 middle=points[middleIndex];
        //Calculate tangent of the constrained control point by mirroring fixed control point around middle control point
        Vector3 enforcedTangent=middle-points[fixedIndex];
        //Scale tangent to original enforced tangent lenght if in Aligned mode
        if(mode==BezierControlPointMode.Aligned){
            enforcedTangent = enforcedTangent.normalized*Vector3.Distance(middle,points[enforcedIndex]);
        }
        //Apply Constraint on enforced point
        points[enforcedIndex]=middle+enforcedTangent;
    }
    //Single 4 vertex control point bezier curve spline initiation
    public void Reset(){
        points =  new Vector3[]{
            new Vector3(1f,0f,0f),
            new Vector3(2f,0f,0f),
            new Vector3(3f,0f,0f),
            new Vector3(4f,0f,0f)
        };
        modes = new BezierControlPointMode[]{
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };
    }
    //Worldspace position at point t along spline
    public Vector3 GetPoint(float t){
        int i;
        if(t>=1f){
            t=1f;
            i=points.Length-4;
        }
        else{
            t=Mathf.Clamp01(t)*CurveCount;
            i=(int)t;
            t-=i;
            i*=3;
        }
        return transform.TransformPoint(Bezier.GetPoint(points[i],points[i+1],points[i+2],points[i+3],t));
    }
    //Derivative at point t of spline
    public Vector3 GetVelocity(float t){
                int i;
        if(t>=1f){
            t=1f;
            i=points.Length-4;
        }
        else{
            t=Mathf.Clamp01(t)*CurveCount;
            i=(int)t;
            t-=i;
            i*=3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i],points[i+1],points[i+2],points[i+3],t))-transform.position;
    }
    //Get spline direction by normalized derivative
    public Vector3 GetDirection(float t){
        return GetVelocity(t).normalized;
    }
    //Calculate amount of four vertex bezier curves are in spline
    public int CurveCount{
        get{
            return (points.Length-1)/3;
        }
    }
    //Add four vertex bezier curve to spline
    public void AddCurve(){
        //Add vertecies to array
        Vector3 point = points[points.Length-1];
        Array.Resize(ref points, points.Length+3);
        point.x+=1f;
        points[points.Length-3]=point;
        point.x+=1f;
        points[points.Length-2]=point;
        point.x+=1f;
        points[points.Length-1]=point;
        //Add control point mode to array
        Array.Resize(ref modes, modes.Length+1);
        modes[modes.Length-1]=modes[modes.Length-2];
    }
}
