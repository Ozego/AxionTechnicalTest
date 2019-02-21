using UnityEngine;
using TMPro;

public class GameGUI : MonoBehaviour{
    // Terminal to write game statistics to
    public TextMeshProUGUI terminal;
    [HideInInspector] public float BPM;
    void Awake(){
        Initialize();
    }
    
    void Update(){
        // Apply game statistics to terminal
        terminal.text = string.Format("{0:0.00} FPS \n{1:0.00} BPM",1f/Time.deltaTime, BPM);
    }
    // Clear input in terminal
    void Initialize(){
        terminal.text = "GUI Intialized";
    }
}
