// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Clouds"
{
	Properties
	{
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_flowmap("flowmap", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 5.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample1;
		uniform sampler2D _flowmap;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime103 = _Time.y * 0.09;
			float2 uv_TexCoord94 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 uv_TexCoord91 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 lerpResult105 = lerp( ( frac( mulTime103 ) * (tex2D( _flowmap, uv_TexCoord94 )).rg ) , uv_TexCoord91 , 0.77);
			float4 tex2DNode82 = tex2D( _TextureSample1, lerpResult105 );
			o.Albedo = tex2DNode82.rgb;
			o.Alpha = tex2DNode82.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
726;185;1086;797;843.7408;2017.382;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-1169.571,-1846.627;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;86;-914.2831,-1875.871;Float;True;Property;_flowmap;flowmap;1;0;Assets/Textures/flowmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;103;-639.6735,-1973.91;Float;False;1;0;FLOAT;0.09;False;1;FLOAT
Node;AmplifyShaderEditor.ComponentMaskNode;95;-597.3534,-1870.096;Float;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.FractNode;101;-425.1721,-1983.38;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-224.9332,-1868.678;Float;True;2;2;0;FLOAT;0,0;False;1;FLOAT2;0;False;1;FLOAT2
Node;AmplifyShaderEditor.TextureCoordinatesNode;91;-802.4807,-1535.761;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;105;-44.51579,-1525.179;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0.77;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;82;268.3442,-1508.249;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;589.5864,-1554.323;Float;False;True;7;Float;ASEMaterialInspector;0;0;Standard;Custom/Clouds;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;1;1;False;0;0;Transparent;1;True;False;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;10;0;61.17;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;86;1;94;0
WireConnection;95;0;86;0
WireConnection;101;0;103;0
WireConnection;97;0;101;0
WireConnection;97;1;95;0
WireConnection;105;0;97;0
WireConnection;105;1;91;0
WireConnection;82;1;105;0
WireConnection;0;0;82;0
WireConnection;0;9;82;4
ASEEND*/
//CHKSM=DE7E445D456BD5FA93CBC328F3608A9A689ED5FF