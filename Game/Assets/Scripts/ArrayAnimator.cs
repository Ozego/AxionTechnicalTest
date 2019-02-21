using System.Collections;
using UnityEngine;

public class ArrayAnimator : MonoBehaviour{   
    // Enumeration of available array animation delay functions
    private enum DelayTypes{RandomStart,RandomDelay,SerialStart,SerialDelay}
    [SerializeField][Tooltip("Select the type of delay for your animations. You can randomize them or cycle through them according to their index in your animation array. You can chose to delay the onset of animations or start them inside the timeline.")]
    private DelayTypes delayType = DelayTypes.SerialStart;

    // Array of animations to be delayed
    public Animator[] animations;

    void Awake(){
        // Switch type for choosing delay function. 
        // Dictionary would be more efficient but enum serializes to dropdown menu without custom editor scripting which makes scene setup easy.
        switch (delayType){
            case DelayTypes.RandomStart: RandomStart(); break;
            case DelayTypes.RandomDelay: RandomDelay(); break;
            case DelayTypes.SerialStart: SerialStart(); break;
            case DelayTypes.SerialDelay: SerialDelay(); break;
            default: Debug.Log("DelayType not found."); break;
        }
    }
    // I like to work to music so I did a small script to sync the animations to whatever I am listening to.
    // TODO: Tap to bpm functionality
    public void SetTempo(float BPM) {
        foreach (var animation in animations){
            animation.speed = BPM/(60f*animation.GetCurrentAnimatorClipInfo(0).Length);
        }
    }
    // Randomizes the starting position in the Animation curves timeline.
    private void RandomStart(){
        foreach (var animation in animations){
            animation.Play(0,0,UnityEngine.Random.Range(0f,1f));
        }
    }
    // Serializes the starting position in the Animation curves timeline.
    private void SerialStart(){
        for (int i = 0; i < animations.Length; i++){
            Animator animation = animations[i];
            animation.Play(0,0,(float)i/(float)animations.Length);
        }
    }
    // Randomizes a delay on each Animation
    private void RandomDelay(){
        foreach (var animation in animations){
            animation.enabled = false;
            StartCoroutine(DelayedEnable(animation,1f+UnityEngine.Random.Range(1f,2f)));
        }
    }
    // Serializes a delay on each Animation
    private void  SerialDelay(){
        for (int i = 0; i < animations.Length; i++){
            Animator animation = animations[i];
            animation.enabled = false;
            StartCoroutine(DelayedEnable(animation,(1f+(float)i)/(float)animations.Length));
        }
    }
    // Delayed function for enabling animator object
    private IEnumerator DelayedEnable(Animator animation, float delay){
        yield return new WaitForSeconds(delay);
        animation.enabled = true;
    }
}
