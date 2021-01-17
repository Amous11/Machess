// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/DiffuseBrightnessBlink" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0,1)) = 0.5
		_ColorBlink("ColorBlink", Color) = (1,1,1,1)
		_Blink("Blink", Range(0,1)) = 0.5
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }	
		
		
		CGPROGRAM
		#pragma surface surf Brightness   

		float _Brightness;			
		
		#include "UnityPBSLighting.cginc"
		
		

		
		inline  fixed4 BrightnessLight (SurfaceOutput s, UnityLight light)
		{			
			float diff = dot (s.Normal, light.dir);
			
			float procent  = (diff + 1.0) * 0.5;
			diff = 1 - (2.0 * (1.0 - _Brightness))  + procent * 2.0 * (1.0 - _Brightness);
			//diff = 1.0 - (2.0 * (1.0 - _Brightness));
			//diff = 1.0 - (2.0 * (0.5 ));
			//diff = _Brightness;

			fixed4 c;
			//c.rgb = s.Albedo * light.color * diff;
			c.rgb = s.Albedo * light.color * diff;
			c.a = s.Alpha;
			return c;
		}
		half4 LightingBrightness (SurfaceOutput s, UnityGI gi)
		{
			fixed4 c;
			c = BrightnessLight (s, gi.light);
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				c.rgb += s.Albedo * gi.indirect.diffuse;
			#endif
			return c;
        }
		
		inline void LightingBrightness_GI (
			SurfaceOutput s,
			UnityGIInput data,
			inout UnityGI gi)
		{
			gi = UnityGlobalIllumination (data, 1.0, s.Normal);
		}
		
		

		float4 _Color;	
		float4 _ColorBlink;	
		float _Blink;		
		struct Input 
		{
			float2 uv_MainTex;
		};
		
		sampler2D _MainTex;


		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb  * _Color.rgb;
			o.Albedo = lerp(o.Albedo, _ColorBlink, _Blink);
			//o.Alpha = c.a * _Color.a;
		}
		ENDCG
	}

	Fallback "Mobile/VertexLit"
}
