using UnityEngine;

public class ImpactAnimator : MonoBehaviour{
    //Animation Event function for selfdestruction
    void kill(){
        Destroy(transform.gameObject);
    }
}
