using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    void Awake(){
        
    }

    void Update(){
        transform.LookAt(Vector3.up);
    }
}
