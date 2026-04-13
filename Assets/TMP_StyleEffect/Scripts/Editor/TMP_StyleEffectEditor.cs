using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TMP_StyleEffect))]
public class TMP_StyleEffectEditor : Editor
{
    // ── Face ──
    SerializedProperty _faceColor;

    // ── Gradient ──
    SerializedProperty _useGradient, _gradientMode;
    SerializedProperty _gradientTopLeft, _gradientTopRight, _gradientBottomLeft, _gradientBottomRight;

    // ── Outline Inner ──
    SerializedProperty _innerEnabled, _innerInward, _innerOutward, _innerSoftness, _innerColor;

    // ── Outline Outer ──
    SerializedProperty _outerEnabled, _outline2Width, _outline2Softness, _outline2Color;

    // ── Drop Shadow ──
    SerializedProperty _shadowEnabled, _shadowColor, _shadowOffset, _shadowDilate, _shadowSoftness;

    // ── Glow ──
    SerializedProperty _glowEnabled, _glowColor, _glowOffset, _glowInner, _glowOuter, _glowPower;

    // ── Lighting ──
    SerializedProperty _lightingEnabled, _lightAngle, _specularColor, _specularPower, _reflectivity, _diffuse, _ambient;

    // ── Advanced ──
    SerializedProperty _atlasPaddingOverride, _extraPadding, _maxSafeOffset;

    // ── Editor state ──
    const string PrefPrefix = "TMP_SE_";

    // Colors matching URP Volume component (dark theme)
    static readonly Color HeaderBgColor = new Color(0.1f, 0.1f, 0.1f, 0.2f);
    static readonly Color SplitterColor = new Color(0.12f, 0.12f, 0.12f, 1.333f);

    void OnEnable()
    {
        _faceColor = serializedObject.FindProperty("faceColor");

        _useGradient = serializedObject.FindProperty("useGradient");
        _gradientMode = serializedObject.FindProperty("gradientMode");
        _gradientTopLeft = serializedObject.FindProperty("gradientTopLeft");
        _gradientTopRight = serializedObject.FindProperty("gradientTopRight");
        _gradientBottomLeft = serializedObject.FindProperty("gradientBottomLeft");
        _gradientBottomRight = serializedObject.FindProperty("gradientBottomRight");

        _innerEnabled = serializedObject.FindProperty("innerEnabled");
        _innerInward = serializedObject.FindProperty("innerInward");
        _innerOutward = serializedObject.FindProperty("innerOutward");
        _innerSoftness = serializedObject.FindProperty("innerSoftness");
        _innerColor = serializedObject.FindProperty("innerColor");

        _outerEnabled = serializedObject.FindProperty("outerEnabled");
        _outline2Width = serializedObject.FindProperty("outline2Width");
        _outline2Softness = serializedObject.FindProperty("outline2Softness");
        _outline2Color = serializedObject.FindProperty("outline2Color");

        _shadowEnabled = serializedObject.FindProperty("shadowEnabled");
        _shadowColor = serializedObject.FindProperty("shadowColor");
        _shadowOffset = serializedObject.FindProperty("shadowOffset");
        _shadowDilate = serializedObject.FindProperty("shadowDilate");
        _shadowSoftness = serializedObject.FindProperty("shadowSoftness");

        _glowEnabled = serializedObject.FindProperty("glowEnabled");
        _glowColor = serializedObject.FindProperty("glowColor");
        _glowOffset = serializedObject.FindProperty("glowOffset");
        _glowInner = serializedObject.FindProperty("glowInner");
        _glowOuter = serializedObject.FindProperty("glowOuter");
        _glowPower = serializedObject.FindProperty("glowPower");

        _lightingEnabled = serializedObject.FindProperty("lightingEnabled");
        _lightAngle = serializedObject.FindProperty("lightAngle");
        _specularColor = serializedObject.FindProperty("specularColor");
        _specularPower = serializedObject.FindProperty("specularPower");
        _reflectivity = serializedObject.FindProperty("reflectivity");
        _diffuse = serializedObject.FindProperty("diffuse");
        _ambient = serializedObject.FindProperty("ambient");

        _atlasPaddingOverride = serializedObject.FindProperty("atlasPaddingOverride");
        _extraPadding = serializedObject.FindProperty("extraPadding");
        _maxSafeOffset = serializedObject.FindProperty("_maxSafeOffset");
    }

    // ─────────────────────────────────────────────────────────────────────
    // Volume-style header helpers
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>1px dark line giữa các sections — giống Volume splitter.</summary>
    static void DrawSplitter()
    {
        var rect = GUILayoutUtility.GetRect(1f, 1f);
        rect.xMin = 0f;
        rect.width += 4f;
        if (Event.current.type == EventType.Repaint)
            EditorGUI.DrawRect(rect, SplitterColor);
    }

