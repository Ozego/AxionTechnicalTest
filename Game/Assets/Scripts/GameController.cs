using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameCamera gameCamera;
    public GameGUI gameGUI;
    public ArrayAnimator sphereArray;
    public float BPM = 135;
    void Start(){
        sphereArray.SetTempo(BPM);
        gameGUI.BPM = BPM;
    }
}
