using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour{
    //Vertex array
    public Vector3[] points;
    //Single 4 vertex bezier curve spline initiation
    public void Reset(){
        points =  new Vector3[]{
            new Vector3(1f,0f,0f),
            new Vector3(2f,0f,0f),
            new Vector3(3f,0f,0f),
            new Vector3(4f,0f,0f)
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
        Vector3 point = points[points.Length-1];
        Array.Resize(ref points, points.Length+3);
        point.x+=1f;
        points[points.Length-3]=point;
        point.x+=1f;
        points[points.Length-2]=point;
        point.x+=1f;
        points[points.Length-1]=point;
    }
}
