using UnityEditor;
using UnityEngine;

namespace TBox
{

    public class AnchorWindow : EditorWindow
    {

        [MenuItem("TBox/Anchoring Window")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<AnchorWindow>();
            window.minSize = new Vector2(100f, 100f);
            window.maxSize = new Vector2(500f, 500f);
        }

        private void OnGUI()
        {
            EditorGUI.indentLevel = 0;

            if (GUILayout.Button("Anchor to Center"))
            {
                var selectedObjects = Selection.gameObjects;
                foreach (var item in selectedObjects)
                    AnchorToCenter(item);
            }

            if (GUILayout.Button("Anchor to Rect"))
            {
                var selectedObjects = Selection.gameObjects;
                foreach (var item in selectedObjects)
                    AnchorToRect(item);
            }
        }

        public void AnchorToCenter(GameObject obj)
        {
            var rt = obj?.GetComponent<RectTransform>();
            var parent = rt?.parent?.GetComponent<RectTransform>();

            if (!rt || !parent)
                return;

            var oldPos = rt.position;

            // Reset pivot
            rt.anchorMin = new Vector2(0f, 0f);
            rt.anchorMax = new Vector2(0f, 0f);
            rt.position = oldPos;

            // Calculate new pivot
            var x = rt.anchoredPosition.x / parent.rect.width;
            var y = rt.anchoredPosition.y / parent.rect.height;

            // Place new pivot
            rt.anchorMin = new Vector2(x, y);
            rt.anchorMax = new Vector2(x, y);

            rt.position = oldPos;
        }

        public virtual void AnchorToRect(GameObject obj)
        {
            var rt = obj?.GetComponent<RectTransform>();
            var parent = rt?.parent?.GetComponent<RectTransform>();

            if (!rt || !parent)
                return;

            var oldPos = rt.position;
            var oldRect = rt.rect;

            // Reset pivot
            rt.anchorMin = new Vector2(0f, 0f);
            rt.anchorMax = new Vector2(0f, 0f);
            rt.position = oldPos;

            // Calculate new pivot
            var minX = (rt.anchoredPosition.x - rt.rect.width / 2f) / parent.rect.width;
            var maxX = (rt.anchoredPosition.x + rt.rect.width / 2f) / parent.rect.width;
            var minY = (rt.anchoredPosition.y - rt.rect.height / 2f) / parent.rect.height;
            var maxY = (rt.anchoredPosition.y + rt.rect.height / 2f) / parent.rect.height;

            // Place new pivot
            rt.anchorMin = new Vector2(minX, minY);
            rt.anchorMax = new Vector2(maxX, maxY);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.position = oldPos;
        }

    }

}