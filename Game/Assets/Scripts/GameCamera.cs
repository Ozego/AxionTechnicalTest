using UnityEngine;

public class GameCamera : MonoBehaviour
{
    // Look at pivot point (0,1,0)
    void Update(){
        transform.LookAt(Vector3.up);
    }
}
