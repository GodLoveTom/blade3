Shader "Unlit/test"
{
    Properties
    {
        [HideInInspector] _MainTex ("Diffuse", 2D) = "white" {}
        _Color0("Color0", Color) = (0, 0, 0, 0)
        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
        Cull Off


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
                float4 color		: COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4  color			: COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color0;
            //float4 _Color;            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                col.rgb = col.rgb * i.color.rgb +  _Color0.rgb;
                col.a = col.a * i.color.a;
                //if( _Color0.a == 1)
                   // col.rgb = dot(col.rgb, fixed3(1, 1, 1));
                //col.g = col.g ;
                //col.b = col.g ;
                //half4 c = _Color0 ;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, c);
                return col;
            }
            ENDCG
        }
    }
    Fallback "Sprites/Default"
}
