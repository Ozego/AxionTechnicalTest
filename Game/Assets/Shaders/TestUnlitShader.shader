Shader "Unlit/TestUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        //Although an entire shadowpass is missing the entire pass can be referenced from the ShaderLab library with a usepass
        //Custom Usepasses can be made with SubShaders and be applied in similar manner to keep dependency amongst shaders
        UsePass "VertexLit/SHADOWCASTER"
        
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 _Color;
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture with UV animation
                //don't mind this. I can't help it; you made me open the shader. You can't have bouncing checkered spheres in a demo without some animation
                fixed4 col = tex2D(_MainTex, float2(i.uv.x+_Time.y,i.uv.y+_SinTime.y));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col * _Color;
            }
            ENDCG
        }
        
    }
}
