Shader "Hidden/WhiteOutlineImageEffectShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _EdgeThickness ("Edge Thickness", Range(0, 1)) = 0.05
        _Threshold ("Alpha Threshold", Range(0, 1)) = 0.5 // 新增Alpha阈值属性
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _EdgeThickness;
            float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);

                // 检查当前像素是否透明或半透明
                if (col.a < _Threshold)
                {
                    discard; // 忽略透明像素
                }

                // 使用Wrap模式自动处理边界情况
                half4 left = tex2D(_MainTex, float2(i.uv.x - _EdgeThickness, i.uv.y));
                half4 right = tex2D(_MainTex, float2(i.uv.x + _EdgeThickness, i.uv.y));
                half4 top = tex2D(_MainTex, float2(i.uv.x, i.uv.y + _EdgeThickness));
                half4 bottom = tex2D(_MainTex, float2(i.uv.x, i.uv.y - _EdgeThickness));

                // 判断周围像素的Alpha值是否显著不同
                bool isEdge = 
                    (abs(col.a - left.a) > 0.1) || 
                    (abs(col.a - right.a) > 0.1) || 
                    (abs(col.a - top.a) > 0.1) || 
                    (abs(col.a - bottom.a) > 0.1);

                // 如果是边缘，则返回白色，否则返回原始颜色
                return isEdge ? _Color : col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}