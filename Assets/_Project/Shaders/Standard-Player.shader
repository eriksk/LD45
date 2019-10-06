Shader "Custom/Player"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _AlphaCut("Alpha Cut", Range(0, 1)) = 0.5
        _DissolveAmount("Dissolve Amount", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alphatest:_AlphaCut addshadow
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
           UNITY_DEFINE_INSTANCED_PROP(float, _DissolveAmount)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float dissolve = tex2D(_MainTex, IN.uv_MainTex*0.5).r;

            fixed4 c = _Color;

            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = lerp(1, dissolve, UNITY_ACCESS_INSTANCED_PROP(Props, _DissolveAmount));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
