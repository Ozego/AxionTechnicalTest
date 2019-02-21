using UnityEngine;
using DG.Tweening;
public class EffectAnimator : MonoBehaviour
{
    //Impact effedt prefab
    public GameObject impact;
    //Animation Event function for instantioating Imoact effect
    public void GroundImpact(){
        // Check if object is wearing effect material
        if(GetComponentInChildren<MeshRenderer>().material.name=="Sphere (Instance)"){
            Instantiate(
                impact,
                //Project to XZ plane
                new Vector3(transform.position.x,0f,transform.position.z),
                //Randomize Y rotation
                Quaternion.Euler(0f,UnityEngine.Random.Range(0f,360f),0f)
            );
            // Camera shake depentend on distance to impact
            Camera.main.DOShakePosition(.75f,2.5f/Mathf.Pow(Vector3.Distance(transform.position,Camera.main.transform.position),2f),10,90f,true);
        }
    }
}
