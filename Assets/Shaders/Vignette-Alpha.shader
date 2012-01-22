Shader "Sample/Vignette-Alpha" 
{
	Properties 
	{
		_Color( "Main Color", Color ) = ( 1, 1, 1, 1 )
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "Queue" = "Transparent" }
		LOD 200
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
	
			uniform float _Intensity;
			uniform float4 _Color;
			sampler2D _MainTex;
	
			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
	
			struct v2f 
			{
			    float4 pos : SV_POSITION;
			    float2 uv : TEXCOORD0;
			    fixed4 color : COLOR;
			};
			
			v2f vert(appdata v) 
			{
			    v2f o;
			    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			    o.uv = v.texcoord;
			    o.color = _Color;
			    return o;
			}
			
			half4 frag(v2f i) : COLOR 
			{
			    half4 c = i.color;
			    c.a = clamp(tex2D(_MainTex, i.uv).a * _Intensity, 0f, 0.8f);
			    return c;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
