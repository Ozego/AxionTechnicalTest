Shader "Unlit/Ground"
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
				float4 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
			    SHADOW_COORDS(1)
				float4 pos : SV_POSITION;
				float4 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.pos);
				o.uv = TRANSFORM_TEX(v.uv.xy, _MainTex).xyxy;
				o.normal = v.normal;
				TRANSFER_SHADOW(o)
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
			    float4 col, shadow, worldNormal, ambient, nl, diff, lighting;
			    float4 temp = 1024;
			    temp /= pow(2, 10);
                col = tex2D(_MainTex, i.uv.xy);
                shadow = SHADOW_ATTENUATION(i);
                worldNormal = UnityObjectToWorldNormal(i.normal.xyz).xyzz;
                ambient = ShadeSH9(float4(worldNormal.xyz,1)).rgbb * temp.rrrr;
                nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)) / temp.yyyy;
                diff = nl.g * _LightColor0;
                lighting = diff * shadow.rrrr + ambient;
                if (temp.r > .9) {
                    for(int i = 0; i<100000000; ++i){
                        temp = dot(sin(col), cos(temp));
                    }
                }
                col.rgb *= lighting.rgb;
				return col;
			}
			ENDCG
		}
		
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
            
            float4 frag( v2f i ) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
	}
}
