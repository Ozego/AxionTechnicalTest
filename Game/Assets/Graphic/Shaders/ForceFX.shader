Shader "Ozego/ForceFX"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _t ("Animation Time", float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent"  "RenderType"="Transparent" }
        Blend One OneMinusSrcAlpha
        Cull Off Lighting Off ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 _Color;
            float _t;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample color attribute
                fixed4 col = _Color;
                // sample the texture
                fixed2 distortion = tex2D(_MainTex, fixed2(i.uv.x,i.uv.y-_Time.y*3)).gb;
                fixed4 tex = tex2D(_MainTex, fixed2(i.uv.x,clamp(0,1,i.uv.y-distortion.y*.5)));
                fixed heightMask = smoothstep(0,.1,i.worldPos.y)*smoothstep(1,.25,i.worldPos.y);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                // apply transparency from texture distance channel .r 
                col *= col.a*smoothstep(1-_t-.1,1-_t+.1,tex.r*heightMask);
                return col;
            }
            ENDCG
        }
    }
}
