Shader "Unlit/sobel"
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
            float2 _MainTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            float calsobel(float2 uv)
            {
                float gx=tex2D(_MainTex, float2(uv.x-_MainTex_TexelSize.x,uv.y-_MainTex_TexelSize.y)).x*-1.0+
                tex2D(_MainTex, float2(uv.x-_MainTex_TexelSize.x,uv.y)).x*-2.0+
                tex2D(_MainTex, float2(uv.x-_MainTex_TexelSize.x,uv.y+_MainTex_TexelSize.y)).x*-1.0+
                tex2D(_MainTex, float2(uv.x+_MainTex_TexelSize.x,uv.y-_MainTex_TexelSize.y)).x*1.0+
                tex2D(_MainTex, float2(uv.x+_MainTex_TexelSize.x,uv.y)).x*2.0+
                tex2D(_MainTex, float2(uv.x+_MainTex_TexelSize.x,uv.y+_MainTex_TexelSize.y)).x*1.0;

                float gy=tex2D(_MainTex, float2(uv.x-_MainTex_TexelSize.x,uv.y-_MainTex_TexelSize.y)).x*-1.0+
                tex2D(_MainTex, float2(uv.x,uv.y-_MainTex_TexelSize.y)).x*-2.0+
                tex2D(_MainTex, float2(uv.x+_MainTex_TexelSize.x,uv.y-_MainTex_TexelSize.y)).x*-1.0+
                tex2D(_MainTex, float2(uv.x-_MainTex_TexelSize.x,uv.y+_MainTex_TexelSize.y)).x*1.0+
                tex2D(_MainTex, float2(uv.x,uv.y+_MainTex_TexelSize.y)).x*2.0+
                tex2D(_MainTex, float2(uv.x+_MainTex_TexelSize.x,uv.y+_MainTex_TexelSize.y)).x*1.0;

                return abs(gx)+abs(gy);
            }
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                float result=calsobel(i.uv);
                return float4(result,result,result,1.0);
            }
            ENDCG
        }
    }
}
