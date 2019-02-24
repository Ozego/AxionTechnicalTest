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
    //Calculate amount of four vertex bezier curves are in spline
    public int CurveCount{
        get{
            return (points.Length-1)/3;
        }
    }
    public float EuclideanLenght{
        get{
            float euclideanLenght = 0f;
            for (int i = 0; i < CurveCount; i++)
            {
                euclideanLenght += CurveLenght(i);
            }
            return euclideanLenght;
        }
    }

    public float CurveLenght(int i)
    {
        return Bezier.GetLenght(points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3]);
    }

    //Read control point vertex
    public Vector3 GetControlPoint(int index){
        return points[index];
    }
    //Set control point vertex
    public void SetControlPoint(int index, Vector3 point){
        if(index%3==0){
            //vector from old to new position
            Vector3 delta=point-points[index];
            if(loop){
                //link last vertex to first vertex if looped
                if(index == 0){
                    points[1] += delta;
                    points[points.Length-2]+=delta;
                    points[points.Length-1]=point;
                }
                //link first vertex to last vertex if looped
                else if(index==points.Length-1){
                    points[0]=point;
                    points[1]+=delta;
                    points[index-1]+=delta;
                }
                //non linked vertexes
                else{
                    points[index-1]+=delta;
                    points[index+1]+=delta;
                }
            }
            else{
                //move adjecent control points with middle point
                if(index>0) points[index-1]+=delta;
                if(index+1<points.Length) points[index+1]+=delta;
            }
        }
        points[index]=point;
        EnforceConstraint(index);
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
        int modeIndex=(index+1)/3;
        modes[modeIndex]=mode;
        //Set modes for beginning/end if spline is loop
        if(loop){
            if(modeIndex == 0){
                modes[modes.Length-1]=mode;
            }
            else if(modeIndex==modes.Length-1){
                modes[0] = mode;
            }
        }
        EnforceConstraint(index);
    }
    //Looped spline option
    [SerializeField]
    private bool loop;
    public bool Loop{
        get{
            return loop;
        }
        set{
            loop = value;
            if(value==true){
                modes[modes.Length-1]=modes[0];
                SetControlPoint(0,points[0]);
            }
        }
    }
    //Enforce Constraints on control points
    private void EnforceConstraint(int index){
        int modeIndex = (index+1)/3;
        BezierControlPointMode mode = modes[modeIndex];
        //if free mode or not constructed enforce no constraints
        if(mode==BezierControlPointMode.Free||!loop&&(modeIndex==0||modeIndex==modes.Length-1)){
            return;
        }
        //sort index of bezier control points to be constrained with loop wrapping
        int middleIndex = modeIndex*3;
        int fixedIndex, enforcedIndex;
        //forwards
        if(index<=middleIndex){
            fixedIndex=middleIndex-1;
            if(fixedIndex<0) fixedIndex=points.Length-2;
            enforcedIndex=middleIndex+1;
            if(enforcedIndex>=points.Length) enforcedIndex=1;
        }
        //backwards
        else{
            fixedIndex=middleIndex+1;
            if(fixedIndex>=points.Length) fixedIndex=1;
            enforcedIndex=middleIndex-1;
            if(enforcedIndex<0) enforcedIndex=points.Length-2;
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
    //Worldpace position at point t along bezier indexed in spline
    public Vector3 GetPoint(int index, float t){
        int i=index*3;
        t=Mathf.Clamp01(t);
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
    //Derivative at point t along bezier indexed in spline
    public Vector3 GetVelocity(int index, float t){
        int i=index*3;
        t=Mathf.Clamp01(t);
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i],points[i+1],points[i+2],points[i+3],t))-transform.position;
    }
    //Get spline direction by normalized derivative
    public Vector3 GetDirection(float t){
        return GetVelocity(t).normalized;
    }
    //Get spline direction by normalized derivative
    public Vector3 GetDirection(int index, float t){
        return GetVelocity(index,t).normalized;
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
        //Enforce constraints
        EnforceConstraint(points.Length-4);
        //Wrap end of looped splines
        if (loop) {
            points[points.Length-1]=points[0];
            modes[modes.Length-1]=modes[0];
            EnforceConstraint(0);
        }
    }
}
