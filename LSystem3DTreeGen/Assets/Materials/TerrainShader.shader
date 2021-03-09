// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/TerrainShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RockTex ("RockTexture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _GrassColor("GrassColor", Color) = (1,1,1,1)
        _RockColor("RockColor", Color) = (1,1,1,1)
        _BumpMap("Rock Normal Map", 2D) = "bump" {}
        _GrassMap("Grass Normal Map", 2D) = "bump" {}
       
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" 
                "PassFlags" = "OnlyDirectional"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                half3 tspace0 : TEXCOORD1; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD2; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD3; // tangent.z, bitangent.z, normal.z
                float3 worldPos : TEXCOORD4;
            };

            sampler2D _MainTex;
            sampler2D _RockTex;
            sampler2D _BumpMap;
            sampler2D _GrassMap;
            fixed4 _Color;
            fixed4 _GrassColor;
            fixed4 _RockColor;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                half3 wNormal = UnityObjectToWorldNormal(v.normal);
                half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) * 1;
                o.normal = v.normal;
                UNITY_TRANSFER_FOG(o,o.vertex);
                // compute bitangent from cross product of normal and tangent
                half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                // output the tangent space matrix
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {


                if (i.normal.g < .8) {
                    // sample the normal map, and decode from the Unity encoding
                    half3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv));
                    // transform normal from tangent to world space
                    half3 worldNormal;
                    worldNormal.x = dot(i.tspace0, tnormal);
                    worldNormal.y = dot(i.tspace1, tnormal);
                    worldNormal.z = dot(i.tspace2, tnormal);
                    // sample the texture
                    //fixed4 col = tex2D(_MainTex, i.uv);
                    float3 normal = normalize(worldNormal);
                    float NdotL = dot(_WorldSpaceLightPos0, worldNormal);

                    float lightIntensity = smoothstep(0, 0.01, NdotL);
                    return tex2D(_RockTex, i.uv) * ((_LightColor0 + lightIntensity)/2);
                    //return fixed4(i.normal * .5 + .5,1);
                    //return _Color;
                }
                else {
                    // sample the normal map, and decode from the Unity encoding
                    half3 tnormal = UnpackNormal(tex2D(_GrassMap, i.uv));
                    // transform normal from tangent to world space
                    half3 worldNormal;
                    worldNormal.x = dot(i.tspace0, tnormal);
                    worldNormal.y = dot(i.tspace1, tnormal);
                    worldNormal.z = dot(i.tspace2, tnormal);
                    // sample the texture
                    //fixed4 col = tex2D(_MainTex, i.uv);
                    float3 normal = normalize(worldNormal);
                    float NdotL = dot(_WorldSpaceLightPos0, worldNormal);

                    float lightIntensity = smoothstep(0, 0.01, NdotL);
                    return tex2D(_MainTex, i.uv) * ((_LightColor0 + lightIntensity)/2);
                    //return fixed4(i.normal * .5 + .5,1);
                }
                
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
