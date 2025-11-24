Shader "Unlit/ImageRoundCorner"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // 四个角的半径
        _Radius ("Corner Radius (LTRB)", Vector) = (0.1, 0.1, 0.1, 0.1)

        // 边框
        _BorderWidth ("Border Width", Float) = 0.0
        _BorderColor ("Border Color", Color) = (1,1,1,1)

        // 全局透明度
        _Alpha ("Alpha", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            fixed4 _Color;

            float4 _Radius; // 左上、右上、右下、左下 半径
            float _BorderWidth;
            fixed4 _BorderColor;
            float _Alpha;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                float2 uv = i.uv;
                float width = 1.0;
                float height = 1.0;

                float rL = _Radius.x; // 左上
                float rR = _Radius.y; // 右上
                float rB = _Radius.z; // 右下
                float rBL = _Radius.w; // 左下

                float b = _BorderWidth;

                // 四个角圆角+边框处理
                float2 p = uv;

                // 左下角
                if (p.x < rBL && p.y < rBL)
                {
                    float d = distance(p, float2(rBL, rBL));
                    if (d > rBL) col.a = 0;
                    else if (b > 0 && d > rBL - b) col = _BorderColor;
                }

                // 左上角
                if (p.x < rL && p.y > 1 - rL)
                {
                    float d = distance(p, float2(rL, 1 - rL));
                    if (d > rL) col.a = 0;
                    else if (b > 0 && d > rL - b) col = _BorderColor;
                }

                // 右下角
                if (p.x > 1 - rB && p.y < rB)
                {
                    float d = distance(p, float2(1 - rB, rB));
                    if (d > rB) col.a = 0;
                    else if (b > 0 && d > rB - b) col = _BorderColor;
                }

                // 右上角
                if (p.x > 1 - rR && p.y > 1 - rR)
                {
                    float d = distance(p, float2(1 - rR, 1 - rR));
                    if (d > rR) col.a = 0;
                    else if (b > 0 && d > rR - b) col = _BorderColor;
                }

                // 透明度控制
                col.a *= _Alpha;

                return col;
            }
            ENDCG
        }
    }
}