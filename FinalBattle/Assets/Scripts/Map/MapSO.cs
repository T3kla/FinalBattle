using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]
public class MapSO : ScriptableObject
{

    [Header(" · Tiles")]
    [SerializeField] private float _tileSize; public float tileSize => _tileSize;
    [SerializeField] private float _tileHeight; public float tileHeight => _tileHeight;

}
