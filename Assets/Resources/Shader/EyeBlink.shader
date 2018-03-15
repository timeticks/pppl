Shader "Custom/EyeBlink"   
{  
   Properties   
    {  
        _MainTex ("Base (RGB)", 2D) = "white" {}  
    }  
  
    CGINCLUDE  
    #include "UnityCG.cginc"  
    uniform sampler2D _MainTex;  
    uniform float _pointY;  
    uniform float _lineWidth;  
	uniform float _waveScale;  
  
    fixed4 frag(v2f_img i) : SV_Target  
    {  
		fixed4 back = fixed4(0,0,0,0);

		float2 centerUv = float2(i.uv.x*2-1 , i.uv.y*2-1);

		fixed4 screenCol = tex2D(_MainTex, i.uv);
		if(centerUv.y>0)
		{
			float dis = length(float2(0,-_waveScale)- centerUv) -_pointY; 
			fixed4 col = lerp(screenCol , back , clamp(dis*_lineWidth,0.1,1));
			return col;
		}
		else
		{
			float dis = length(float2(0,_waveScale)- centerUv) - _pointY; 
			fixed4 col = lerp(screenCol , back ,clamp(dis*_lineWidth,0.1,1));
			return col;
		}
		return back;
		
    }
  
    ENDCG  
  
    SubShader   
    {  
        Pass  
        {  
            ZTest Always  
            Cull Off  
            ZWrite Off  
            Fog { Mode off }  
  
            CGPROGRAM  
            #pragma vertex vert_img  
            #pragma fragment frag  
            #pragma fragmentoption ARB_precision_hint_fastest   
            ENDCG  
        }  
    }  
    Fallback off  
}  