using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameGUI : MonoBehaviour
{
    public TextMeshProUGUI terminal;
    [HideInInspector] public float BPM;
    void Awake(){
        Initialize();
    }


    void Update(){
        terminal.text = string.Format("{0:0.00} FPS \n{1:0.00} BPM",1f/Time.deltaTime, BPM);
    }

    void Initialize(){
        terminal.text = "GUI Intialized";
    }
}
