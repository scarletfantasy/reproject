Shader "Unlit/reprojectwithadaptive"
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
            #pragma vertex tessvert
            #pragma fragment frag
            #pragma hull MyHullProgram
            #pragma domain MyDomainProgram
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
            struct TessellationControlPoint {
	            float4 vertex : INTERNALTESSPOS;
	            
	            float2 uv : TEXCOORD0;

            };
            
            struct TessellationFactors {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };
            sampler2D _MainTex;            
            float4 _MainTex_ST;
            sampler2D _depth;
            float2 _depth_TexelSize;
            sampler2D _color;
            sampler2D _sobel;
            float4x4 maininvp;
            float3 getworldpos(float2 uv)
            {
                float3 normal;
	            float depth;
                //DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv), depth, normal);
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
	            if(abs(getworldpos(cp0.uv).z-getworldpos(cp1.uv).z)>0.0001)
                {
                    return 2.0f;
                }
                else{
                    return 1.0f;
                }
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
            ) 
            {
	            appdata data;
                MY_DOMAIN_PROGRAM_INTERPOLATE(vertex);
                MY_DOMAIN_PROGRAM_INTERPOLATE(uv);
                return vert(data);		            
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                if(abs(tex2D(_sobel,i.uv).x)>0.1)
                {
                    return float4(0.0,1.0,0.0,1.0);
                }
                else{
                    return tex2D(_color,i.uv);
                }
                //return tex2D(_color,i.uv);
                
            }
            ENDCG
        }
    }
}
