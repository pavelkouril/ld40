// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Globe"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 10
		_TessMin( "Tess Min Distance", Float ) = 0
		_TessMax( "Tess Max Distance", Float ) = 61.17
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_DisplacementTex("DisplacementTex", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Albedo("Albedo", 2D) = "white" {}
		_Displacement("Displacement", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		uniform sampler2D _TextureSample2;
		uniform sampler2D _DisplacementTex;
		uniform float4 _DisplacementTex_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Displacement;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		float4 tessFunction( appdata v0, appdata v1, appdata v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float2 uv_DisplacementTex = v.texcoord * _DisplacementTex_ST.xy + _DisplacementTex_ST.zw;
			float4 tex2DNode1 = tex2Dlod( _DisplacementTex, float4( uv_DisplacementTex, 0, 1.0) );
			v.vertex.xyz += ( ( float4( ase_vertexNormal , 0.0 ) * tex2DNode1 ) * _Displacement ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float mulTime57 = _Time.y * -0.00025;
			float2 appendResult60 = (float2(mulTime57 , 0.0));
			float2 uv_DisplacementTex = i.uv_texcoord * _DisplacementTex_ST.xy + _DisplacementTex_ST.zw;
			float4 tex2DNode1 = tex2D( _DisplacementTex, uv_DisplacementTex );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float2 paralaxOffset70 = ParallaxOffset( tex2DNode1.r , -0.004 , mul( ase_worldToTangent, ase_worldlightDir ) );
			float2 clampResult105 = clamp( paralaxOffset70 , float2( -0.004,-0.004 ) , float2( 0.004,0.004 ) );
			float2 uv_TexCoord64 = i.uv_texcoord * float2( 1,1 ) + ( appendResult60 + clampResult105 );
			float4 smoothstepResult69 = smoothstep( float4( 0.5073529,0.5073529,0.5073529,0 ) , float4( 1,1,1,0 ) , ( 1.0 - tex2D( _TextureSample2, uv_TexCoord64 ) ));
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = ( smoothstepResult69 * tex2D( _Albedo, uv_Albedo ) ).rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float dotResult23 = dot( ase_worldlightDir , mul( unity_ObjectToWorld, float4( ase_vertexNormal , 0.0 ) ).xyz );
			float4 lerpResult12 = lerp( float4(0,0,0,0) , tex2D( _TextureSample0, uv_TextureSample0 ) , -( dotResult23 + -0.25 ));
			float4 ifLocalVar30 = 0;
			if( (lerpResult12).r > 0.0 )
				ifLocalVar30 = lerpResult12;
			o.Emission = ( ifLocalVar30 * 3.0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
788;179;1086;797;1631.816;1413.074;1;True;False
Node;AmplifyShaderEditor.WorldToTangentMatrix;101;-1278.964,-867.6294;Float;False;0;1;FLOAT3x3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;24;-1442.867,-387.3341;Float;False;1;0;FLOAT;0.0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-928.2839,496.5341;Float;True;Property;_DisplacementTex;DisplacementTex;6;0;Assets/Textures/world-height.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-1072.964,-799.6294;Float;False;2;2;0;FLOAT3x3;0.0;False;1;FLOAT3;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.NormalVertexDataNode;3;-1549.448,249.1489;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ObjectToWorldMatrixNode;46;-1605.583,-22.19911;Float;False;0;1;FLOAT4x4
Node;AmplifyShaderEditor.SimpleTimeNode;57;-1250.74,-1152.441;Float;False;1;0;FLOAT;-0.00025;False;1;FLOAT
Node;AmplifyShaderEditor.ParallaxOffsetHlpNode;70;-902.4105,-836.2711;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;-0.004;False;2;FLOAT3;0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-1395.287,70.91389;Float;False;2;2;0;FLOAT4x4;0.0;False;1;FLOAT3;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.DynamicAppendNode;60;-1011.408,-1152.625;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.DotProductOpNode;23;-1026.055,-366.1674;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;105;-690.2729,-846.3992;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;-0.004,-0.004;False;2;FLOAT2;0.004,0.004;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-476.7249,-873.3932;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0.01;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-792.1268,-311.4962;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;-0.25;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;64;-267.5554,-795.627;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.NegateNode;40;-647.5466,-367.3571;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;13;-515.6877,-606.6997;Float;False;Constant;_Color0;Color 0;5;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;7;-836.004,-619.6644;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Assets/Textures/earth_lights.gif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;100;33.13105,-831.9601;Float;True;Property;_TextureSample2;Texture Sample 2;7;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;12;-377.5749,-328.2701;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;66;392.3154,-796.0569;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ComponentMaskNode;31;-93.61678,-168.3056;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-498.7993,212.5002;Float;False;2;2;0;FLOAT3;0.0,0,0,0;False;1;COLOR;0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SmoothstepOpNode;69;625.6881,-797.684;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.5073529,0.5073529,0.5073529,0;False;2;COLOR;1,1,1,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;4;-1573.282,557.804;Float;False;Property;_Displacement;Displacement;9;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ConditionalIfNode;30;148.3131,-52.24221;Float;False;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;44;181.3164,117.5081;Float;False;Constant;_Float0;Float 0;5;0;3;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-117.9136,-551.9376;Float;True;Property;_Albedo;Albedo;8;0;Assets/Textures/world.topo.bathy.200408.3x5400x2700.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;106;-1114.816,-1313.074;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.00025,-0.00025;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;934.3406,-560.2334;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-314.2512,236.9467;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;606.4125,-48.81091;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1200.284,-358.9985;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Custom/Globe;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;True;0;10;0;61.17;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;102;0;101;0
WireConnection;102;1;24;0
WireConnection;70;0;1;0
WireConnection;70;2;102;0
WireConnection;47;0;46;0
WireConnection;47;1;3;0
WireConnection;60;0;57;0
WireConnection;23;0;24;0
WireConnection;23;1;47;0
WireConnection;105;0;70;0
WireConnection;65;0;60;0
WireConnection;65;1;105;0
WireConnection;54;0;23;0
WireConnection;64;1;65;0
WireConnection;40;0;54;0
WireConnection;100;1;64;0
WireConnection;12;0;13;0
WireConnection;12;1;7;0
WireConnection;12;2;40;0
WireConnection;66;0;100;0
WireConnection;31;0;12;0
WireConnection;5;0;3;0
WireConnection;5;1;1;0
WireConnection;69;0;66;0
WireConnection;30;0;31;0
WireConnection;30;2;12;0
WireConnection;67;0;69;0
WireConnection;67;1;2;0
WireConnection;6;0;5;0
WireConnection;6;1;4;0
WireConnection;43;0;30;0
WireConnection;43;1;44;0
WireConnection;0;0;67;0
WireConnection;0;2;43;0
WireConnection;0;11;6;0
ASEEND*/
//CHKSM=CB1172FFA6CD8A9C856C91B5AD7FE4842884747E