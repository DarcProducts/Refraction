Shader "Custom/Masker"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "black" {}
        _Color("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _YFill("Y Fill", Range(0,1)) = 1.0
        _XFill("X Fill", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MaskTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MaskTex;
        };

        fixed4 _Color;

        half _Glossiness;
        half _Metallic;
        half _MaskValue;
        half _YFill;
        half _XFill;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 m = tex2D (_MaskTex, IN.uv_MaskTex);

            float2 u = IN.uv_MainTex;

            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            if (c.x < 0.1f)
                discard;
            if (u.y > _YFill)
                discard;
            if (u.x > _XFill)
                discard;
            if (m.x < 0.05)
                discard;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
