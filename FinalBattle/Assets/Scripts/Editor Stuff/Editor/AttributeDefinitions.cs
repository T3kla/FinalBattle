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

[CustomPropertyDrawer(typeof(Coord))]
public class CoordDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        var nameWidth = pos.width * .37f;
        var labelWidth = 12f;
        var fieldWidth = ((pos.width - nameWidth) / 2f) - labelWidth;

        var x = prop.FindPropertyRelative("x");
        var y = prop.FindPropertyRelative("z");

        var posX = pos.x;

        EditorGUI.LabelField(new Rect(pos.x, pos.y, nameWidth, pos.height), prop.displayName);
        posX += nameWidth;

        EditorGUI.LabelField(new Rect(posX, pos.y, labelWidth, pos.height), "X"); posX += labelWidth;
        EditorGUI.IntField(new Rect(posX, pos.y, fieldWidth, pos.height), x.intValue); posX += fieldWidth;

        EditorGUI.LabelField(new Rect(posX, pos.y, labelWidth, pos.height), "Z"); posX += labelWidth;
        EditorGUI.DoubleField(new Rect(posX, pos.y, fieldWidth, pos.height), y.intValue); posX += fieldWidth;
    }

}
