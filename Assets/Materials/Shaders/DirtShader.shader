Shader "Custom/DirtShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_PaintTex ("Paint Texture (RGBA)", 2D) = "white" {}
        _BlendAmount("Blend Amount", Range(0.0, 1.0)) = 1.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
         #pragma surface surf Lambert
 
        sampler2D _MainTex;
        sampler2D _PaintTex;
        fixed _BlendAmount;

		struct Input {
			float2 uv_PaintTex;

		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 main = _Color;
            fixed4 paint = tex2D (_PaintTex, IN.uv_PaintTex);
            o.Albedo = lerp(main.rgb, paint, main.a * _BlendAmount);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
