// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Clouds"
{
	Properties
	{
		_Texture1("Texture 1", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "white" {}
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

		uniform sampler2D _Texture0;
		uniform sampler2D _Texture1;
		uniform float4 _Texture1_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture1 = i.uv_texcoord * _Texture1_ST.xy + _Texture1_ST.zw;
			float2 panner142 = ( tex2D( _Texture1, uv_Texture1 ).rg + 1.0 * _Time.y * float2( 0.03,0.02 ));
			float2 panner149 = ( tex2D( _Texture1, uv_Texture1 ).rg + 0.5 * _Time.y * float2( -0.01,-0.02 ));
			float2 panner160 = ( tex2D( _Texture1, uv_Texture1 ).rg + 1.0 * _Time.y * float2( -0.04,-0.01 ));
			float2 panner161 = ( tex2D( _Texture1, uv_Texture1 ).rg + 1.0 * _Time.y * float2( 0.02,0.04 ));
			float2 panner192 = ( tex2D( _Texture1, uv_Texture1 ).rg + 0.5 * _Time.y * float2( 0.01,0.08 ));
			float2 panner193 = ( tex2D( _Texture1, uv_Texture1 ).rg + 0.5 * _Time.y * float2( -0.01,0.07 ));
			float clampResult172 = clamp( ( pow( ( pow( ( tex2D( _Texture0, panner142 ).r * tex2D( _Texture0, panner149 ).g ) , 0.68 ) * pow( ( tex2D( _Texture0, panner160 ).r * tex2D( _Texture0, panner161 ).b ) , 0.84 ) * pow( ( tex2D( _Texture0, panner192 ).g * tex2D( _Texture0, panner193 ).b ) , 1.27 ) ) , 0.6 ) * 30.08 ) , 0.0 , 1.0 );
			float3 temp_cast_6 = (clampResult172).xxx;
			o.Albedo = temp_cast_6;
			o.Alpha = clampResult172;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14003
577;236;1086;797;2576.992;1963.206;1.826922;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;202;-3354.08,-1325.964;Float;True;Property;_Texture1;Texture 1;0;0;None;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;158;-2706.562,-1525.016;Float;True;Property;_TextureSample8;Texture Sample 8;6;0;Assets/Textures/flowmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;189;-2686.939,-410.2261;Float;True;Property;_TextureSample3;Texture Sample 3;3;0;Assets/Textures/flowmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;188;-2682.367,-845.2336;Float;True;Property;_TextureSample2;Texture Sample 2;5;0;Assets/Textures/flowmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;141;-2714.095,-2268.609;Float;True;Property;_flowmap;flowmap;4;0;Assets/Textures/flowmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;148;-2718.667,-1833.602;Float;True;Property;_TextureSample6;Texture Sample 6;2;0;Assets/Textures/flowmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;159;-2713.953,-1239.409;Float;True;Property;_TextureSample9;Texture Sample 9;0;0;Assets/Textures/flowmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;149;-2366.667,-1824.602;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.01,-0.02;False;1;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;142;-2362.095,-2259.609;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.03,0.02;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;160;-2354.562,-1516.016;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.04,-0.01;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;192;-2330.367,-836.2335;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.01,0.08;False;1;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;161;-2359.414,-1230.409;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0.04;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;193;-2346.907,-480.2181;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.01,0.07;False;1;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;201;-1975.681,-1397.793;Float;True;Property;_Texture0;Texture 0;1;0;None;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;194;-1353.896,-515.1913;Float;True;Property;_TextureSample4;Texture Sample 4;0;0;Assets/Textures/27d378304073986bfe0ab39285685650ac51417f.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;150;-1385.624,-1824.363;Float;True;Property;_TextureSample7;Texture Sample 7;1;0;Assets/Textures/27d378304073986bfe0ab39285685650ac51417f.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;195;-1328.525,-885.5927;Float;True;Property;_TextureSample12;Texture Sample 12;3;0;Assets/Textures/27d378304073986bfe0ab39285685650ac51417f.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;162;-1380.91,-1230.17;Float;True;Property;_TextureSample10;Texture Sample 10;2;0;Assets/Textures/27d378304073986bfe0ab39285685650ac51417f.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;163;-1353.72,-1565.376;Float;True;Property;_TextureSample11;Texture Sample 11;5;0;Assets/Textures/27d378304073986bfe0ab39285685650ac51417f.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;145;-1360.253,-2308.969;Float;True;Property;_TextureSample5;Texture Sample 5;4;0;Assets/Textures/27d378304073986bfe0ab39285685650ac51417f.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;-851.4549,-1322.483;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;-887.1078,-1834.066;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;-799.8818,-788.821;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;167;-613.5311,-1780.243;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.68;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;200;-549.3959,-946.4506;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;1.27;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;168;-593.4278,-1337.997;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.84;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;-280.3138,-1484.195;Float;True;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;170;-36.05905,-1478.562;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;307.4305,-1456.524;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;30.08;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;172;601.8433,-1454.734;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1038.68,-1560.326;Float;False;True;7;Float;ASEMaterialInspector;0;0;Standard;Custom/Clouds;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;1;1;False;0;0;Transparent;1;True;False;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;10;0;61.17;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;158;0;202;0
WireConnection;189;0;202;0
WireConnection;188;0;202;0
WireConnection;141;0;202;0
WireConnection;148;0;202;0
WireConnection;159;0;202;0
WireConnection;149;0;148;0
WireConnection;142;0;141;0
WireConnection;160;0;158;0
WireConnection;192;0;188;0
WireConnection;161;0;159;0
WireConnection;193;0;189;0
WireConnection;194;0;201;0
WireConnection;194;1;193;0
WireConnection;150;0;201;0
WireConnection;150;1;149;0
WireConnection;195;0;201;0
WireConnection;195;1;192;0
WireConnection;162;0;201;0
WireConnection;162;1;161;0
WireConnection;163;0;201;0
WireConnection;163;1;160;0
WireConnection;145;0;201;0
WireConnection;145;1;142;0
WireConnection;164;0;163;1
WireConnection;164;1;162;3
WireConnection;151;0;145;1
WireConnection;151;1;150;2
WireConnection;198;0;195;2
WireConnection;198;1;194;3
WireConnection;167;0;151;0
WireConnection;200;0;198;0
WireConnection;168;0;164;0
WireConnection;173;0;167;0
WireConnection;173;1;168;0
WireConnection;173;2;200;0
WireConnection;170;0;173;0
WireConnection;171;0;170;0
WireConnection;172;0;171;0
WireConnection;0;0;172;0
WireConnection;0;9;172;0
ASEEND*/
//CHKSM=6AE51E20F71ACACE0643D3FF77188D21A177F269