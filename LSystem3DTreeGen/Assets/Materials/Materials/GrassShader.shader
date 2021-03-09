Shader "Unlit/GrassShader"{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _AnimationSpeed("Animation Speed", Range(0,5)) = 0
        _Clip("Clip", Range(0,1)) = 0
        _Frequency("Frequency", Range(0, 10)) = 0

    }
    SubShader
    {
        Tags {"Queue" = "Transparent"  "RenderType" = "Transparent" "IgnoreProjector" = "True"}

        Pass
            {  
            Cull Off
            Blend One OneMinusSrcAlpha
            ZWrite Off
               
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog


            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            float _Frequency;
            float _AnimationSpeed;
            float _Clip;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 diff : COLOR0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                v.vertex.z += sin(_Time.y * _AnimationSpeed) / _Frequency;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0.2, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a = 1;

                clip(_Clip - col.r * col.g * col.b);

                return col;// *i.diff / 2;
            }
            ENDCG
        }
    }
}
