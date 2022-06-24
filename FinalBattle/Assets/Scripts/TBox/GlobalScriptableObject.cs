using UnityEditor;
using UnityEngine;
using static TBox.Logger;

namespace TBox
{

    public class GlobalScriptableObject<T> : ScriptableObject where T : GlobalScriptableObject<T>
    {

        protected static T instance;
        public static T Instance => GetReference();

        protected static T GetReference()
        {
            if (instance)
                return instance;

            // Search in assets
            var name = typeof(T).Name;
            var ids = AssetDatabase.FindAssets($"t:{name}");

            if (ids.Length > 0)
                instance = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]), typeof(T)) as T;

            if (ids.Length > 1)
                LogWarn($"Multiple {name} files found");

            if (instance)
                return instance;

            // Create the asset
            instance = CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, $"Assets/{name}.asset");
            AssetDatabase.SetLabels(instance, new[] { $"{name}" });
            AssetDatabase.SaveAssets();

            LogWarn($"Created missing {name} file", "GSO");

            return instance;
        }

    }

}