using UnityEditor;
using UnityEngine;

namespace TBox
{

    [CustomPropertyDrawer(typeof(Field<>))]
    public class FieldDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(pos, label, prop);

            // Draw label
            pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

            // Calculate percentages
            var fieldsWidth = pos.width / 2f;
            var padding = 2f;

            // Calculate rects
            var iRect = new Rect(pos.x, pos.y, fieldsWidth - padding, pos.height);
            var rRect = new Rect(pos.x + fieldsWidth + padding, pos.y, fieldsWidth, pos.height);

            // Get fields
            var initial = prop.FindPropertyRelative("initial");
            var value = prop.FindPropertyRelative("value");

            // Draw fields
            EditorGUI.PropertyField(iRect, initial, GUIContent.none);
            GUI.enabled = false;
            EditorGUI.PropertyField(rRect, value, GUIContent.none);
            GUI.enabled = true;
        }

    }

}
