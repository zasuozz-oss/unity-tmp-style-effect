using UnityEngine;
using TMPro;

public enum GradientColorMode
{
    Single,
    HorizontalGradient,
    VerticalGradient,
    FourCornersGradient
}

[ExecuteAlways]
[RequireComponent(typeof(TextMeshProUGUI))]
[AddComponentMenu("UI/TMP Style Effect")]
public class TMP_StyleEffect : MonoBehaviour
{
    // ── Face ──
    public Color faceColor = Color.white;
    public bool useGradient = false;
    public GradientColorMode gradientMode = GradientColorMode.VerticalGradient;
    public Color gradientTopLeft = Color.white;
    public Color gradientTopRight = Color.white;
    public Color gradientBottomLeft = Color.white;
    public Color gradientBottomRight = Color.white;

    // ── Outline Inner ──
    public bool innerEnabled = false;
    [Range(0f, 0.5f)] public float innerInward = 0f;
    [Range(0f, 0.5f)] public float innerOutward = 0f;
    [Range(0f, 1f)] public float innerSoftness = 0f;
    public Color innerColor = Color.white;

    // ── Outline Outer ──
    public bool outerEnabled = false;
    [Range(0f, 1f)] public float outline2Width = 0f;
    [Range(0f, 1f)] public float outline2Softness = 0f;
    public Color outline2Color = Color.white;

    // ── Drop Shadow ──
    public bool shadowEnabled = false;
    public Color shadowColor = Color.white;
    [Tooltip("Shadow offset in texels. Positive X = RIGHT, Positive Y = DOWN")]
    public Vector2 shadowOffset = Vector2.zero;
    [Range(0f, 1f)] public float shadowDilate = 0f;
    [Range(0f, 1f)] public float shadowSoftness = 0f;

    // ── Underlay ──
    [Tooltip("Override Atlas Padding. 0 = tự đọc từ Font Asset. Tăng giá trị này (và regenerate font) để mở rộng shadow range.")]
    [Min(0)] public int atlasPaddingOverride = 0;
    [Tooltip("Max shadow offset an toàn (texels). Tự tính từ Atlas Padding - GradientScale/2.")]
    [SerializeField, HideInInspector] private float _maxSafeOffset = 0;

    // ── Glow ──
    public bool glowEnabled = false;
    public Color glowColor = new Color(1f, 1f, 0f, 0.5f);
    [Range(-1f, 1f)] public float glowOffset = 0f;
    [Range(0f, 1f)] public float glowInner = 0.05f;
    [Range(0f, 1f)] public float glowOuter = 0.45f;
    [Range(0.1f, 8f)] public float glowPower = 1f;

    // ── Lighting ──
    public bool lightingEnabled = false;
    [Range(0f, 360f)] public float lightAngle = 180f;
    public Color specularColor = Color.white;
    [Range(0f, 1f)] public float specularPower = 0.5f;
    [Range(0f, 5f)] public float reflectivity = 1f;
    [Range(0f, 1f)] public float diffuse = 0.5f;
    [Range(0f, 1f)] public float ambient = 0.5f;

    // ── Extra Padding ──
    [Tooltip("Bật thêm padding để đảm bảo outline/shadow không bị cắt.")]
    public bool extraPadding = true;

    static readonly int ID_FaceColor = Shader.PropertyToID("_FaceColor");
    static readonly int ID_OutlineColor = Shader.PropertyToID("_OutlineColor");
    static readonly int ID_OutlineWidth = Shader.PropertyToID("_OutlineWidth");
    static readonly int ID_Outline2Color = Shader.PropertyToID("_Outline2Color");
    static readonly int ID_Outline2Width = Shader.PropertyToID("_Outline2Width");
    static readonly int ID_InnerOutward = Shader.PropertyToID("_InnerOutward");
    static readonly int ID_InnerSoftness = Shader.PropertyToID("_InnerSoftness");
    static readonly int ID_OuterRenderWidth = Shader.PropertyToID("_OuterRenderWidth");
    static readonly int ID_OuterSoftness = Shader.PropertyToID("_OuterSoftness");
    static readonly int ID_ShadowColor = Shader.PropertyToID("_ShadowColor");
    static readonly int ID_ShadowOffsetX = Shader.PropertyToID("_ShadowOffsetX");
    static readonly int ID_ShadowOffsetY = Shader.PropertyToID("_ShadowOffsetY");
    static readonly int ID_ShadowDilate = Shader.PropertyToID("_ShadowDilate");
    static readonly int ID_ShadowSoftness = Shader.PropertyToID("_ShadowSoftness");
    static readonly int ID_AtlasPadding = Shader.PropertyToID("_AtlasPadding");
    // TMP reads these for auto-padding via GetPaddingForMaterial
    static readonly int ID_UnderlayOffsetX = Shader.PropertyToID("_UnderlayOffsetX");
    static readonly int ID_UnderlayOffsetY = Shader.PropertyToID("_UnderlayOffsetY");
    static readonly int ID_UnderlayDilate = Shader.PropertyToID("_UnderlayDilate");
    static readonly int ID_UnderlaySoftness = Shader.PropertyToID("_UnderlaySoftness");
    // Glow
    static readonly int ID_GlowColor = Shader.PropertyToID("_GlowColor");
    static readonly int ID_GlowOffset = Shader.PropertyToID("_GlowOffset");
    static readonly int ID_GlowInner = Shader.PropertyToID("_GlowInner");
    static readonly int ID_GlowOuter = Shader.PropertyToID("_GlowOuter");
    static readonly int ID_GlowPower = Shader.PropertyToID("_GlowPower");
    // Lighting
    static readonly int ID_LightAngle = Shader.PropertyToID("_LightAngle");
    static readonly int ID_SpecularColor = Shader.PropertyToID("_SpecularColor");
    static readonly int ID_SpecularPower = Shader.PropertyToID("_SpecularPower");
    static readonly int ID_Reflectivity = Shader.PropertyToID("_Reflectivity");
    static readonly int ID_Diffuse = Shader.PropertyToID("_Diffuse");
    static readonly int ID_Ambient = Shader.PropertyToID("_Ambient");

