Shader "Custom/UIOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineSize ("Outline Size", Float) = 0
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

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            fixed4 _OutlineColor;
            float _OutlineSize;

            float4 _MainTex_TexelSize;

            v2f vert(appdata_t IN)
            {
                v2f OUT;

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, IN.texcoord) * IN.color;

                if (_OutlineSize > 0)
                {
                    float2 offset = _MainTex_TexelSize.xy * _OutlineSize;

                    float a1 = tex2D(_MainTex, IN.texcoord + float2(offset.x,0)).a;
                    float a2 = tex2D(_MainTex, IN.texcoord - float2(offset.x,0)).a;
                    float a3 = tex2D(_MainTex, IN.texcoord + float2(0,offset.y)).a;
                    float a4 = tex2D(_MainTex, IN.texcoord - float2(0,offset.y)).a;

                    float outline = max(max(a1,a2), max(a3,a4));

                    col.rgb = lerp(_OutlineColor.rgb, col.rgb, col.a);
                    col.a = max(col.a, outline * _OutlineColor.a);
                }

                return col;
            }
            ENDCG
        }
    }
}