using UnityEditor;
using UnityEngine;

namespace TBox
{

    public class RenameWindow : EditorWindow
    {

        private string find = "";
        private string replace = "";
        private string text = "";

        [MenuItem("TBox/Renaming Window")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<RenameWindow>();
            window.minSize = new Vector2(100f, 100f);
            window.maxSize = new Vector2(500f, 500f);
        }

        private void OnGUI()
        {
            EditorGUI.indentLevel = 0;

            EditorGUILayout.LabelField("Replace");
            EditorGUILayout.Space();

            EditorGUI.indentLevel++;

            {
                EditorGUILayout.HelpBox("Replace 'Find' with 'Replace' in the name of selected assets", MessageType.Info);
                EditorGUILayout.Space();

                find = EditorGUILayout.TextField("Find", find);
                replace = EditorGUILayout.TextField("Replace", replace);
                EditorGUILayout.Space();

                if (GUILayout.Button("Replace"))
                {
                    var selectedObjects = Selection.gameObjects;

                    foreach (var item in selectedObjects)
                        item.name = item.name.Replace(find, replace);
                }
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rename");
            EditorGUILayout.Space();

            EditorGUI.indentLevel++;

            {
                EditorGUILayout.HelpBox("Rename assets with the specified name followed by '(0)', ' 0' or '_0' ", MessageType.Info);
                EditorGUILayout.Space();

                text = EditorGUILayout.TextField("Name", text);
                EditorGUILayout.Space();

                if (GUILayout.Button("Rename"))
                {
                    var selectedObjects = Selection.gameObjects;
                    for (int i = 0; i < selectedObjects.Length; i++)
                        selectedObjects[i].name = $"{text}";
                }

                if (GUILayout.Button("Rename with '(0)'"))
                {
                    var selectedObjects = Selection.gameObjects;
                    for (int i = 0; i < selectedObjects.Length; i++)
                        selectedObjects[i].name = $"{text}({i})";
                }

                if (GUILayout.Button("Rename with ' 0'"))
                {
                    var selectedObjects = Selection.gameObjects;
                    for (int i = 0; i < selectedObjects.Length; i++)
                        selectedObjects[i].name = $"{text} {i}";
                }

                if (GUILayout.Button("Rename with '_0'"))
                {
                    var selectedObjects = Selection.gameObjects;
                    for (int i = 0; i < selectedObjects.Length; i++)
                        selectedObjects[i].name = $"{text}_{i}";
                }
            }

        }

    }

}