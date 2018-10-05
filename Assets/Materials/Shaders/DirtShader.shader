Shader "Custom/DirtShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_DirtColor("Dirt Color", Color) = (1, 1, 1, 1)
		_PaintTex ("Paint Texture (RGBA)", 2D) = "white" {}
        _BlendAmount("Blend Amount", Range(0.0, 1.0)) = 1.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf StandardSpecular fullforwardshadows
 
        sampler2D _MainTex;
        sampler2D _PaintTex;
        

		struct Input {
			float2 uv_PaintTex;

		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _DirtColor;
		
		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		//UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here 
		half _BlendAmount;
		//UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandardSpecular o) {
			fixed4 main = _Color; //Generates the primary paint color for the car.
			fixed4 paint = tex2D(_PaintTex, IN.uv_PaintTex); //Samples the correct paint texture for this UV on the car.

			//This line overlays the base texture with the dirt texture, with respect to Parameter _BlendAmount
			o.Specular = main.rgb * (1 - paint.a * _BlendAmount) + paint.a * _BlendAmount * _DirtColor;
			//Adds metallic to non-dirty areas to simulate the shiny paint showing through.
			o.Smoothness = _Metallic * (1 - paint.a * _BlendAmount);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
