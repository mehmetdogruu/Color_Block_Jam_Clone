Shader "Custom/ClippingShaderRectangle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Obje için bir texture
        _Color ("Color Tint", Color) = (1, 1, 1, 1)  // Renk özelliði
        _ClipPosition ("Clip Position", Vector) = (0, 0, 0, 1)  // Dikdörtgen merkezi
        _ClipWidth ("Clip Width", Float) = 1.0  // Dikdörtgen geniþliði
        _ClipHeight ("Clip Height", Float) = 1.0  // Dikdörtgen yüksekliði
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _ClipPosition;
            float _ClipWidth;
            float _ClipHeight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Objenin dünya pozisyonunu clipping merkezi ile karþýlaþtýr
                float3 localPos = i.worldPos - _ClipPosition.xyz;

                // Eðer objenin pozisyonu dikdörtgenin geniþlik ve yüksekliði sýnýrlarý içindeyse gizle
                if (abs(localPos.x) <= _ClipWidth / 2.0 && abs(localPos.z) <= _ClipHeight / 2.0)
                {
                    discard;  // Dikdörtgen alanýndaysa gizle
                }

                // Texture ve renk karýþýmý
                half4 texColor = tex2D(_MainTex, i.uv);
                return texColor * _Color;
            }
            ENDCG
        }
    }
}
