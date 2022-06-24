using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Coord))]
public class CoordDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(pos, label, prop);

        // Draw label
        pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        // Calculate percentages
        var labelsWidth = 12f;
        var fieldsWidth = (pos.width - labelsWidth * 2f) / 2f;
        var padding = 2f;
        var off = pos.x;

        // Calculate rects
        var xLbRect = new Rect(off, pos.y, labelsWidth, pos.height);
        off += labelsWidth;
        var xFlRect = new Rect(off, pos.y, fieldsWidth - padding, pos.height);
        off += fieldsWidth + padding;
        var zLbRect = new Rect(off, pos.y, labelsWidth, pos.height);
        off += labelsWidth;
        var zFlRect = new Rect(off, pos.y, fieldsWidth, pos.height);

        // Get fields
        var x = prop.FindPropertyRelative("x");
        var z = prop.FindPropertyRelative("z");

        // Draw fields
        EditorGUI.LabelField(xLbRect, "X");
        EditorGUI.IntField(xFlRect, x.intValue);
        EditorGUI.LabelField(zLbRect, "Z");
        EditorGUI.IntField(zFlRect, z.intValue);
    }

}
