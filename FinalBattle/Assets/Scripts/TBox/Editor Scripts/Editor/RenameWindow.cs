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
                    var assets = Selection.assetGUIDs;

                    foreach (var obj in selectedObjects)
                        obj.name = obj.name.Replace(find, replace);

                    foreach (var asset in assets)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(asset);
                        var name = AssetDatabase.LoadAssetAtPath<Object>(path).name;
                        AssetDatabase.RenameAsset(path, name.Replace(find, replace));
                    }
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
                    var assets = Selection.assetGUIDs;

                    for (int i = 0; i < selectedObjects.Length; i++)
                        selectedObjects[i].name = $"{text}";

                    for (int i = 0; i < assets.Length; i++)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(assets[i]);
                        AssetDatabase.RenameAsset(path, $"{text}");
                    }
                }

                if (GUILayout.Button("Rename with '(0)'"))
                {
                    var selectedObjects = Selection.gameObjects;
                    var assets = Selection.assetGUIDs;

                    for (int i = 0; i < selectedObjects.Length; i++)
                        selectedObjects[i].name = $"{text}({i})";

                    for (int i = 0; i < assets.Length; i++)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(assets[i]);
                        AssetDatabase.RenameAsset(path, $"{text}({i})");
                    }
                }

                if (GUILayout.Button("Rename with ' 0'"))
                {
                    var selectedObjects = Selection.gameObjects;
                    var assets = Selection.assetGUIDs;

                    for (int i = 0; i < selectedObjects.Length; i++)
                        selectedObjects[i].name = $"{text} {i}";

                    for (int i = 0; i < assets.Length; i++)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(assets[i]);
                        AssetDatabase.RenameAsset(path, $"{text} {i}");
                    }
                }

                if (GUILayout.Button("Rename with '_0'"))
                {
                    var selectedObjects = Selection.gameObjects;
                    var assets = Selection.assetGUIDs;

                    for (int i = 0; i < selectedObjects.Length; i++)
                        selectedObjects[i].name = $"{text}_{i}";

                    for (int i = 0; i < assets.Length; i++)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(assets[i]);
                        AssetDatabase.RenameAsset(path, $"{text}_{i}");
                    }
                }
            }

        }

    }

}