    /// <summary>
    /// Volume-style dark header. Returns true if expanded.
    /// </summary>
    bool DrawSectionHeader(string title, SerializedProperty enableProp = null)
    {
        string prefKey = PrefPrefix + title.Replace(" ", "");
        bool expanded = EditorPrefs.GetBool(prefKey, true);

        DrawSplitter();

        // ── Header rect ──
        var headerRect = GUILayoutUtility.GetRect(1f, 22f);
        var bgRect = headerRect;
        bgRect.xMin = 0f;
        bgRect.xMax += 4f;

        // Background
        EditorGUI.DrawRect(bgRect, HeaderBgColor);

        // ── Layout positions ──
        float x = headerRect.x;

        // Foldout arrow rect
        var arrowRect = new Rect(x, headerRect.y + 4f, 13f, 13f);
        x += 16f;

        // Checkbox rect — luôn hiện, căn chỉnh cùng chiều cao với text
        var toggleRect = new Rect(x, headerRect.y + 4f, 13f, 13f);
        x += 18f;

        // Title rect
        var titleRect = new Rect(x, headerRect.y + 2f, headerRect.width - x, 17f);

        // ── Click handling (trước khi draw → không consume event) ──
        Event e = Event.current;
        if (e.type == EventType.MouseDown && headerRect.Contains(e.mousePosition))
        {
            if (enableProp != null && toggleRect.Contains(e.mousePosition))
            {
                enableProp.boolValue = !enableProp.boolValue;
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                expanded = !expanded;
                EditorPrefs.SetBool(prefKey, expanded);
            }
            e.Use();
        }

        // ── Draw visuals ──
        // Arrow
        if (Event.current.type == EventType.Repaint)
            EditorStyles.foldout.Draw(arrowRect, false, false, expanded, false);

        // Checkbox — luôn hiện
        if (enableProp != null)
        {
            // Editable checkbox
            EditorGUI.BeginChangeCheck();
            bool val = GUI.Toggle(toggleRect, enableProp.boolValue, GUIContent.none, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck())
                enableProp.boolValue = val;
        }
        else
        {
            // Always-on, disabled checkbox
            EditorGUI.BeginDisabledGroup(true);
            GUI.Toggle(toggleRect, true, GUIContent.none, EditorStyles.toggle);
            EditorGUI.EndDisabledGroup();
        }

        // Title
        EditorGUI.LabelField(titleRect, title, EditorStyles.boldLabel);

        return expanded;
    }

    // ─────────────────────────────────────────────────────────────────────
    // Main Inspector
    // ─────────────────────────────────────────────────────────────────────

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(2);

        DrawFaceSection();
        DrawGradientSection();
        DrawInnerOutlineSection();
        DrawOuterOutlineSection();
        DrawDropShadowSection();
        DrawGlowSection();
        DrawLightingSection();
        DrawAdvancedSection();

        // Bottom splitter
        DrawSplitter();

