// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASETemplateShaders/DefaultUnlit"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Float0("Float 0", Float) = 0
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }
		LOD 100
		Cull Off


		Pass
		{
			CGPROGRAM
			#pragma target 3.0 
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
			};

			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform float _Float0;
			
			v2f vert ( appdata v )
			{
				v2f o;
				o.texcoord.xy = v.texcoord.xy;
				o.texcoord.zw = v.texcoord1.xy;
				
				// ase common template code
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				fixed4 myColorVar;
				// ase common template code
				float4 lerpResult3 = lerp( ( float4(0,1,0,1) * ( 1.0 - _Float0 ) ) , ( float4(1,0,0,0) * _Float0 ) , _Float0);
				
				
				myColorVar = lerpResult3;
				return myColorVar;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13803
119;204;1417;734;788.3044;362.4957;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;4;-469.3044,113.5043;Float;True;Property;_Float0;Float 0;0;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;2;-556.3044,-99.4957;Float;False;Constant;_Color1;Color 1;0;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;6;-181.3044,-219.4957;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;1;-502.6661,-300.2418;Float;False;Constant;_Color0;Color 0;0;0;0,1,0,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-235.3044,-57.4957;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;32.69562,-291.4957;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;3;38.6956,-109.4957;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.TemplateMasterNode;0;286,21;Float;False;True;2;Float;ASEMaterialInspector;0;2;ASETemplateShaders/DefaultUnlit;6e114a916ca3e4b4bb51972669d463bf;ASETemplateShaders/DefaultUnlit;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;6;0;4;0
WireConnection;7;0;2;0
WireConnection;7;1;4;0
WireConnection;5;0;1;0
WireConnection;5;1;6;0
WireConnection;3;0;5;0
WireConnection;3;1;7;0
WireConnection;3;2;4;0
WireConnection;0;0;3;0
ASEEND*/
//CHKSM=9A0229F50A50D5644C76F57B3BC38536771BA84C