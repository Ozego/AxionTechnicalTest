Shader "Unlit/Ground" //This shader is not really unlit is it now?
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
             
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 pos : POSITION;			 	
                float2 uv : TEXCOORD0;	//we use 2D textures in the material, but I imagine 4D textures would look stunning
                float3 normal : NORMAL; //normals are 3 components: normal, tangent and binormal
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; //2D textures here as well
                SHADOW_COORDS(1)
                float4 pos : SV_POSITION;
                float3 normal : NORMAL; //3 component normals
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); //2D textures here as too
                o.normal = v.normal;
                TRANSFER_SHADOW(o)
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                //we do not need 32bit precission in the fragment shader. 11bit fixed will work just as well
                //many of these variables are not vectors but still stored as 4 component vectors, let us make efficient use of memory
                //we do not need to store some of these variables at all, the compiler is clever, but we do not need to challenge it
                fixed4 col; //to keep alpha or not to keep alpha. I decided to keep it since it was sampled and passed to output in the original. Although this is only visible when opening the frame debugger it could have impact on the functionality of the shader. stored alphas can be used in posteffects
                fixed3 ambient, worldNormal, lighting;
                fixed nl, shadow;
                //float4 temp = 1024; we don't want to create a new constant variable on each fragment call lets store constants outside. also this should be an int not a float4
                //temp /= pow(2, 10); 2^10 = 1024, now temp is equal to 1... this has no functionality so I will not feel bad about removing all references to temp from here on
                col = tex2D(_MainTex, i.uv); // would be tex2D(_MainTex, i.uv).rgb if we do not need alpha output
                worldNormal = UnityObjectToWorldNormal(i.normal);
                ambient = ShadeSH9(fixed4(worldNormal,1)); // times 1
                nl = max(0, dot(worldNormal, _WorldSpaceLightPos0)); // divided by 1
                shadow = SHADOW_ATTENUATION(i);
                lighting = nl * shadow * _LightColor0 + ambient;
                
                //if (temp.r > .9) { 						//branching is not good for shaders, especially in fragment part; anyway 1 is always larger than .9
                //    for(int i = 0; i<100000000; ++i){ 	//the calculation within is overwriting temp with the same result. there is no reason for a for() loop
                //        temp = dot(sin(col), cos(temp)); 	//there are no further use of temp so changing it is useless here
                //    }
                //}
                col.rgb *= lighting;
                return col;
            }
            ENDCG
        }
        
        //we could use UsePass "VertexLit/SHADOWCASTER" here; but although this is more verbose the perfomance should be the same 
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"
            
            struct v2f {
                V2F_SHADOW_CASTER;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            v2f vert( appdata_base v )
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }
            //except for fragment shader precission
            fixed4 frag( v2f i ) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
