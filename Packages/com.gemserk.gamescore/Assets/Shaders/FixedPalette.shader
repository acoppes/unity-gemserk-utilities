// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FixedPalette"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PaletteTex("Palette", 2D) = "white" {}
		// _Color ("Tint", Color) = (1,1,1,1)
		_Invert("Invert", Float) = 0
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
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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
				// float4 color : COLOR; 
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				// float4 color : COLOR; 
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				// o.color = v.color;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _PaletteTex;
			float _Invert;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				// float x = (col.r * i.color.r + col.g * i.color.g + col.b * i.color.b) / 3.0f;
				float x = (col.r + col.g + col.b) / 3.0f;

				float4 newColNormal = tex2D(_PaletteTex, float2(x, 0));
				float4 newColInverted = tex2D(_PaletteTex, 1.0 - float2(x, 0));

				float4 newCol = newColNormal * (1.0 - _Invert) + newColInverted * _Invert;
				
				// newCol.a = col.a * i.color.a;
				newCol.a = col.a;
				return newCol;
			}

			ENDCG
		}
	}
}
