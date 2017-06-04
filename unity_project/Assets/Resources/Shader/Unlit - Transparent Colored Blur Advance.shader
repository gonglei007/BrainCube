Shader "Unlit/Transparent Colored Blur Advance"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_Distance ("Distance", Float) = 0.003
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
			#pragma target 3.0		
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
      		float _Distance;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
				float distance = _Distance;
		        
			    fixed4 col = tex2D(_MainTex, IN.texcoord) * 0.16;
			    
			    col += tex2D(_MainTex, half2(IN.texcoord.x-5.0 * distance, IN.texcoord.y+5.0 * distance)) * 0.025;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+5.0 * distance, IN.texcoord.y-5.0 * distance)) * 0.025;
			    
			    col += tex2D(_MainTex, half2(IN.texcoord.x-4.0 * distance, IN.texcoord.y+4.0 * distance)) * 0.05;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+4.0 * distance, IN.texcoord.y-4.0 * distance)) * 0.05;

			    
			    col += tex2D(_MainTex, half2(IN.texcoord.x-3.0 * distance, IN.texcoord.y+3.0 * distance)) * 0.09;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+3.0 * distance, IN.texcoord.y-3.0 * distance)) * 0.09;
			    
			    col += tex2D(_MainTex, half2(IN.texcoord.x-2.0 * distance, IN.texcoord.y+2.0 * distance)) * 0.12;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+2.0 * distance, IN.texcoord.y-2.0 * distance)) * 0.12;
			    
			    col += tex2D(_MainTex, half2(IN.texcoord.x-1.0 * distance, IN.texcoord.y+1.0 * distance)) *  0.15;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+1.0 * distance, IN.texcoord.y-1.0 * distance)) *  0.15;
			    //
			    col += tex2D(_MainTex, half2(IN.texcoord.x-5.0 * distance, IN.texcoord.y-5.0 * distance)) * 0.025;
			    col += tex2D(_MainTex, half2(IN.texcoord.x-4.0 * distance, IN.texcoord.y-4.0 * distance)) * 0.05;
			    col += tex2D(_MainTex, half2(IN.texcoord.x-3.0 * distance, IN.texcoord.y-3.0 * distance)) * 0.09;
			    col += tex2D(_MainTex, half2(IN.texcoord.x-2.0 * distance, IN.texcoord.y-2.0 * distance)) * 0.12;
			    col += tex2D(_MainTex, half2(IN.texcoord.x-1.0 * distance, IN.texcoord.y-1.0 * distance)) * 0.15;
			    //
			    col += tex2D(_MainTex, half2(IN.texcoord.x+5.0 * distance, IN.texcoord.y+5.0 * distance)) * 0.025;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+4.0 * distance, IN.texcoord.y+4.0 * distance)) * 0.05;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+3.0 * distance, IN.texcoord.y+3.0 * distance)) * 0.09;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+2.0 * distance, IN.texcoord.y+2.0 * distance)) * 0.12;
			    col += tex2D(_MainTex, half2(IN.texcoord.x+1.0 * distance, IN.texcoord.y+1.0 * distance)) * 0.15;
			    col /= 2;
			    
		        return col;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
	Fallback "Unlit/Transparent Colored Blur"
}
