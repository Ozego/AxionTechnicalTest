﻿using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Performance Manager checks framerate and dynamically sets quality settings.
// It will progressively lower quality setting until target is reched.
public class PerformanceManager : MonoBehaviour
{
    [Tooltip("Target framerate of game")]
    public float TargetFramerate = 60f;
    [Tooltip("Error margin in sampled framerate")]
    public float SampleMargin = 5f;
    [Tooltip("Interval in seconds between quality checks.")]
    public float CheckInterval = 0.5f;
    
    private ReflectionProbe[] reflectionProbes;

    void Awake(){
        //Get all reflection probes
        reflectionProbes = GameObject.FindObjectsOfType<ReflectionProbe>();
        //Set Game target Framerated
        Application.targetFrameRate = (int)TargetFramerate;
        //Initiate delayed performance checks
        StartCoroutine(DelayedPerformanceChecks());
    }
    //Coroutine for asyncronous perfomance checks
    private IEnumerator DelayedPerformanceChecks(){
        //High Quality Pass
        SetHighQuality();
        yield return new WaitForSeconds(CheckInterval);
        //Check performance of high quality
        if(CheckIfPerformancePoor()){
            //Medium Quality Pass
            SetMediumQuality();
            //Check performance of medium quality
            yield return new WaitForSeconds(CheckInterval);
            if(CheckIfPerformancePoor()){
                //Low Quality Pass
                SetLowQuality();
            }
        }
        //Performance Quality Settings Set
        Destroy(gameObject);
        yield break;
    }
    //Checks fps against target
    private bool CheckIfPerformancePoor(){
        return 1f/(TargetFramerate-SampleMargin)>1f/Time.smoothDeltaTime;
    }
    //Method for setting high quality
    private void SetHighQuality(){
        foreach (var reflectionProbe in reflectionProbes){
            //Refresh reflections every frame
            reflectionProbe.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        }
    }
    //Method for setting medium quality
    private void SetMediumQuality(){
        foreach (var reflectionProbe in reflectionProbes){
            //Refresh reflections every 9th frame
            reflectionProbe.timeSlicingMode = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
        }
    }
    //Method for setting low quality
    private void SetLowQuality(){
        foreach (var reflectionProbe in reflectionProbes){
            //Refresh reflections every 14th frame
            reflectionProbe.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;
        }
    }
}

