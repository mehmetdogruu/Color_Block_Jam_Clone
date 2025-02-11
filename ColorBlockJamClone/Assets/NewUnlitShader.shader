Shader "Custom/ClippingShaderRectangle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Obje i�in bir texture
        _Color ("Color Tint", Color) = (1, 1, 1, 1)  // Renk �zelli�i
        _ClipPosition ("Clip Position", Vector) = (0, 0, 0, 1)  // Dikd�rtgen merkezi
        _ClipWidth ("Clip Width", Float) = 1.0  // Dikd�rtgen geni�li�i
        _ClipHeight ("Clip Height", Float) = 1.0  // Dikd�rtgen y�ksekli�i
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
                // Objenin d�nya pozisyonunu clipping merkezi ile kar��la�t�r
                float3 localPos = i.worldPos - _ClipPosition.xyz;

                // E�er objenin pozisyonu dikd�rtgenin geni�lik ve y�ksekli�i s�n�rlar� i�indeyse gizle
                if (abs(localPos.x) <= _ClipWidth / 2.0 && abs(localPos.z) <= _ClipHeight / 2.0)
                {
                    discard;  // Dikd�rtgen alan�ndaysa gizle
                }

                // Texture ve renk kar���m�
                half4 texColor = tex2D(_MainTex, i.uv);
                return texColor * _Color;
            }
            ENDCG
        }
    }
}
