using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EffectAnimator : MonoBehaviour
{
    public GameObject impact;
    public void GroundImpact(){
        if(GetComponentInChildren<MeshRenderer>().material.name=="Sphere (Instance)"){
            Instantiate(
                impact,
                new Vector3(transform.position.x,0f,transform.position.z),
                Quaternion.Euler(0f,UnityEngine.Random.Range(0f,360f),0f)
            );
            Camera.main.DOShakePosition(.75f,2.5f/Mathf.Pow(Vector3.Distance(transform.position,Camera.main.transform.position),2f),10,90f,true);
        }
    }
}