        serializedObject.ApplyModifiedProperties();
    }

    // ── Face ─────────────────────────────────────────────────────────────

    void DrawFaceSection()
    {
        if (!DrawSectionHeader("Face")) return;

        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(_faceColor, new GUIContent("Color"));
        EditorGUI.indentLevel--;
        EditorGUILayout.Space(2);
    }

    // ── Gradient ─────────────────────────────────────────────────────────

    void DrawGradientSection()
    {
        if (!DrawSectionHeader("Gradient", _useGradient)) return;

        EditorGUI.BeginDisabledGroup(!_useGradient.boolValue);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(_gradientMode, new GUIContent("Color Mode"));

        var mode = (GradientColorMode)_gradientMode.enumValueIndex;

        switch (mode)
        {
            case GradientColorMode.Single:
                EditorGUILayout.PropertyField(_gradientTopLeft, new GUIContent("Color"));
                break;

            case GradientColorMode.HorizontalGradient:
                EditorGUILayout.PropertyField(_gradientTopLeft, new GUIContent("Left"));
                EditorGUILayout.PropertyField(_gradientTopRight, new GUIContent("Right"));
                break;

            case GradientColorMode.VerticalGradient:
                EditorGUILayout.PropertyField(_gradientTopLeft, new GUIContent("Top"));
                EditorGUILayout.PropertyField(_gradientBottomLeft, new GUIContent("Bottom"));
                break;

            case GradientColorMode.FourCornersGradient:
                // 2x2 grid layout giống TMP
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_gradientTopLeft, new GUIContent("Top Left"));
                EditorGUILayout.PropertyField(_gradientTopRight, new GUIContent("Top Right"));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_gradientBottomLeft, new GUIContent("Bot Left"));
                EditorGUILayout.PropertyField(_gradientBottomRight, new GUIContent("Bot Right"));
                EditorGUILayout.EndHorizontal();
                break;
        }

        EditorGUI.indentLevel--;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(2);
    }

    // ── Outline Inner ────────────────────────────────────────────────────

    void DrawInnerOutlineSection()
    {
        if (!DrawSectionHeader("Outline Inner", _innerEnabled)) return;

        EditorGUI.BeginDisabledGroup(!_innerEnabled.boolValue);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(_innerInward, new GUIContent("Inward"));
        EditorGUILayout.PropertyField(_innerOutward, new GUIContent("Outward"));
        EditorGUILayout.PropertyField(_innerSoftness, new GUIContent("Softness"));
        EditorGUILayout.PropertyField(_innerColor, new GUIContent("Color"));

        EditorGUI.indentLevel--;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(2);
    }

    // ── Outline Outer ────────────────────────────────────────────────────

    void DrawOuterOutlineSection()
    {
        if (!DrawSectionHeader("Outline Outer", _outerEnabled)) return;

        EditorGUI.BeginDisabledGroup(!_outerEnabled.boolValue);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(_outline2Width, new GUIContent("Width"));
        EditorGUILayout.PropertyField(_outline2Softness, new GUIContent("Softness"));
        EditorGUILayout.PropertyField(_outline2Color, new GUIContent("Color"));

        EditorGUI.indentLevel--;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(2);
    }

    // ── Drop Shadow ──────────────────────────────────────────────────────

    void DrawDropShadowSection()
    {
        if (!DrawSectionHeader("Drop Shadow", _shadowEnabled)) return;

        EditorGUI.BeginDisabledGroup(!_shadowEnabled.boolValue);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(_shadowColor, new GUIContent("Color"));
        EditorGUILayout.PropertyField(_shadowOffset, new GUIContent("Offset"));
        EditorGUILayout.PropertyField(_shadowDilate, new GUIContent("Dilate"));
        EditorGUILayout.PropertyField(_shadowSoftness, new GUIContent("Softness"));

        EditorGUI.indentLevel--;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(2);
    }
    // ── Glow ────────────────────────────────────────────────────────────

    void DrawGlowSection()
    {
        if (!DrawSectionHeader("Glow", _glowEnabled)) return;

        EditorGUI.BeginDisabledGroup(!_glowEnabled.boolValue);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(_glowColor, new GUIContent("Color"));
        EditorGUILayout.PropertyField(_glowOffset, new GUIContent("Offset"));
        EditorGUILayout.PropertyField(_glowInner, new GUIContent("Inner"));
        EditorGUILayout.PropertyField(_glowOuter, new GUIContent("Outer"));
        EditorGUILayout.PropertyField(_glowPower, new GUIContent("Power"));

        EditorGUI.indentLevel--;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(2);
    }

    // ── Lighting ──────────────────────────────────────────────────────────

    void DrawLightingSection()
    {
        if (!DrawSectionHeader("Lighting", _lightingEnabled)) return;

        EditorGUI.BeginDisabledGroup(!_lightingEnabled.boolValue);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(_lightAngle, new GUIContent("Light Angle"));
        EditorGUILayout.PropertyField(_specularColor, new GUIContent("Specular Color"));
        EditorGUILayout.PropertyField(_specularPower, new GUIContent("Specular Power"));
        EditorGUILayout.PropertyField(_reflectivity, new GUIContent("Reflectivity"));
        EditorGUILayout.PropertyField(_diffuse, new GUIContent("Diffuse"));
        EditorGUILayout.PropertyField(_ambient, new GUIContent("Ambient"));

        EditorGUI.indentLevel--;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(2);
    }

    // ── Advanced ─────────────────────────────────────────────────────────

    void DrawAdvancedSection()
    {
        if (!DrawSectionHeader("Advanced")) return;

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(_atlasPaddingOverride,
            new GUIContent("Atlas Padding Override",
                "0 = tự đọc từ Font Asset. Tăng giá trị này (và regenerate font) để mở rộng shadow range."));
        EditorGUILayout.PropertyField(_extraPadding,
            new GUIContent("Extra Padding",
                "Bật thêm padding để đảm bảo outline/shadow không bị cắt."));

        var effect = (TMP_StyleEffect)target;
        if (effect.shadowEnabled && _maxSafeOffset != null)
        {
            EditorGUILayout.Space(4);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField(
                new GUIContent("Max Safe Offset",
                    "Shadow offset tối đa (texels) mà không bị clamp."),
                _maxSafeOffset.floatValue
            );
            EditorGUI.EndDisabledGroup();

            float maxOffset = Mathf.Max(
                Mathf.Abs(effect.shadowOffset.x),
                Mathf.Abs(effect.shadowOffset.y)
            );
            if (maxOffset > _maxSafeOffset.floatValue && _maxSafeOffset.floatValue > 0)
            {
                EditorGUILayout.HelpBox(
                    $"Shadow offset ({maxOffset:F1}) > max safe ({_maxSafeOffset.floatValue:F1}). " +
                    "Shadow sẽ bị clamp lại gần text hơn.\n" +
                    "→ Tăng Atlas Padding Override hoặc regenerate font với padding lớn hơn.",
                    MessageType.Warning
                );
            }
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.Space(2);
    }
}
