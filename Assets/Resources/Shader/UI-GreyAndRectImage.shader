Shader "UI/GreyAndRectImage"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		//_AlphaTex ("_AlphaTex", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15
		_RadiusScale ("RadiusScale", Range(1,2)) = 1.173
		_RadiusRatio ("RadiusRatio", Range(1,4)) = 3.25
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				float2 RadiusBuceVU : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			half _RadiusScale;
			half _RadiusRatio;
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				OUT.RadiusBuceVU=IN.texcoord-float2(0.5,0.5);

				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;
			//sampler2D _AlphaTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				//fixed4 color2 = tex2D(_AlphaTex, IN.texcoord);
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				//color.a = color2.r*color.a;

				fixed rx = fmod(IN.RadiusBuceVU.x, 0.4*_RadiusScale);
				fixed ry = fmod(IN.RadiusBuceVU.y, 0.4);
				fixed mx = step(0.4*_RadiusScale, abs(IN.RadiusBuceVU.x));
				fixed my = step(0.4, abs(IN.RadiusBuceVU.y));
				if(mx*my*step(0.1, length(fixed2(rx*_RadiusRatio,ry)))==1)
				{
					discard;
				}

				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif
				fixed grey = dot(color.rgb, fixed3(0.299, 0.587, 0.114));
				return fixed4(grey,grey,grey,color.w);
			}
		ENDCG
		}
	}
}
