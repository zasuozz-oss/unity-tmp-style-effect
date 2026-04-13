Shader "TextMeshPro/TMP_StyleEffect"
{

Properties
{
	[HideInInspector] _FaceTex			("Face Texture", 2D) = "white" {}
	[HideInInspector] _FaceUVSpeedX		("Face UV Speed X", Range(-5, 5)) = 0.0
	[HideInInspector] _FaceUVSpeedY		("Face UV Speed Y", Range(-5, 5)) = 0.0
	[HideInInspector] _FaceColor		("Face Color", Color) = (1,1,1,1)
	[HideInInspector] _FaceDilate		("Face Dilate", Range(-1,1)) = 0

	[HideInInspector] _OutlineColor		("Outline Color", Color) = (1,1,1,1)
	[HideInInspector] _OutlineTex		("Outline Texture", 2D) = "white" {}
	[HideInInspector] _OutlineUVSpeedX	("Outline UV Speed X", Range(-5, 5)) = 0.0
	[HideInInspector] _OutlineUVSpeedY	("Outline UV Speed Y", Range(-5, 5)) = 0.0
	[HideInInspector] _OutlineWidth		("Outline Total Width", Range(0, 1)) = 0
	[HideInInspector] _OutlineSoftness	("Outline Softness", Range(0,1)) = 0

	[HideInInspector] _Outline2Color	("Outline 2 Color", Color) = (0,0,0,1)
	[HideInInspector] _Outline2Width	("Inner Inward", Range(0, 0.5)) = 0
	[HideInInspector] _InnerOutward		("Inner Outward", Range(0, 0.5)) = 0
	[HideInInspector] _InnerSoftness	("Inner Softness", Range(0, 1)) = 0
	[HideInInspector] _OuterRenderWidth	("Outer Render Width", float) = 0
	[HideInInspector] _OuterSoftness	("Outer Softness", Range(0, 1)) = 0

	[HideInInspector] _WeightNormal		("Weight Normal", float) = 0
	[HideInInspector] _WeightBold		("Weight Bold", float) = 0.5

	[HideInInspector] _ShaderFlags		("Flags", float) = 0
	[HideInInspector] _ScaleRatioA		("Scale RatioA", float) = 1
	[HideInInspector] _ScaleRatioB		("Scale RatioB", float) = 1
	[HideInInspector] _ScaleRatioC		("Scale RatioC", float) = 1

	[HideInInspector] _MainTex			("Font Atlas", 2D) = "white" {}
	[HideInInspector] _TextureWidth		("Texture Width", float) = 512
	[HideInInspector] _TextureHeight	("Texture Height", float) = 512
	[HideInInspector] _GradientScale	("Gradient Scale", float) = 5.0
	[HideInInspector] _ScaleX			("Scale X", float) = 1.0
	[HideInInspector] _ScaleY			("Scale Y", float) = 1.0
	[HideInInspector] _PerspectiveFilter("Perspective Correction", Range(0, 1)) = 0.875
	[HideInInspector] _Sharpness		("Sharpness", Range(-1,1)) = 0

	[HideInInspector] _VertexOffsetX	("Vertex OffsetX", float) = 0
	[HideInInspector] _VertexOffsetY	("Vertex OffsetY", float) = 0

	[HideInInspector] _MaskCoord		("Mask Coordinates", vector) = (0, 0, 32767, 32767)
	[HideInInspector] _ClipRect			("Clip Rect", vector) = (-32767, -32767, 32767, 32767)
	[HideInInspector] _MaskSoftnessX	("Mask SoftnessX", float) = 0
	[HideInInspector] _MaskSoftnessY	("Mask SoftnessY", float) = 0

	[HideInInspector] _StencilComp		("Stencil Comparison", Float) = 8
	[HideInInspector] _Stencil			("Stencil ID", Float) = 0
	[HideInInspector] _StencilOp		("Stencil Operation", Float) = 0
	[HideInInspector] _StencilWriteMask	("Stencil Write Mask", Float) = 255
	[HideInInspector] _StencilReadMask	("Stencil Read Mask", Float) = 255

	[HideInInspector] _CullMode			("Cull Mode", Float) = 0
	[HideInInspector] _ColorMask		("Color Mask", Float) = 15

	// Drop Shadow (rendering)
	[HideInInspector] _ShadowColor		("Shadow Color", Color) = (0,0,0,0.5)
	[HideInInspector] _ShadowOffsetX	("Shadow Offset X", float) = 1
	[HideInInspector] _ShadowOffsetY	("Shadow Offset Y", float) = -1
	[HideInInspector] _ShadowDilate		("Shadow Dilate", float) = 0
	[HideInInspector] _ShadowSoftness	("Shadow Softness", float) = 0.5
	[HideInInspector] _AtlasPadding		("Atlas Padding", float) = 5

	// TMP Underlay (for GetPaddingForMaterial auto-padding)
	[HideInInspector] _UnderlayOffsetX	("Underlay OffsetX", Range(-1,1)) = 0
	[HideInInspector] _UnderlayOffsetY	("Underlay OffsetY", Range(-1,1)) = 0
	[HideInInspector] _UnderlayDilate	("Underlay Dilate", Range(-1,1)) = 0
	[HideInInspector] _UnderlaySoftness	("Underlay Softness", Range(0,1)) = 0

	// Glow
	[HideInInspector] _GlowColor		("Glow Color", Color) = (1,1,0,0.5)
	[HideInInspector] _GlowOffset		("Glow Offset", Range(-1,1)) = 0
	[HideInInspector] _GlowInner		("Glow Inner", Range(0,1)) = 0.05
	[HideInInspector] _GlowOuter		("Glow Outer", Range(0,1)) = 0.45
	[HideInInspector] _GlowPower		("Glow Power", Range(0.1,8)) = 1

	// Lighting
	[HideInInspector] _LightAngle		("Light Angle", Range(0,6.2832)) = 3.1416
	[HideInInspector] _SpecularColor	("Specular Color", Color) = (1,1,1,1)
	[HideInInspector] _SpecularPower	("Specular Power", Range(0,1)) = 0.5
	[HideInInspector] _Reflectivity		("Reflectivity", Range(0,5)) = 1
	[HideInInspector] _Diffuse			("Diffuse Intensity", Range(0,1)) = 0.5
	[HideInInspector] _Ambient			("Ambient", Range(0,1)) = 0.5
}

SubShader
{
	Tags
	{
		"Queue"="Transparent"
		"IgnoreProjector"="True"
		"RenderType"="Transparent"
	}

	Stencil
	{
		Ref [_Stencil]
		Comp [_StencilComp]
		Pass [_StencilOp]
		ReadMask [_StencilReadMask]
		WriteMask [_StencilWriteMask]
	}

	Cull [_CullMode]
	ZWrite Off
	Lighting Off
	Fog { Mode Off }
	ZTest [unity_GUIZTestMode]
	Blend One OneMinusSrcAlpha
	ColorMask [_ColorMask]

	Pass
	{
		CGPROGRAM
		#pragma target 3.0
		#pragma vertex VertShader
		#pragma fragment PixShader

		#pragma multi_compile __ UNITY_UI_CLIP_RECT
		#pragma multi_compile __ UNITY_UI_ALPHACLIP

		#include "UnityCG.cginc"
		#include "UnityUI.cginc"

		// ── Properties ───────────────────────────────────────────────────────
		sampler2D	_FaceTex;
		float		_FaceUVSpeedX;
		float		_FaceUVSpeedY;
		fixed4		_FaceColor;
		float		_FaceDilate;

		sampler2D	_OutlineTex;
		float		_OutlineUVSpeedX;
		float		_OutlineUVSpeedY;
		fixed4		_OutlineColor;
		float		_OutlineWidth;
		float		_OutlineSoftness;

		float		_WeightNormal;
		float		_WeightBold;
		float		_ScaleRatioA;

		sampler2D	_MainTex;
		float		_TextureWidth;
		float		_TextureHeight;
		float		_GradientScale;
		float		_ScaleX;
		float		_ScaleY;
		float		_PerspectiveFilter;
		float		_Sharpness;

		float		_VertexOffsetX;
		float		_VertexOffsetY;

		float4		_ClipRect;
		float		_MaskSoftnessX;
		float		_MaskSoftnessY;

		fixed4		_Outline2Color;
		float		_Outline2Width;
		float		_InnerOutward;
		float		_InnerSoftness;
		float		_OuterRenderWidth;
		float		_OuterSoftness;

		// Shadow
		fixed4		_ShadowColor;
		float		_ShadowOffsetX;
		float		_ShadowOffsetY;
		float		_ShadowDilate;
		float		_ShadowSoftness;
		float		_AtlasPadding;

		// Glow
		fixed4		_GlowColor;
		float		_GlowOffset;
		float		_GlowInner;
		float		_GlowOuter;
		float		_GlowPower;

		// Lighting
		float		_LightAngle;
		fixed4		_SpecularColor;
		float		_SpecularPower;
		float		_Reflectivity;
		float		_Diffuse;
		float		_Ambient;

		// ── Structs ──────────────────────────────────────────────────────────
		struct vertex_t
		{
			UNITY_VERTEX_INPUT_INSTANCE_ID
			float4	position	: POSITION;
			float3	normal		: NORMAL;
			fixed4	color		: COLOR;
			float4	texcoord0	: TEXCOORD0;
			float2	texcoord1	: TEXCOORD1;
		};

		struct pixel_t
		{
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
			float4	position	: SV_POSITION;
			fixed4	color		: COLOR;
			float2	atlas		: TEXCOORD0;
			float4	param		: TEXCOORD1;	// x=alphaClip, y=scale, z=bias, w=weight
			float4	mask		: TEXCOORD2;
			float4	textures	: TEXCOORD3;
			float2	shadowUV	: TEXCOORD4;	// shadow UV offset (pixel→SDF)
			float	shadowClip	: TEXCOORD5;	// separate alphaClip for shadow
		};

		float4	_FaceTex_ST;
		float4	_OutlineTex_ST;
		float	_UIMaskSoftnessX;
		float	_UIMaskSoftnessY;
		int		_UIVertexColorAlwaysGammaSpace;

		// ── Vertex (giữ nguyên logic TMP gốc) ───────────────────────────────
		pixel_t VertShader(vertex_t input)
		{
			pixel_t output;
			UNITY_INITIALIZE_OUTPUT(pixel_t, output);
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_TRANSFER_INSTANCE_ID(input, output);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

			float bold = step(input.texcoord0.w, 0);

			float4 vert = input.position;
			vert.x += _VertexOffsetX;
			vert.y += _VertexOffsetY;

			float4 vPosition = UnityObjectToClipPos(vert);

			float2 pixelSize = vPosition.w;
			pixelSize /= float2(_ScaleX, _ScaleY) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
			float scale = rsqrt(dot(pixelSize, pixelSize));
			scale *= abs(input.texcoord0.w) * _GradientScale * (_Sharpness + 1);
			if (UNITY_MATRIX_P[3][3] == 0) scale = lerp(abs(scale) * (1 - _PerspectiveFilter), scale, abs(dot(UnityObjectToWorldNormal(input.normal.xyz), normalize(WorldSpaceViewDir(vert)))));

			float weight = lerp(_WeightNormal, _WeightBold, bold) / 4.0;
			weight = (weight + _FaceDilate) * _ScaleRatioA * 0.5;

			float bias = (.5 - weight) + (.5 / scale);

			// alphaClip cho text + outline (không bao gồm shadow)
			float alphaClip = (1.0 - _OutlineWidth * _ScaleRatioA - _OutlineSoftness * _ScaleRatioA);
			alphaClip = alphaClip / 2.0 - (.5 / scale) - weight;
			alphaClip = max(alphaClip, 0.001);

			// Shadow UV offset (texels → UV space)
			// Y negated so positive Inspector value = shadow downward on screen.
			// Clamp to atlas padding to prevent cross-glyph sampling.
			// Nếu offset > padding, shadow tự động bị limit vào vùng an toàn.
			// Tăng Atlas Padding (regenerate font) để mở rộng shadow range.
			float2 sUV = float2(_ShadowOffsetX, -_ShadowOffsetY) / float2(_TextureWidth, _TextureHeight);
			float2 safeRange = _AtlasPadding / float2(_TextureWidth, _TextureHeight);
			sUV = clamp(sUV, -safeRange, safeRange);

			// Separate alphaClip cho shadow — cần rộng hơn để chứa shadow offset + dilate + softness
			// Shadow extent trong SDF space
			float shadowExtentSDF = (max(abs(_ShadowOffsetX), abs(_ShadowOffsetY)) / _GradientScale
			                       + _ShadowDilate + _ShadowSoftness) * _ScaleRatioA;
			float shadowClipVal = (1.0 - (_OutlineWidth + shadowExtentSDF) * _ScaleRatioA - _OutlineSoftness * _ScaleRatioA);
			shadowClipVal = shadowClipVal / 2.0 - (.5 / scale) - weight;
			shadowClipVal = max(shadowClipVal, -1.0); // Cho phép âm để shadow không bị clip

			float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
			float2 faceUV    = TRANSFORM_TEX(input.texcoord1, _FaceTex);
			float2 outlineUV = TRANSFORM_TEX(input.texcoord1, _OutlineTex);

			if (_UIVertexColorAlwaysGammaSpace && !IsGammaSpace())
				input.color.rgb = UIGammaToLinear(input.color.rgb);

			output.position   = vPosition;
			output.color      = input.color;
			output.atlas      = input.texcoord0;
			output.param      = float4(alphaClip, scale, bias, weight);
			output.shadowUV   = sUV;
			output.shadowClip = shadowClipVal;

			const half2 maskSoftness = half2(max(_UIMaskSoftnessX, _MaskSoftnessX), max(_UIMaskSoftnessY, _MaskSoftnessY));
			output.mask     = half4(vert.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * maskSoftness + pixelSize.xy));
			output.textures = float4(faceUV, outlineUV);

			return output;
		}

		// ── Fragment ─────────────────────────────────────────────────────────
		// Layers (back to front): Shadow → Outer → Inner → Face
		fixed4 PixShader(pixel_t input) : SV_Target
		{
			UNITY_SETUP_INSTANCE_ID(input);

			float c = tex2D(_MainTex, input.atlas).a;

			// Shadow: sample SDF tại (atlas - offset)
			// Shadow dịch (dx,dy) → tại pixel P, bóng = text ở (P - offset)
			float cS = tex2D(_MainTex, input.atlas.xy - input.shadowUV).a;

			// Clip: giữ pixel nếu có text data HOẶC shadow data
			float textPass   = c - input.param.x;
			float shadowPass = cS * step(0.001, _ShadowColor.a) - max(input.shadowClip, 0.001);
			clip(max(textPass, shadowPass));

			float scale = input.param.y;
			float bias  = input.param.z;
			float sd    = (bias - c) * scale;

			// ── Outline widths (screen-space) ──
			float outerWidth = (_OuterRenderWidth * _ScaleRatioA) * scale;
			float innerIn    = (_Outline2Width   * _ScaleRatioA) * scale;
			float innerOut   = (_InnerOutward    * _ScaleRatioA) * scale;

			// ── Softness (screen-space) ──
			float innerSoft = max(1.0, _InnerSoftness * _ScaleRatioA * scale);
			float outerSoft = max(1.0, _OuterSoftness * _ScaleRatioA * scale);

			// ── Boundary masks ──
			// Khi softness = 0: max(1,0) = 1 → smoothstep(1,0,x) ≈ saturate(x) (sharp edge)
			// Khi softness > 0: transition mượt hơn
			float pureFaceMask = smoothstep(innerSoft, 0, sd - (0.5 - innerIn));
			float faceMask     = smoothstep(innerSoft, 0, sd - 0.5);
			float innerOutMask = smoothstep(innerSoft, 0, sd - (0.5 + innerOut));
			float outerEdgeMask = smoothstep(outerSoft, 0, sd - (0.5 + outerWidth));

			float maxOutward = max(outerWidth, innerOut);
			float maxSoft = max(innerSoft, outerSoft);
			float totalMask = smoothstep(maxSoft, 0, sd - (0.5 + maxOutward));

			// Ngăn outline/shadow tràn ra ngoài vùng SDF hợp lệ
			float sdfLimit = smoothstep(0.0, 0.05, c);
			totalMask *= sdfLimit;
			outerEdgeMask *= sdfLimit;
			innerOutMask *= sdfLimit;

			// ── Lấy màu từng layer ──
			half3 fRGB = _FaceColor.rgb * input.color.rgb;
			fRGB *= tex2D(_FaceTex, input.textures.xy + float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.y).rgb;

			// ── Lighting (Bevel) ── Modulate face color bằng SDF normals ──
			if (_Diffuse > 0.001 || _Reflectivity > 0.001)
			{
				// Pseudo-normals từ SDF gradient (screen-space derivatives)
				float2 sdfGrad = float2(ddx(c), ddy(c));
				float3 normal = normalize(float3(sdfGrad * 200.0, 1.0));

				// Light direction từ angle (XY plane)
				float3 lightDir = normalize(float3(cos(_LightAngle), sin(_LightAngle), 0.5));

				// Diffuse (Lambert)
				float diffuse = max(0, dot(normal, lightDir));
				float lighting = lerp(_Ambient, 1.0, diffuse * _Diffuse);
				fRGB *= lighting;

				// Specular (Blinn-Phong)
				float3 halfDir = normalize(lightDir + float3(0, 0, 1));
				float spec = pow(max(0, dot(normal, halfDir)), _SpecularPower * 128.0 + 1.0);
				fRGB += _SpecularColor.rgb * spec * _Reflectivity * faceMask;
			}

			half3 innerRGB = _OutlineColor.rgb;
			innerRGB *= tex2D(_OutlineTex, input.textures.zw + float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.y).rgb;

			half3 outerRGB = _Outline2Color.rgb;

			// ── Alpha từ outline color ──
			float innerAlpha = _OutlineColor.a;
			float outerAlpha = _Outline2Color.a;

			// ── Composite back-to-front: Outer → Inner → Face ──
			float hasInner = step(0.001, innerIn + innerOut);

			half3 color = outerRGB;
			float outerZoneMask = totalMask * outerAlpha;

			color = lerp(color, innerRGB, innerOutMask * hasInner * innerAlpha);
			color = lerp(color, innerRGB, faceMask * hasInner * innerAlpha);

			float effectiveFaceMask = lerp(faceMask, pureFaceMask, hasInner);
			color = lerp(color, fRGB, effectiveFaceMask);

			// ── Final alpha ──
			float innerZoneMask = saturate(max(innerOutMask, faceMask) - effectiveFaceMask) * hasInner;
			float outerOnlyMask = saturate(totalMask - max(innerOutMask * hasInner, faceMask));
			float textAlpha = effectiveFaceMask
			                + innerZoneMask * innerAlpha
			                + outerOnlyMask * outerAlpha;

			// ── Glow ── Soft halo ring quanh text edge ──
			float glowAlpha = 0;
			half3 glowRGB = _GlowColor.rgb;
			if (_GlowColor.a > 0.001)
			{
				float glowOffset = _GlowOffset * _ScaleRatioA * scale;
				float glowInner = max(0.001, _GlowInner * _ScaleRatioA * scale);
				float glowOuter = max(0.001, _GlowOuter * _ScaleRatioA * scale);

				// Distance from glow center (offset from text edge)
				float glowDist = abs(sd - (0.5 + glowOffset));

				// Smooth ring: 1 at center, fading both directions
				float glowMask = 1.0 - smoothstep(glowInner, glowOuter, glowDist);
				glowMask = pow(saturate(glowMask), _GlowPower);
				glowMask *= sdfLimit; // prevent SDF bleed

				glowAlpha = glowMask * _GlowColor.a;
			}

			// ── Shadow ──
			float shadowAlpha = 0;
			if (_ShadowColor.a > 0.001)
			{
				float sdS = (bias - cS) * scale;
				float dilate = _ShadowDilate * _ScaleRatioA * scale;
				float soft = max(1.0, _ShadowSoftness * _ScaleRatioA * scale);

				shadowAlpha = _ShadowColor.a * smoothstep(soft, 0, sdS - dilate - maxOutward);
				shadowAlpha *= smoothstep(0.0, 0.05, cS);
			}

			// ── Composite: Text → Glow (behind text) → Shadow (back-most) ──
			// Layer 1: Glow behind text
			float textGlowAlpha = textAlpha + glowAlpha * (1.0 - textAlpha);
			half3 textGlowColor = (textGlowAlpha > 0.001)
				? (color * textAlpha + glowRGB * glowAlpha * (1.0 - textAlpha)) / textGlowAlpha
				: half3(0,0,0);

			// Layer 2: Shadow behind text+glow
			half3 shadowRGB = _ShadowColor.rgb;
			float finalAlpha = textGlowAlpha + shadowAlpha * (1.0 - textGlowAlpha);
			half3 finalColor = (finalAlpha > 0.001)
				? (textGlowColor * textGlowAlpha + shadowRGB * shadowAlpha * (1.0 - textGlowAlpha)) / finalAlpha
				: half3(0,0,0);

			// Output premultiplied alpha
			half4 result;
			result.rgb = finalColor * finalAlpha;
			result.a   = finalAlpha;

			#if UNITY_UI_CLIP_RECT
			half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(input.mask.xy)) * input.mask.zw);
			result *= m.x * m.y;
			#endif

			#if UNITY_UI_ALPHACLIP
			clip(result.a - 0.001);
			#endif

			return result * input.color.a;
		}
		ENDCG
	}
}

Fallback "TextMeshPro/Mobile/Distance Field"
}
