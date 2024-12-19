Shader "Custom/Curved World Effect"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature ("Curvature", Float) = 0.05 // Controls the strength of the curve
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

        sampler2D _MainTex;
        float _Curvature;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v)
        {
            // Apply curvature by modifying vertex positions
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

            // Curving along the Z-axis (into the screen)
            worldPos.y += _Curvature * (worldPos.z * worldPos.z);

            // Update vertex position
            v.vertex = mul(unity_WorldToObject, float4(worldPos, 1.0));
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
