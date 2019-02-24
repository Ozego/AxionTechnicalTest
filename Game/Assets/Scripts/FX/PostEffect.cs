using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostEffect : MonoBehaviour{
    public RenderTexture buffer;
    void OnRenderImage(RenderTexture source, RenderTexture destination){
        // Graph
        // buffer = RenderTexture.GetTemporary(source.width,source.height,0,source.format);
        // Graphics.Blit(source,buffer);
        // Graphics.Blit(buffer,destination);
    }
}
