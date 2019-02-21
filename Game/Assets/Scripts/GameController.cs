using UnityEngine;

public class GameController : MonoBehaviour{
    // GameGUI to be linked with main game loop
    public GameGUI gameGUI;
    // ArrayAnimator to be linked with main game loop
    public ArrayAnimator sphereArray;
    // Main GameLoop Tempo(Beats Per Minutes)
    public float BPM = 135;

    void Start(){
        SetTempo(BPM);
    }
    // Apply Tempo
    public void SetTempo(float BPM) {
        sphereArray.SetTempo(BPM);
        gameGUI.BPM = BPM;
    }
}