    TextMeshProUGUI _tmp;
    Material _mat;
    Material _lastBaseMat;   // Track font base material để detect đổi font
    bool _applying;

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTMPChanged);
        Apply();
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTMPChanged);
    }

    void OnValidate() => Apply();

    /// <summary>
    /// Khi TMP regenerate mesh (đổi font, text, size…) → re-apply effect.
    /// Guard _applying ngăn vòng lặp vô hạn từ ForceMeshUpdate().
    /// </summary>
    void OnTMPChanged(Object obj)
    {
        if (obj == (Object)_tmp)
            Apply();
    }

    void Apply()
    {
        if (_applying) return;
        _applying = true;

        try
        {
            if (_tmp == null)
                _tmp = GetComponent<TextMeshProUGUI>();
            if (_tmp == null || _tmp.font == null)
                return;

            Shader shader = Shader.Find("TextMeshPro/TMP_StyleEffect");
            if (shader == null)
            {
                Debug.LogError("[TMP_StyleEffect] Shader không tồn tại.");
                return;
            }

            // Tạo material từ font base material, đổi shader
            Material baseMat = _tmp.font.material;
            bool fontChanged = _lastBaseMat != baseMat;

            if (_mat == null || _mat.shader != shader || fontChanged)
            {
                if (_mat != null)
                    DestroyImmediate(_mat);
                _mat = new Material(baseMat);
                _mat.shader = shader;
                _mat.name = "StyleEffect_Instance";
                _lastBaseMat = baseMat;
            }

            // LUÔN sync SDF properties mỗi lần Apply()
            // → fix: đổi font (atlas/gradient thay đổi), đổi resolution/aspect (ScaleRatio thay đổi)
            _mat.SetTexture("_MainTex", baseMat.GetTexture("_MainTex"));
            _mat.SetFloat("_GradientScale", baseMat.GetFloat("_GradientScale"));
            _mat.SetFloat("_TextureWidth", baseMat.GetFloat("_TextureWidth"));
            _mat.SetFloat("_TextureHeight", baseMat.GetFloat("_TextureHeight"));
            _mat.SetFloat("_ScaleRatioA", baseMat.GetFloat("_ScaleRatioA"));
            _mat.SetFloat("_ScaleRatioB", baseMat.GetFloat("_ScaleRatioB"));
            _mat.SetFloat("_ScaleRatioC", baseMat.GetFloat("_ScaleRatioC"));
            _mat.SetFloat("_WeightNormal", baseMat.GetFloat("_WeightNormal"));
            _mat.SetFloat("_WeightBold", baseMat.GetFloat("_WeightBold"));
            _mat.SetFloat("_Sharpness", baseMat.GetFloat("_Sharpness"));

            // QUAN TRỌNG: TMP nhân vertex color vào faceColor trong shader
            _tmp.color = Color.white;

            // Gradient
            _tmp.enableVertexGradient = useGradient;
            if (useGradient)
            {
                _mat.SetColor(ID_FaceColor, Color.white);

                VertexGradient vg;
                switch (gradientMode)
                {
                    case GradientColorMode.Single:
                        vg = new VertexGradient(gradientTopLeft);
                        break;
                    case GradientColorMode.HorizontalGradient:
                        vg = new VertexGradient(gradientTopLeft, gradientTopRight, gradientTopLeft, gradientTopRight);
                        break;
                    case GradientColorMode.VerticalGradient:
                        vg = new VertexGradient(gradientTopLeft, gradientTopLeft, gradientBottomLeft, gradientBottomLeft);
                        break;
                    default: // FourCornersGradient
                        vg = new VertexGradient(gradientTopLeft, gradientTopRight, gradientBottomLeft, gradientBottomRight);
                        break;
                }
                _tmp.colorGradient = vg;
            }
            else
            {
                _mat.SetColor(ID_FaceColor, faceColor);
            }

            // ── Inner outline ──
            float inIn = innerEnabled ? innerInward : 0f;
            float inOut = innerEnabled ? innerOutward : 0f;
            float inSoft = innerEnabled ? innerSoftness : 0f;
            _mat.SetColor(ID_OutlineColor, innerColor);
            _mat.SetFloat(ID_Outline2Width, inIn);
            _mat.SetFloat(ID_InnerOutward, inOut);
            _mat.SetFloat(ID_InnerSoftness, inSoft);

            // ── Outer outline ──
            float outW = outerEnabled ? outline2Width : 0f;
            float outSoft = outerEnabled ? outline2Softness : 0f;
            _mat.SetColor(ID_Outline2Color, outline2Color);
            _mat.SetFloat(ID_OuterRenderWidth, outW);
            _mat.SetFloat(ID_OuterSoftness, outSoft);

            // _OutlineWidth → TMP dùng để tính padding cho quad mesh & alphaClip
            // Phải bao gồm softness để vertex shader không clip pixel ở vùng soft edge
            float paddingWidth = Mathf.Max(outW + outSoft, inOut + inSoft);

            // ── Drop Shadow ──
            float gradientScale = _tmp.font.material.GetFloat("_GradientScale");
            if (shadowEnabled && gradientScale > 0)
            {
                _mat.SetColor(ID_ShadowColor, shadowColor);
                _mat.SetFloat(ID_ShadowOffsetX, shadowOffset.x);
                _mat.SetFloat(ID_ShadowOffsetY, shadowOffset.y);
                _mat.SetFloat(ID_ShadowDilate, shadowDilate);
                _mat.SetFloat(ID_ShadowSoftness, shadowSoftness);

                // Atlas padding → shader clamp shadow UV vào vùng an toàn
                int effectivePadding = atlasPaddingOverride > 0
                    ? atlasPaddingOverride
                    : _tmp.font.atlasPadding;
                _mat.SetFloat(ID_AtlasPadding, effectivePadding);

                // Cập nhật max safe offset để hiển thị trong Inspector
                _maxSafeOffset = effectivePadding - gradientScale * 0.5f;

                // TMP underlay properties → auto-padding
                float sdfOffsetX = shadowOffset.x / gradientScale;
                float sdfOffsetY = shadowOffset.y / gradientScale;
                _mat.SetFloat(ID_UnderlayOffsetX, sdfOffsetX);
                _mat.SetFloat(ID_UnderlayOffsetY, sdfOffsetY);
                _mat.SetFloat(ID_UnderlayDilate, shadowDilate);
                _mat.SetFloat(ID_UnderlaySoftness, shadowSoftness);

                // Cộng thêm shadow extent vào _OutlineWidth cho alphaClip
                float shadowExtent = Mathf.Max(Mathf.Abs(sdfOffsetX), Mathf.Abs(sdfOffsetY))
                                   + shadowDilate + shadowSoftness;
                paddingWidth = Mathf.Max(paddingWidth, paddingWidth + shadowExtent);
            }
            else
            {
                _mat.SetColor(ID_ShadowColor, new Color(0, 0, 0, 0));
                _mat.SetFloat(ID_UnderlayOffsetX, 0);
                _mat.SetFloat(ID_UnderlayOffsetY, 0);
                _mat.SetFloat(ID_UnderlayDilate, 0);
                _mat.SetFloat(ID_UnderlaySoftness, 0);
            }

            // ── Glow ──
            if (glowEnabled)
            {
                _mat.SetColor(ID_GlowColor, glowColor);
                _mat.SetFloat(ID_GlowOffset, glowOffset);
                _mat.SetFloat(ID_GlowInner, glowInner);
                _mat.SetFloat(ID_GlowOuter, glowOuter);
                _mat.SetFloat(ID_GlowPower, glowPower);

                // Glow extends outward → needs padding
                float glowExtent = Mathf.Abs(glowOffset) + glowOuter;
                paddingWidth = Mathf.Max(paddingWidth, glowExtent);
            }
            else
            {
                _mat.SetColor(ID_GlowColor, new Color(0, 0, 0, 0));
            }

            // ── Lighting ──
            if (lightingEnabled)
            {
                _mat.SetFloat(ID_LightAngle, lightAngle * Mathf.Deg2Rad);
                _mat.SetColor(ID_SpecularColor, specularColor);
                _mat.SetFloat(ID_SpecularPower, specularPower);
                _mat.SetFloat(ID_Reflectivity, reflectivity);
                _mat.SetFloat(ID_Diffuse, diffuse);
                _mat.SetFloat(ID_Ambient, ambient);
            }
            else
            {
                _mat.SetFloat(ID_Diffuse, 0);
                _mat.SetFloat(ID_Reflectivity, 0);
            }

            _mat.SetFloat(ID_OutlineWidth, Mathf.Min(paddingWidth, 1f));

            // Gán material
            _tmp.fontMaterial = _mat;

            // QUAN TRỌNG: Bật extraPadding để TMP thêm buffer cho quad mesh
            _tmp.extraPadding = extraPadding;

            // Force rebuild mesh (tính lại padding cho outline mới)
            _tmp.ForceMeshUpdate();
        }
        finally
        {
            _applying = false;
        }
    }

    void OnDestroy()
    {
        if (_mat != null)
            DestroyImmediate(_mat);
    }
}
