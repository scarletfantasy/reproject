Shader "reproject/reproject"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _depth("Texture",2D)="white"{}
        _color("Texture",2D)="white"{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #pragma enable_d3d11_debug_symbols
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
                float4 color:COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _depth;
            sampler2D _color;
            float4x4 maininvp;
            float3 getworldpos(float2 uv)
            {
                float3 normal;
	            float depth;
                float4 tmp= tex2Dlod(_depth,float4(uv,0,0));
                depth=1-tmp.x;
                float4 loc=float4(2*uv.x-1,uv.y*2-1,depth*2-1,1.0);
                float4 eyepos=mul(maininvp,loc);
                eyepos=eyepos/eyepos.w;
                return eyepos.xyz;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityWorldToClipPos(getworldpos(v.uv));
                o.uv = v.uv;
                o.color=tex2Dlod(_color,float4(v.uv,0,0));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return tex2D(_color,i.uv);
            }
            ENDCG
        }
    }
}
