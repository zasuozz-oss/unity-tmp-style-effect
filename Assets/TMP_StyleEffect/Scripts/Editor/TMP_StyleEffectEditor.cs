using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TMP_StyleEffect))]
public class TMP_StyleEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var effect = (TMP_StyleEffect)target;

        // Hiển thị max safe offset (read-only)
        if (effect.shadowEnabled)
        {
            var maxSafe = serializedObject.FindProperty("_maxSafeOffset");
            if (maxSafe != null)
            {
                EditorGUILayout.Space(4);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.FloatField(
                    new GUIContent("Max Safe Offset", "Shadow offset tối đa (texels) mà không bị clamp. Tăng Atlas Padding để mở rộng."),
                    maxSafe.floatValue
                );
                EditorGUI.EndDisabledGroup();

                // Cảnh báo nếu offset vượt quá max safe
                float maxOffset = Mathf.Max(Mathf.Abs(effect.shadowOffset.x), Mathf.Abs(effect.shadowOffset.y));
                if (maxOffset > maxSafe.floatValue && maxSafe.floatValue > 0)
                {
                    EditorGUILayout.HelpBox(
                        $"Shadow offset ({maxOffset:F1}) > max safe ({maxSafe.floatValue:F1}). " +
                        "Shadow sẽ bị clamp lại gần text hơn.\n" +
                        "→ Tăng Atlas Padding Override hoặc regenerate font với padding lớn hơn.",
                        MessageType.Warning
                    );
                }
            }
        }
    }
}
