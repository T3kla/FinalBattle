using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }

}

[CustomPropertyDrawer(typeof(Field<>))]
public class FieldDrawer : PropertyDrawer
{

    Rect top, mid, bot;
    SerializedProperty initial, runtime;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        top = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        mid = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
        bot = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight);

        EditorGUI.LabelField(top, label);

        initial = property.FindPropertyRelative("initial");
        runtime = property.FindPropertyRelative("runtime");

        EditorGUI.PropertyField(mid, initial, new GUIContent("      Initial"), true);
        GUI.enabled = false;
        EditorGUI.PropertyField(bot, runtime, new GUIContent("      Runtime"), true);
        GUI.enabled = true;
    }

}
