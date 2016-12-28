Shader "Custom/Toon" {
	Properties {
		_Color ("Color" , COLOR) = (1.0,1.0,1.0,1.0)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Midtone ("Midtone", 2D) = "white" {}
		_Shadow ("Shadow",2D) = "white" {}
		_RimPower ("Rim Power",Range (0,9)) = 1
		_Factor ("Toon Direct Factor",Range (0,1)) = 0.5
		_Outline ("Toon Outline",Range (0, 0.1)) = 0.02
		_OutlineColor ("Outline Color", COLOR) = (0.2,0.2,0.2,1.0)
		_Steps ("Toon Steps",range (0,9)) = 3
		_Ramp ("Ramp",2D) = "white" {}
	}
	SubShader {
		LOD 200

		Pass {
			Tags {"LightMode" = "Always"}
			Cull Front
			Lighting Off
			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _Outline;
			float _Factor;
			float4 _OutlineColor;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert (a2v v) {
				v2f o;
				float3 dir = normalize (v.vertex.xyz);
				float3 dir2 = normalize(v.normal);
				float D = dot (dir, dir2);
				dir = dir*sign (D);
				dir = dir*_Factor + dir2*(1 - _Factor);
				float timefac = floor (3 * frac (_Time[1] / 2)) / 3 + 0.5;
				half4 viewpos = mul (UNITY_MATRIX_MV, v.vertex);
				float spacefac = 0.5 + abs (sin (10 * viewpos[0] + timefac) + sin (10 * viewpos[2] + timefac)) / 2;
				v.vertex.xyz += dir * _Outline * spacefac;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				return o;
			}

			float4 frag (v2f i) : COLOR{
				return _OutlineColor;
			}

			ENDCG
		}
		
		Pass {
			Cull Back
			Lighting On
			Tags{"LightMode"="ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include"UnityCG.cginc"
			uniform float4 _LightColor0;
			float4 _Color;
			float _Steps;

			sampler2D _MainTex;
			sampler2D _Midtone;
			sampler2D _Shadow;
			sampler2D _Ramp;

			float4 _MainTex_ST;
			float4 _Midtone_ST;
			float4 _Shadow_ST;

			struct v2f {
				float4 pos : SV_POSITION;
				float3 lightdir : TEXCOORD0;
				float3 viewdir : TEXCOORD1;
				float3 norm : TEXCOORD2;
				float4 uv0 : TEXCOORD3;
				float4 uv1 : TEXCOORD4;
				float4 uv2 : TEXCOORD5;
			};

			v2f vert (appdata_full v) {
				v2f o;

				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.norm = v.normal;
				o.lightdir = ObjSpaceLightDir (v.vertex);
				o.viewdir = ObjSpaceViewDir (v.vertex);
				o.uv0 = float4(v.texcoord.xy * _MainTex_ST.xy, _MainTex_ST.zw);
				o.uv1 = float4(v.texcoord.xy * _Midtone_ST.xy, _Midtone_ST.zw);
				o.uv2 = float4(v.texcoord.xy * _Shadow_ST.xy, _Shadow_ST.zw);
				return o;
			}

			float4 frag (v2f i) : COLOR {
				float4 c = tex2D (_MainTex, i.uv0);
				c = c * _Color;
				float4 m = tex2D (_Midtone, i.uv1);
				float4 s = tex2D (_Shadow, i.uv2);
				float3 n = normalize(i.norm);

				float3 viewdir = normalize (i.viewdir);
				float3 lightdir = normalize (i.lightdir);

				float diff = max (0, dot(n, lightdir));
				diff = (1 + diff) / 2;
				diff = smoothstep (0, 1, diff);
				float toon = floor (diff*_Steps) / _Steps;
				diff = tex2D (_Ramp, float2(toon, 0.5));
				if(diff > 0.49) {
					c.rgb = _LightColor0 * c.rgb * diff;
				}
				else if(diff > 0.2) {
					c.rgb = _LightColor0 * c.rgb * diff * m.rgb;
				}
				else {
					c.rgb = _LightColor0 * c.rgb * diff * s.rgb;
				}
				
				return c;
			}

			ENDCG
		}

		Pass {
			Tags{"LightMode"="ForwardAdd"}
			Cull Back
			Blend One One
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include"UnityCG.cginc"
			uniform float4 _LightColor0;
			float4 _Color;
			float _Steps;
			float _RimPower;

			sampler2D _MainTex;
			sampler2D _Midtone;
			sampler2D _Shadow;
			sampler2D _Ramp;

			float4 _MainTex_ST;
			float4 _Midtone_ST;
			float4 _Shadow_ST;

			struct v2f {
				float4 pos : SV_POSITION;
				float3 lightdir : TEXCOORD0;
				float3 viewdir : TEXCOORD1;
				float3 norm : TEXCOORD2;
				float4 uv0 : TEXCOORD3;
				float4 uv1 : TEXCOORD4;
				float4 uv2 : TEXCOORD5;
			};

			v2f vert (appdata_full v) {
				v2f o;

				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.norm = v.normal;
				o.lightdir = ObjSpaceLightDir (v.vertex);
				o.viewdir = ObjSpaceViewDir (v.vertex);
				o.uv0 = float4(v.texcoord.xy * _MainTex_ST.xy, _MainTex_ST.zw);
				o.uv1 = float4(v.texcoord.xy * _Midtone_ST.xy, _Midtone_ST.zw);
				o.uv2 = float4(v.texcoord.xy * _Shadow_ST.xy, _Shadow_ST.zw);
				return o;
			}

			float4 frag (v2f i) : COLOR {
				float4 c = tex2D (_MainTex, i.uv0);
				c = c * _Color;
				float4 m = tex2D (_Midtone, i.uv1);
				float4 s = tex2D (_Shadow, i.uv2);
				float3 n = normalize(i.norm);
				float3 viewdir = normalize (i.viewdir);
				float rim = 1.0 - saturate (dot (n, normalize (viewdir)));
				rim = rim + 1;
				rim = pow (rim, _RimPower);

				float lengthsq = length (i.lightdir);
				float atten = 1 / lengthsq;
				float3 lightdir = normalize (i.lightdir);

				float diff = max (0, dot(n, lightdir));
				diff = (1 + diff) / 2;
				diff = smoothstep (0, 1, diff);
				float toon = floor (diff * atten * _Steps) / _Steps;
				diff = tex2D (_Ramp, float2(toon, 0.5));

				half3 h = normalize (lightdir + viewdir);
				float nh = max (0, dot (n, h));
				float spec = pow (nh, 32.0);
				float toonspec = floor (spec*atten * 2) / 2;
				spec = tex2D (_Ramp, float2(toonspec, 0.5));

				float toonrim = floor (rim * _Steps) / _Steps;

				if(diff > 0.49) {
					c.rgb = _LightColor0 * c.rgb * (diff + spec) * rim;
				}
				else {
					c.rgb = _LightColor0 * c.rgb * (diff + spec) * rim * m.rgb;
				}
				return c;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
