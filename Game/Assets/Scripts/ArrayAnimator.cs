using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayAnimator : MonoBehaviour{   
    
    private enum DelayTypes{RandomStart,RandomDelay,SerialStart,SerialDelay}
    [SerializeField][Tooltip("Select the type of delay for your animations. You can randomize them or cycle through them according to their index in your animation array. You can chose to delay the onset of animations or start them inside the timeline.")]
    private DelayTypes delayType = DelayTypes.SerialStart;

    public Animator[] animations;

    void Awake(){
        switch (delayType){
            case DelayTypes.RandomStart: RandomStart(); break;
            case DelayTypes.RandomDelay: RandomDelay(); break;
            case DelayTypes.SerialStart: SerialStart(); break;
            case DelayTypes.SerialDelay: SerialDelay(); break;
            default: Debug.Log("DelayType not found."); break;
        }
    }

    public void SetTempo(float BPM) {
        foreach (var animation in animations){
            animation.speed = BPM/(60f*animation.GetCurrentAnimatorClipInfo(0).Length);
        }
    }
    private void RandomStart(){
        foreach (var animation in animations){
            animation.Play(0,0,UnityEngine.Random.Range(0f,1f));
        }
    }
    private void SerialStart(){
        for (int i = 0; i < animations.Length; i++){
            Animator animation = animations[i];
            animation.Play(0,0,(float)i/(float)animations.Length);
        }
    }
    private void RandomDelay(){
        foreach (var animation in animations){
            animation.enabled = false;
            StartCoroutine(DelayedEnable(animation,1f+UnityEngine.Random.Range(1f,2f)));
        }
    }
    private void  SerialDelay(){
        for (int i = 0; i < animations.Length; i++){
            Animator animation = animations[i];
            animation.enabled = false;
            StartCoroutine(DelayedEnable(animation,(1f+(float)i)/(float)animations.Length));
        }
    }
    private IEnumerator DelayedEnable(Animator animation, float delay){
        yield return new WaitForSeconds(delay);
        animation.enabled = true;
    }
}
