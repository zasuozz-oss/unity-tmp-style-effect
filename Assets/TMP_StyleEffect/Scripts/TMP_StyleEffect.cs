using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshProUGUI))]
public class TMP_StyleEffect : MonoBehaviour
{
    [Header("Face")]
    public Color faceColor = Color.white;
    public bool useGradient = false;
    public Color gradientTop    = Color.black;
    public Color gradientBottom = Color.black;

    [Header("Outline Inner")]
    public bool innerEnabled = false;
    [Range(0f, 0.5f)] public float innerInward = 0f;
    [Range(0f, 0.5f)] public float innerOutward = 0f;
    public Color innerColor = Color.black;

    [Header("Outline Outer")]
    [Range(0f, 1f)] public float outline2Width = 0f;
    public Color outline2Color = Color.black;

    [Header("Drop Shadow")]
    public bool shadowEnabled = false;
    public Color shadowColor = Color.black;
    [Tooltip("Shadow offset in texels. Positive X = RIGHT, Positive Y = DOWN")]
    public Vector2 shadowOffset = Vector2.zero;
    [Range(0f, 1f)] public float shadowDilate = 0f;
    [Range(0f, 1f)] public float shadowSoftness = 0f;

    [Header("Underlay")]
    [Tooltip("Override Atlas Padding. 0 = tự đọc từ Font Asset. Tăng giá trị này (và regenerate font) để mở rộng shadow range.")]
    [Min(0)] public int atlasPaddingOverride = 0;
    [Tooltip("Max shadow offset an toàn (texels). Tự tính từ Atlas Padding - GradientScale/2.")]
    [SerializeField, HideInInspector] private float _maxSafeOffset = 0;

    [Header("Extra Padding")]
    [Tooltip("Bật thêm padding để đảm bảo outline/shadow không bị cắt.")]
    public bool extraPadding = true;

    static readonly int ID_FaceColor        = Shader.PropertyToID("_FaceColor");
    static readonly int ID_OutlineColor     = Shader.PropertyToID("_OutlineColor");
    static readonly int ID_OutlineWidth     = Shader.PropertyToID("_OutlineWidth");
    static readonly int ID_Outline2Color    = Shader.PropertyToID("_Outline2Color");
    static readonly int ID_Outline2Width    = Shader.PropertyToID("_Outline2Width");
    static readonly int ID_InnerOutward     = Shader.PropertyToID("_InnerOutward");
    static readonly int ID_OuterRenderWidth = Shader.PropertyToID("_OuterRenderWidth");
    static readonly int ID_ShadowColor      = Shader.PropertyToID("_ShadowColor");
    static readonly int ID_ShadowOffsetX    = Shader.PropertyToID("_ShadowOffsetX");
    static readonly int ID_ShadowOffsetY    = Shader.PropertyToID("_ShadowOffsetY");
    static readonly int ID_ShadowDilate     = Shader.PropertyToID("_ShadowDilate");
    static readonly int ID_ShadowSoftness   = Shader.PropertyToID("_ShadowSoftness");
    static readonly int ID_AtlasPadding     = Shader.PropertyToID("_AtlasPadding");
    // TMP reads these for auto-padding via GetPaddingForMaterial
    static readonly int ID_UnderlayOffsetX  = Shader.PropertyToID("_UnderlayOffsetX");
    static readonly int ID_UnderlayOffsetY  = Shader.PropertyToID("_UnderlayOffsetY");
    static readonly int ID_UnderlayDilate   = Shader.PropertyToID("_UnderlayDilate");
    static readonly int ID_UnderlaySoftness = Shader.PropertyToID("_UnderlaySoftness");

    TextMeshProUGUI _tmp;
    Material        _mat;
    bool            _applying;

    void OnEnable()   => Apply();
    void OnValidate() => Apply();

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
            if (_mat == null || _mat.shader != shader)
            {
                if (_mat != null)
                    DestroyImmediate(_mat);
                _mat = new Material(_tmp.font.material);
                _mat.shader = shader;
                _mat.name = "StyleEffect_Instance";
            }

            // QUAN TRỌNG: TMP nhân vertex color vào faceColor trong shader
            _tmp.color = Color.white;

            // Gradient
            _tmp.enableVertexGradient = useGradient;
            if (useGradient)
            {
                _mat.SetColor(ID_FaceColor, Color.white);
                _tmp.colorGradient = new VertexGradient(gradientTop, gradientTop, gradientBottom, gradientBottom);
            }
            else
            {
                _mat.SetColor(ID_FaceColor, faceColor);
            }

            // ── Inner outline ──
            float inIn  = innerEnabled ? innerInward  : 0f;
            float inOut = innerEnabled ? innerOutward : 0f;
            _mat.SetColor(ID_OutlineColor, innerColor);
            _mat.SetFloat(ID_Outline2Width, inIn);
            _mat.SetFloat(ID_InnerOutward, inOut);

            // ── Outer outline ──
            _mat.SetColor(ID_Outline2Color, outline2Color);
            _mat.SetFloat(ID_OuterRenderWidth, outline2Width);

            // _OutlineWidth → TMP dùng để tính padding cho quad mesh & alphaClip
            float paddingWidth = Mathf.Max(outline2Width, inOut);

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
