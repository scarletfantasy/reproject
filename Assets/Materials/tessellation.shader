Shader "Unlit/tessellation"
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
            #pragma vertex tessvert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma hull MyHullProgram
            #pragma domain MyDomainProgram
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct TessellationControlPoint {
	            float4 vertex : INTERNALTESSPOS;
	            
	            float2 uv : TEXCOORD0;

            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
            struct TessellationFactors {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
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
            TessellationControlPoint tessvert (appdata v)
            {
                TessellationControlPoint p;
	            p.vertex = v.vertex;
                p.uv = v.uv;
                return p;
            }
            float TessellationEdgeFactor (
	            TessellationControlPoint cp0, TessellationControlPoint cp1
            ) {
	            return 2.0f;
            }
            TessellationFactors MyPatchConstantFunction (InputPatch<TessellationControlPoint, 3> patch) {
	            TessellationFactors f;
                f.edge[0] = TessellationEdgeFactor(patch[1], patch[2]);
                f.edge[1] = TessellationEdgeFactor(patch[2], patch[0]);
                f.edge[2] = TessellationEdgeFactor(patch[0], patch[1]);
	            f.inside = (f.edge[0] + f.edge[1] + f.edge[2]) * (1 / 3.0);
	            return f;
            }
            [UNITY_domain("tri")]
            [UNITY_outputcontrolpoints(3)]
            [UNITY_outputtopology("triangle_cw")]
            [UNITY_partitioning("integer")]
            [UNITY_patchconstantfunc("MyPatchConstantFunction")]
            TessellationControlPoint MyHullProgram (
                InputPatch<TessellationControlPoint,3> patch,
                uint id : SV_OutputControlPointID               
            )
            {
                      return patch[id];          
            }


            #define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) data.fieldName = \
		        patch[0].fieldName * barycentricCoordinates.x + \
		        patch[1].fieldName * barycentricCoordinates.y + \
		        patch[2].fieldName * barycentricCoordinates.z;
            [UNITY_domain("tri")]
            v2f MyDomainProgram (
	            TessellationFactors factors,
	            OutputPatch<TessellationControlPoint, 3> patch,
	            float3 barycentricCoordinates : SV_DomainLocation
            ) {
	            appdata data;
                MY_DOMAIN_PROGRAM_INTERPOLATE(vertex);
                MY_DOMAIN_PROGRAM_INTERPOLATE(uv);
                return vert(data);
		            
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
